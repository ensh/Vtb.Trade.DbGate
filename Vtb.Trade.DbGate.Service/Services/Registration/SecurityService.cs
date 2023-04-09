using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<SecurityService>), "BindService")]
    public abstract class SecurityBase : BaseGrpcService
    {
    }

    public class SecurityService : TServiceNameProvider
    {
        public SecurityService() { }

        public override string ServiceName => ServiceEndpoints.DbGate.SecurityService;
    }
}
