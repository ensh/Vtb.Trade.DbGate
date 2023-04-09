using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<AgreementService>), "BindService")]
    public abstract class AgreementBase : BaseGrpcService
    {
    }
    public class AgreementService : TServiceNameProvider
    {
        public AgreementService() { }
        public override string ServiceName => ServiceEndpoints.DbGate.AgreementService;
    }
}
