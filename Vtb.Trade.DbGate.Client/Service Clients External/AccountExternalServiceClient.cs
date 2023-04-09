using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AccountExternalServiceClient : ServiceClient
    {
        public AccountExternalServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public AccountExternalServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected AccountExternalServiceClient() : base()
        {
        }
        protected AccountExternalServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGateAnalit.AccountService; }
    }
}
