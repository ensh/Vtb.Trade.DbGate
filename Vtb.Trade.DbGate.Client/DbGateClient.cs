using Grpc.Core;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class DbGateClient
    {
        public readonly ILogger<DbGateClient> Logger;
        public readonly Func<GrpcChannel> CreateChannel;
        public int OutcomeConnectionTimeout = 10000;
        public int OutcomeQueueLength = 10;
        public int OutcomeQueueTimeout = 1000;

        public DbGateClient(Func<GrpcChannel> createChannel, ILoggerFactory logFactory)
        {
            Logger = logFactory.CreateLogger<DbGateClient>();
            CreateChannel = createChannel;
        }

        public async Task Query(Func<GrpcChannel, ServiceClient> getClient, IEnumerable<CE> requests, 
            Action<CE> processResult, Action onComplete = null)
        {           
            using var channel = CreateChannel();
            var client = getClient(channel);

            foreach (var request in requests)
            {
                if (Logger.IsEnabled(LogLevel.Debug))
                    Logger.LogDebug(request.AsFlatString());

                var result = await client.QueryAsync(request);

                processResult(result);
            }

            onComplete?.Invoke();
        }

        public async Task IncomeEx(Func<GrpcChannel, ServiceClient> getClient, CE request, Action<CE> processResult, Action onComplete = null)
        {
            Logger.LogInformation(request.AsFlatString());

            using var channel = CreateChannel();
            var client = getClient(channel);

            using var requestMethod = client.IncomeEx(request);

            await foreach (var response in requestMethod.ResponseStream.ReadAllAsync())
            {
                foreach (var item in response.Entities())
                {
                    processResult(item);
                }
            }

            onComplete?.Invoke();
        }

        public async Task PackIncomeEx(Func<GrpcChannel, ServiceClient> getClient, CE request, Action<CE> processPacket, Action onComplete = null)
        {
            Logger.LogInformation(request.AsFlatString());

            using var channel = CreateChannel();
            var client = getClient(channel);

            using var requestMethod = client.IncomeEx(request);

            await foreach (var response in requestMethod.ResponseStream.ReadAllAsync())
            {
                processPacket(response);
            }

            onComplete?.Invoke();
        }

        public async Task<CE> OutcomeEx(Func<GrpcChannel, ServiceClient> getClient, IEnumerable<CE> entities)
        {
            try
            {
                using var channel = CreateChannel();
                var client = getClient(channel);

                using var requestMethod = client.OutcomeEx();

                //requestMethod.RequestStream.WriteOptions = new WriteOptions(0);
                foreach (var entity in entities)
                {
                    await requestMethod.RequestStream.WriteAsync(entity);
                }

                await requestMethod.RequestStream.CompleteAsync();
                return await requestMethod;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }

            return default;
        }

        public async Task<CE> OutcomeEx(Func<GrpcChannel, ServiceClient> getClient
            , Action<(Func<CE, Task> send, Func<Task> cancel)> forSend)
        {
            try
            {
                using var channel = CreateChannel();
                var client = getClient(channel);

                using var requestMethod = client.OutcomeEx();

                var waitLocker = "";
                using var cancellationTokenSource = new CancellationTokenSource();
                var forSendQueue = new ConcurrentQueue<CE>();

                forSend?.Invoke(
                   (
                        message => Send(message, forSendQueue, waitLocker),
                        () => Send(0.CreateParameters(0), forSendQueue, waitLocker)
                    )
                );

                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    lock (waitLocker)
                    {
                        Monitor.Wait(waitLocker, 1000);
                    }

                    while (forSendQueue.TryDequeue(out var entity))
                    {
                        await requestMethod.RequestStream.WriteAsync(entity, cancellationTokenSource.Token);
                        if (entity.IsEmpty())
                        {
                            goto end_data;
                        }
                    }
                }

                end_data:
                await requestMethod.RequestStream.CompleteAsync();
                return await requestMethod;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }

            return default;
        }

        private async Task Send(CE message, ConcurrentQueue<CE> queue, object waitLocker)
        {
            while (queue.Count > OutcomeQueueLength)
            {
                await Task.Delay(OutcomeQueueTimeout);
            }

            lock (waitLocker)
            {
                queue.Enqueue(message);
                Monitor.Pulse(waitLocker);
            }
        }
    }

    public static class DbClientRequests
    {
        public static CE CreateAccountsRequest(string agreementNumber, string exchange)
        {
            var request = new AccountRequestAdapter();

            if (agreementNumber != default)
            {
                request.AgreementNumber = agreementNumber;
            }

            if (exchange != default)
            {
                request.Exchange = exchange;
            }

            request.WithIdentity = true;

            return request;
        }

        public static CE CreateAccountsRequest(int count)
            => new AccountRequestAdapter()
            {
                Count = count,
                WithIdentity = true
            };

        public static CE CreateAgreementsRequest(int count)
            => new AgreementRequestAdapter()
            {
                Count = count,
                WithIdentity = true
            };


        public static CE CreateSecuritySnapshotRequest(int snapshot, ResultObjectEnum resultObject = ResultObjectEnum.Adapter)
            => new SecuritySnapshotRequestAdapter() { SnapshotId = snapshot, ResultsObject = resultObject };

        public static bool StartOutcome(this DbGateClient client, Func<GrpcChannel, ServiceClient> getClient
            , Action<(Func<CE, Task> send, Func<Task> cancel)> forSend, Action<CE> finish)
        {
            async Task Outcome(ManualResetEvent connected)
            {
                var result = await client.OutcomeEx(getClient, forSend + (_ => connected.Set()));
                finish?.Invoke(result);
            }

            using var connected = new ManualResetEvent(false);

            try
            {
                Task.Run(async() => await Outcome(connected));

                return connected.WaitOne(client.OutcomeConnectionTimeout);
            }
            catch (Exception ex)
            {
                client.Logger?.LogError(ex.ToString());
            }

            return false;
        }
    }

    public static class DbClientExternalRequests
    {
        //public static CE CreateRemainRequest(List<DateTime> dates, int placeId = 2)
        //{
        //    var datesString = string.Join(",", dates.Select(d => d.ToString("yyyy-MM-dd")));
        //    var request = new RemainRequestAdapter()
        //    {
        //        Dates = datesString,
        //        PlaceId = placeId
        //    };
        //    return request;
        //}

        public static CE CreateMarginInstrumentsRequest(DateTime date)
        {
            var request = new MarginInstrumentsExternalRequestAdapter()
            {
                Date = date
            };
            return request;
        }

        public static CE CreateMarginInstrumentRatesRequest(DateTime date)
        {
            var request = new MarginInstrumentRatesExternalRequestAdapter()
            {
                Date = date
            };
            return request;
        }

        public static CE CreateAccountsRequest(string agreementNumber, string exchange)
        {
            var request = new AccountRequestAdapter();

            if (agreementNumber != default)
            {
                request.AgreementNumber = agreementNumber;
            }

            if (exchange != default)
            {
                request.Exchange = exchange;
            }

            request.WithIdentity = true;

            return request;
        }

        public static CE CreateAccountsRequest(int count)
            => new AccountRequestAdapter()
            {
                Count = count,
                WithIdentity = true
            };

        public static CE CreateLastUpdateIdRequest()
            => new LastUpdateIdExternalRequestAdapter();

        public static CE CreateAccountChangeRequest(int lastUpdateId, int count)
            => new AccountChangeExternalRequestAdapter()
            {
                LastUpdateId = lastUpdateId,
                Count = count
            };

        public static CE CreateSetProcessedRequest()
            => new SetProcessedExternalRequestAdapter();
    }
}