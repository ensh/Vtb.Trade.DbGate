using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

using Vtb.Trade.Grpc.Common;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Service.Repositories;
using Vtb.Trade.DbGate.Service.Registration;

namespace Vtb.Trade.DbGate.Service
{
    public class SecuritySnapshotAddService : SecuritySnapshotAddBase
    {
        private readonly SecuritySnapshotAddRepository m_repository;
        private readonly ILogger<SecuritySnapshotAddService> m_logger;

        public SecuritySnapshotAddService(
            ILogger<SecuritySnapshotAddService> logger,
            IOptions<RepositoryOptions> repositoryOptions)
        {
            m_logger = logger;
            m_repository = new SecuritySnapshotAddRepository(repositoryOptions.Value);
        }

        public override async Task<CE> OutcomeEx(IAsyncStreamReader<CE> requestStream, ServerCallContext context)
        {
            if (await requestStream.MoveNext())
            {
                var request = (SecuritySnapshotAddRequestAdapter)requestStream.Current;

                m_logger.LogInformation($"Connect Host:{context.Host} Remote:{context.Peer}\n\t" +
                    $"Request:{request.Entity.AsFlatString()}");
                    
                var snapshotStartAdapter = m_repository.GetAdapters(request).Single();
                request.SnapshotId = snapshotStartAdapter.SnapshotId;

                m_logger.LogInformation($"Start snapshot{request.SnapshotId}");

                while (await requestStream.MoveNext() && requestStream.Current.IsNotEmpty())
                {
                    m_logger.LogInformation($"Data Packet Size:{requestStream.Current.CalculateSize()}");

                    request.SnapshotEnities = requestStream.Current.Entities();
                    m_repository.Get(request).Single();
                    request.SnapshotEnities = null;
                }

                m_logger.LogInformation($"Commit snapshot{request.SnapshotId}");

                request.Commit = true;
                m_repository.GetAdapters(request).Single();

                return request;
            }

            return default(CE);
        }
    }
}
