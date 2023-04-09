using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<AgreementAddOrUpdService>), "BindService")]
    public abstract class AgreementAddOrUpdBase : BaseGrpcService
    {
    }

    public class AgreementAddOrUpdService : TServiceNameProvider
    {
        public AgreementAddOrUpdService() { }

        public override string ServiceName => ServiceEndpoints.DbGate.AgreementAddOrUpdService;
    }
}
