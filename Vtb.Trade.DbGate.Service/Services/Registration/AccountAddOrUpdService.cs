using Grpc.Core;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service.Registration
{
    [BindServiceMethod(typeof(BaseServiceRegistration<AccountAddOrUpdService>), "BindService")]
    public abstract class AccountAddOrUpdBase : BaseGrpcService
    {
    }

    public class AccountAddOrUpdService : TServiceNameProvider
    {
        public AccountAddOrUpdService() { }

        public override string ServiceName => ServiceEndpoints.DbGate.AccountAddOrUpdService;
    }
}
