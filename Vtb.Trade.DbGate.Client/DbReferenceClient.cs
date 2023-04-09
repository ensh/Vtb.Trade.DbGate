using Grpc.Core;

using Vtb.Trade.Grpc.Common;
using Vtb.Trade.Configuration.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class DbReferenceServiceClient : ServiceClient
    {
        public DbReferenceServiceClient(ChannelBase channel) : base(channel)
        {
        }
        public DbReferenceServiceClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }
        protected DbReferenceServiceClient() : base()
        {
        }
        protected DbReferenceServiceClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public override string ServiceName { get => ServiceEndpoints.DbGate.DbReferenceService; }
    }
}

