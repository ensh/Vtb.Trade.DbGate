using System;
using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Service
{
    public class AgreementRepository: DbRepository<AgreementAdapter, AgreementRequestAdapter>
    {
        public AgreementRepository(RepositoryOptions repositoryOptions, Action<CE.FieldValue> identityAdd) 
            : base (repositoryOptions, identityAdd)
        {
        }
        public override AgreementAdapter CreateAdapter() => new AgreementAdapter();
    }
}