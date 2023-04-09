using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class MarginInstrumentsExternalServiceClient : ServiceClient
    {
        public MarginInstrumentsExternalServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public MarginInstrumentsExternalServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected MarginInstrumentsExternalServiceClient() : base()
        {
        }
        protected MarginInstrumentsExternalServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGateAnalit.MarginInstrumentsService; }
    }
}
