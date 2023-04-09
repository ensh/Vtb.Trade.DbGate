using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Common;

namespace Vtb.Trade.DbGate.Service
{
    public class AccountAddOrUpdRepository : DbRepository<AccountAddOrUpdResponseAdapter, AccountAddOrUpdRequestAdapter>
    {
        public AccountAddOrUpdRepository(RepositoryOptions repositoryOptions) :
            base(repositoryOptions, null)
        {
        }

        public override AccountAddOrUpdResponseAdapter CreateAdapter() => new AccountAddOrUpdResponseAdapter();
    }
}
