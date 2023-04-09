using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AgreementAddOrUpdServiceClient : ServiceClient
    {
        public AgreementAddOrUpdServiceClient(ChannelBase channel) : base(channel)
        {
        }

        public AgreementAddOrUpdServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }

        protected AgreementAddOrUpdServiceClient() : base()
        {
        }

        protected AgreementAddOrUpdServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGate.AgreementAddOrUpdService; }
    }
}
