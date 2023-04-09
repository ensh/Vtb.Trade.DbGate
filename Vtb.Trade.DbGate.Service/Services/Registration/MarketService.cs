using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<MarketService>), "BindService")]
    public abstract class MarketBase : BaseGrpcService
    {
    }

    public class MarketService : TServiceNameProvider
    {
        public MarketService() { }
        public override string ServiceName => ServiceEndpoints.DbGate.MarketService;
    }
}
