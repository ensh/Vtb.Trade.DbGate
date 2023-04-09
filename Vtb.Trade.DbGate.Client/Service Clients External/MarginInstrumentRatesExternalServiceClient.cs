using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class MarginInstrumentRatesExternalServiceClient : ServiceClient
    {
        public MarginInstrumentRatesExternalServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public MarginInstrumentRatesExternalServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected MarginInstrumentRatesExternalServiceClient() : base()
        {
        }
        protected MarginInstrumentRatesExternalServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGateAnalit.MarginInstrumentRatesService; }
    }
}
