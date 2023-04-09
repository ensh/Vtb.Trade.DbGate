using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class RiskInfoServiceClient : ServiceClient
    {
        public RiskInfoServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public RiskInfoServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected RiskInfoServiceClient() : base()
        {
        }
        protected RiskInfoServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGate.RiskInfoService; }
    }
}
