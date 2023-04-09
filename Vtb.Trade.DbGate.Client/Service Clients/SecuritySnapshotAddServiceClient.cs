using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class SecuritySnapshotAddServiceClient : ServiceClient
    {
        public SecuritySnapshotAddServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public SecuritySnapshotAddServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected SecuritySnapshotAddServiceClient() : base()
        {
        }
        protected SecuritySnapshotAddServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGate.SecuritySnapshotAddService; }
    }
}
