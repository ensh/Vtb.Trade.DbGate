using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<BoardService>), "BindService")]
    public abstract class BoardBase : BaseGrpcService
    {
    }

    public class BoardService : TServiceNameProvider
    {
        public BoardService() { }
        public override string ServiceName => ServiceEndpoints.DbGate.BoardService;
    }
}
