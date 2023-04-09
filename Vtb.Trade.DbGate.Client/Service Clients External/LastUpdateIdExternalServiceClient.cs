using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class LastUpdateIdExternalServiceClient : ServiceClient
    {
        public LastUpdateIdExternalServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public LastUpdateIdExternalServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected LastUpdateIdExternalServiceClient() : base()
        {
        }
        protected LastUpdateIdExternalServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGateAnalit.LastUpdateIdService; }
    }
}
