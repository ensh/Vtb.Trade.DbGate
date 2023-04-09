using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class SecuritySnapshotServiceClient : ServiceClient
    {
        public SecuritySnapshotServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public SecuritySnapshotServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected SecuritySnapshotServiceClient() : base()
        {
        }
        protected SecuritySnapshotServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGate.SecuritySnapshotService; }
    }
}
