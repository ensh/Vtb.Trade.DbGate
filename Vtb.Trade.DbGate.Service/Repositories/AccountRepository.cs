using System;
using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service
{
    public class AccountRepository: DbRepository<AccountAdapter, AccountRequestAdapter>
    {
        public AccountRepository(RepositoryOptions repositoryOptions, Action<CE.FieldValue> identityAdd): 
            base(repositoryOptions, identityAdd) {  }

        public override AccountAdapter CreateAdapter() => new AccountAdapter();
    }
}
