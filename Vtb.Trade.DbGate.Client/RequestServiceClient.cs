using Grpc.Core;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class RequestServiceClient : ServiceClient
    {
        public RequestServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public RequestServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected RequestServiceClient() : base()
        {
        }
        protected RequestServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGate.RequestService; }
    }
}
