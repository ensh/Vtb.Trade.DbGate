using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class SetProcessedExternalServiceClient : ServiceClient
    {
        public SetProcessedExternalServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public SetProcessedExternalServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected SetProcessedExternalServiceClient() : base()
        {
        }
        protected SetProcessedExternalServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGateAnalit.SetProcessedService; }
    }
}
