using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<AssetGetService>), "BindService")]
    public abstract class AssetGetBase : BaseGrpcService
    {
    }

    public class AssetGetService : TServiceNameProvider
    {
        public AssetGetService() { }

        public override string ServiceName => ServiceEndpoints.DbGate.AssetGetService;
    }
}
