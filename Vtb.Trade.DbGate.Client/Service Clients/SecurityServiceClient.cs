using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class SecurityServiceClient : ServiceClient
    {
        public SecurityServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public SecurityServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected SecurityServiceClient() : base()
        {
        }
        protected SecurityServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGate.SecurityService; }
    }
}
