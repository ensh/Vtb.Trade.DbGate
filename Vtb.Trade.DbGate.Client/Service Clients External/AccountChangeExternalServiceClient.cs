using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AccountChangeExternalServiceClient : ServiceClient
    {
        public AccountChangeExternalServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public AccountChangeExternalServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected AccountChangeExternalServiceClient() : base()
        {
        }
        protected AccountChangeExternalServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGateAnalit.AccountChangeService; }
    }
}
