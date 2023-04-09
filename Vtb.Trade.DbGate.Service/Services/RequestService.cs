using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.DbGate.Service;
using Vtb.Trade.Identity.Client;
using Vtb.Trade.Grpc.Common;


namespace Vtb.Trade.DbGate.Service
{
    public class RequestService : Registration.RequestBase
    {
        private static int s_number = 0;
        private readonly string m_number;
        private readonly ILoggerFactory m_logFactory;
        private readonly ILogger<RequestService> m_logger;
        private readonly RepositoryOptions m_repositoryOptions;
        private readonly Func<string, RequestOptions> m_optionsGetter;
        private readonly (Action<CE.FieldValue> add, Action post) m_identityCommands;

        public RequestService(Func<string, RequestOptions> optionsGetter, IOptions<RepositoryOptions> repositoryOptions,
            IdentityGetter identityGetter, ILoggerFactory logFactory)
        {
            m_logger = (m_logFactory = logFactory).CreateLogger<RequestService>();
            m_number = $"{Interlocked.Increment(ref s_number)}";
            identityGetter.Create(out m_identityCommands);
            m_optionsGetter = optionsGetter;
            m_repositoryOptions = repositoryOptions.Value;
        }

        public override async Task<CE> Query(CE request, ServerCallContext context)
        {
            m_logger.LogInformation($"({m_number}) Host:{context.Host}, Remote:{context.Peer}\n\t");

            if (m_logger.IsEnabled(LogLevel.Debug))
            {
                m_logger.LogDebug($"({m_number}) Request:{request.AsFlatString()}");
            }

            try
            {
                var requestName = RequestName(request);
                var requestParams = request.Params(1);

                var requestOptions = m_optionsGetter(requestName);
                var results = 0.CreateParameters(request.Count());

                var parameters = requestParams.Select(p => (p.Name, p.AsObject()));
                var repository = new DbRequestRepository(m_repositoryOptions, m_identityCommands.add,
                    m_logFactory.CreateLogger<DbRequestRepository>());

                var getResults = repository.Get(requestOptions, parameters, request.Entities());

                results.Append(getResults);

                m_identityCommands.post();

                if (m_logger.IsEnabled(LogLevel.Debug))
                {
                    m_logger.LogDebug(results.AsFlatString());
                }

                return results;
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                m_logger.LogError(ex.ToString(), $"({m_number})RequestService.Query");
            }
            m_logger.LogInformation($"({m_number}) Host:{context.Host}, Remote:{context.Peer} finish");

            return new CE();
        }


        private IEnumerable<CE> IncomeEx(RequestOptions requestOptions, IEnumerable<CE_Param> requestParams)
        {
            var parameters = requestParams.Select(p => (p.Name, p.AsObject()));
            var repository = new DbRequestRepository(m_repositoryOptions, m_identityCommands.add, 
                m_logFactory.CreateLogger<DbRequestRepository>());

            if (m_logger.IsEnabled(LogLevel.Debug))
            {
                return GetWithDebug(repository.Get(requestOptions, parameters));
            }
            else
            {
                return repository.Get(requestOptions, parameters);
            }
        }

        public override async Task IncomeEx(CE request, IServerStreamWriter<CE> responseStream, ServerCallContext context)
        {
            m_logger.LogInformation($"({m_number}) Host:{context.Host}, Remote:{context.Peer}\n\t" +
                $"Request:{request.AsFlatString()}");
            try
            {
                var requestName = RequestName(request);
                var requestParams = request.Params(1);

                var requestOptions = m_optionsGetter(requestName);

                await IncomeEx(requestOptions, requestParams)
                        .IncomeEx(responseStream, 
                        m_identityCommands.post +
                        context.CancellationToken.ThrowIfCancellationRequested, requestOptions.PacketSize);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                m_logger.LogError(ex.ToString(), $"({m_number})RequestService.IncomeEx");
            }
            m_logger.LogInformation($"({m_number}) Host:{context.Host}, Remote:{context.Peer} finish");
        }

        private string RequestName(CE request) => request.Strings().First();

        private IEnumerable<CE> GetWithDebug(IEnumerable<CE> items)
        {
            foreach (var item in items)
            {
                m_logger.LogDebug($"{m_number}=>{item.AsFlatString()}");
                yield return item;
            }
        }

        public override async Task<CE> OutcomeEx(IAsyncStreamReader<CE> requestStream, ServerCallContext context)
        {
            var results = 0.CreateParameters();

            try
            {
                if (await requestStream.MoveNext())
                {
                    var request = requestStream.Current;

                    m_logger.LogInformation($"({m_number}) Host:{context.Host} Remote:{context.Peer}\n\t" +
                        $"Request:{request.AsFlatString()}");

                    var requestName = RequestName(request);
                    var requestParams = request.Params(1);
                    var requestOptions = m_optionsGetter(requestName);

                    var parameters = requestParams.Select(p => (p.Name, p.AsObject()));
                    var repository = new DbRequestRepository(m_repositoryOptions, m_identityCommands.add,
                        m_logFactory.CreateLogger<DbRequestRepository>());

                    while (await requestStream.MoveNext() && requestStream.Current.IsNotEmpty())
                    {
                        if (m_logger.IsEnabled(LogLevel.Debug))
                        {
                            m_logger.LogDebug(requestStream.Current.AsFlatString());
                        }

                        m_logger.LogInformation($"Request with {requestStream.Current.Entities().Count()} entities");

                        var data = requestStream.Current.Entities();
                        results.Append(repository.Get(requestOptions, parameters, data));
                    }

                    m_logger.LogDebug(results.AsFlatString());
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                m_logger.LogError(ex.ToString(), $"({m_number}) RequestService.OutcomeEx");
            }
            m_logger.LogInformation($"({m_number}) Host:{context.Host}, Remote:{context.Peer} finish");

            return results;
        }
    }
}
