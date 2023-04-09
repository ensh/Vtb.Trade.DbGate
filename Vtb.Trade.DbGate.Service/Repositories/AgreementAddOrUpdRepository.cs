using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Common;

namespace Vtb.Trade.DbGate.Service
{
    public class AgreementAddOrUpdRepository : DbRepository<AgreementAddOrUpdResponseAdapter, AgreementAddOrUpdRequestAdapter>
    {
        public AgreementAddOrUpdRepository(RepositoryOptions repositoryOptions) :
            base(repositoryOptions, null)
        {
        }

        public override AgreementAddOrUpdResponseAdapter CreateAdapter() => new AgreementAddOrUpdResponseAdapter();
    }
}
