using Grpc.Core;

using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<SpecialDatesService>), "BindService")]
    public abstract class SpecialDatesBase : BaseGrpcService
    {
    }

    public class SpecialDatesService : TServiceNameProvider
    {
        public SpecialDatesService() { }
        public override string ServiceName => ServiceEndpoints.DbGate.SpecialDatesService;
    }
}
