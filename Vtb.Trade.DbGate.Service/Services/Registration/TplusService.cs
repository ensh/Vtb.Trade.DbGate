using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<TplusService>), "BindService")]
    public abstract class TplusBase : BaseGrpcService
    {
    }
    public class TplusService : TServiceNameProvider
    {
        public TplusService() { }
        public override string ServiceName => ServiceEndpoints.DbGate.TplusService;
    }
}
