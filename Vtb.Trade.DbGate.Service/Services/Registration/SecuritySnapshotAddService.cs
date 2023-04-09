using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<SecuritySnapshotAddService>), "BindService")]
    public abstract class SecuritySnapshotAddBase : BaseGrpcService
    {
    }

    public class SecuritySnapshotAddService : TServiceNameProvider
    {
        public SecuritySnapshotAddService() { }

        public override string ServiceName => ServiceEndpoints.DbGate.SecuritySnapshotAddService;
    }
}
