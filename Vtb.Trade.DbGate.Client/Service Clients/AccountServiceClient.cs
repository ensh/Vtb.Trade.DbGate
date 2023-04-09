using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AccountServiceClient : ServiceClient
    {
        public AccountServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public AccountServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected AccountServiceClient() : base()
        {
        }
        protected AccountServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGate.AccountService; }
    }
}
