using Grpc.Core;

using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<RequestService>), "BindService")]
    public abstract class RequestBase : BaseGrpcService
    {
    }

    public class RequestService : TServiceNameProvider
    {
        public RequestService() { }
        public override string ServiceName => ServiceEndpoints.DbGate.RequestService;
    }
}
