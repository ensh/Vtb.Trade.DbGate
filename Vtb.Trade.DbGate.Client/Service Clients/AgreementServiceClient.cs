using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AgreementServiceClient : ServiceClient
    {
        public AgreementServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public AgreementServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected AgreementServiceClient() : base()
        {
        }
        protected AgreementServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGate.AgreementService; }
    }
}
