using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.DbGate.Service.Registration;

using Vtb.Trade.Identity.Client;

namespace Vtb.Trade.DbGate.Service
{
    public class AccountService : AccountBase
    {
        private static int s_number;
        private readonly string m_number;
        private readonly AccountRepository m_repository;
        private readonly ILogger<AccountService> m_logger;
        private readonly (Action<CE.FieldValue> add, Action post) m_identityCommands;

        public AccountService(ILogger<AccountService> logger, IOptions<RepositoryOptions> repositoryOptions,
            IdentityGetter identityGetter)
        {
            m_logger = logger;
            m_number = $"({Interlocked.Increment(ref s_number)}) AccountService,";
            identityGetter.Create(out m_identityCommands);
            m_repository = new AccountRepository(repositoryOptions.Value, m_identityCommands.add);
        }

        public override async Task IncomeEx(CE request, IServerStreamWriter<CE> responseStream, ServerCallContext context)
        {
            m_logger.LogInformation($"{m_number} Host:{context.Host}, Remote:{context.Peer}\n\t" +
                $"Request:{request.AsFlatString()}");

            var requestAdapter = (AccountRequestAdapter)request;

            if (m_logger.IsEnabled(LogLevel.Debug))
                await GetWithDebug(requestAdapter)
                    .IncomeEx(responseStream, m_identityCommands.post, requestAdapter.PacketSize);
            else
                await m_repository.Get(requestAdapter)
                    .IncomeEx(responseStream, m_identityCommands.post, requestAdapter.PacketSize);

            m_logger.LogInformation($"{m_number} Host:{context.Host}, Remote:{context.Peer} finish");
        }

        private IEnumerable<CE> GetWithDebug(AccountRequestAdapter request)
        {
            foreach (var result in m_repository.Get(request))
            {
                m_logger.LogDebug($"({m_number})=>{result.AsFlatString()}");
                yield return result;
            }
        }
    }
}
