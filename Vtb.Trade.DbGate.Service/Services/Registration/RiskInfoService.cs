using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<RiskInfoService>), "BindService")]
    public abstract class RiskInfoBase : BaseGrpcService
    {
    }

    public class RiskInfoService : TServiceNameProvider
    {
        public RiskInfoService() { }
        public override string ServiceName => ServiceEndpoints.DbGate.RiskInfoService;
    }
}
