using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Common;

namespace Vtb.Trade.DbGate.Service.Repositories
{
    public class SecuritySnapshotRepository : DbRepository<SecuritySnapshotAdapter, SecuritySnapshotRequestAdapter>
    {
        public SecuritySnapshotRepository(RepositoryOptions repositoryOptions) :
            base(repositoryOptions, null)
        {
        }

        public override SecuritySnapshotAdapter CreateAdapter() => new SecuritySnapshotAdapter();
    }
}
