using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<RemainService>), "BindService")]
    public abstract class RemainBase : BaseGrpcService
    {
    }
    public class RemainService : TServiceNameProvider
    {
        public RemainService() { }
        public override string ServiceName => ServiceEndpoints.DbGate.RemainService;
    }
}
