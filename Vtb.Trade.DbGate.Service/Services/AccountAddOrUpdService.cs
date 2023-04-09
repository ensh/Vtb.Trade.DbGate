using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.DbGate.Service.Registration;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service
{
    public class AccountAddOrUpdService : AccountAddOrUpdBase
    {
        private readonly AccountAddOrUpdRepository m_repository;
        private readonly ILogger<AccountAddOrUpdService> m_logger;

        public AccountAddOrUpdService(
            ILogger<AccountAddOrUpdService> logger,
            IOptions<RepositoryOptions> repositoryOptions)
        {
            m_logger = logger;
            m_repository = new AccountAddOrUpdRepository(repositoryOptions.Value);
        }

        public override async Task<CE> OutcomeEx(IAsyncStreamReader<CE> requestStream, ServerCallContext context)
        {
            int addedCount = 0;
            int updatedCount = 0;

            if (await requestStream.MoveNext())
            {
                CE ce = requestStream.Current;
                m_logger.LogInformation($"Connect Host:{context.Host}; Remote:{context.Peer}; Request:{ce.AsFlatString()}");
                AccountAddOrUpdRequestAdapter request = ce;
                int packetNum = 0;

                while(await requestStream.MoveNext())
                {
                    if (requestStream.Current.IsEmpty())
                    {
                        break;
                    }

                    packetNum++;
                    request.Accounts = requestStream.Current.Entities();

                    AccountAddOrUpdResponseAdapter response = m_repository.GetAdapters(request).Single();
                    m_logger.LogInformation($"Packet #{packetNum}; Added: {response.Added}; Updated: {response.Updated}");

                    addedCount += response.Added;
                    updatedCount += response.Updated;
                }
            }

            return new AccountAddOrUpdResponseAdapter { Added = addedCount, Updated = updatedCount };
        }
    }
}
