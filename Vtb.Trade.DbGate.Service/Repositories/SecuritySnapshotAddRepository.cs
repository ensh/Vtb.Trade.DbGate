using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Common;

namespace Vtb.Trade.DbGate.Service
{
    public class SecuritySnapshotAddRepository : DbRepository<SecuritySnapshotAdapter, SecuritySnapshotAddRequestAdapter>
    {
        public SecuritySnapshotAddRepository(RepositoryOptions repositoryOptions) :
            base(repositoryOptions, null)
        {
        }

        public override SecuritySnapshotAdapter CreateAdapter() => new SecuritySnapshotAdapter();
    }
}
