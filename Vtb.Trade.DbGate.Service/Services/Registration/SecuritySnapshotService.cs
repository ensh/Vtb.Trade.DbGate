using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<SecuritySnapshotService>), "BindService")]
    public abstract class SecuritySnapshotGetBase : BaseGrpcService
    {
    }

    public class SecuritySnapshotService : TServiceNameProvider
    {
        public SecuritySnapshotService() { }

        public override string ServiceName => ServiceEndpoints.DbGate.SecuritySnapshotService;
    }
}
