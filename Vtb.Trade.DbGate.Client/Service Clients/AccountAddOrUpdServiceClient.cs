using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AccountAddOrUpdServiceClient : ServiceClient
    {
        public AccountAddOrUpdServiceClient(ChannelBase channel) : base(channel)
        {
        }

        public AccountAddOrUpdServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }

        protected AccountAddOrUpdServiceClient() : base()
        {
        }

        protected AccountAddOrUpdServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGate.AccountAddOrUpdService; }
    }
}
