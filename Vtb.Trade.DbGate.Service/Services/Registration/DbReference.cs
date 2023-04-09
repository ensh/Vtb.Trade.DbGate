using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<DbReferenceService>), "BindService")]
    public abstract class DbReferenceBase : BaseGrpcService
    {
    }

    public class DbReferenceService : TServiceNameProvider
    {
        public DbReferenceService() { }
        public override string ServiceName => ServiceEndpoints.DbGate.DbReferenceService;
    }
}
