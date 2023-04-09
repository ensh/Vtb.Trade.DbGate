using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<AccountService>), "BindService")]
    public abstract class AccountBase : BaseGrpcService
    {
    }

    public class AccountService : TServiceNameProvider
    {
        public AccountService() { }
        public override string ServiceName => ServiceEndpoints.DbGate.AccountService;
    }
}
