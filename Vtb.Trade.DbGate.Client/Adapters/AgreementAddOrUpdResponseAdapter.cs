using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AgreementAddOrUpdResponseAdapter : CE_Adapter
    {
        public const int iAdded = 0;
        public const int iUpdated = 1;

        public AgreementAddOrUpdResponseAdapter()
            : base(EntityType.AccountAddOrUpdResult)
        {
        }

        public AgreementAddOrUpdResponseAdapter(CE data)
            : base((data.EntityType == EntityType.AgreementAddOrUpdResult) ? data : new CE { EntityType = EntityType.AgreementAddOrUpdResult })
        {
        }

        public static implicit operator AgreementAddOrUpdResponseAdapter(CE entity)
            => new AgreementAddOrUpdResponseAdapter(entity);

        public int Added
        {
            get => this[iAdded]?.AsInteger ?? default;
            set => this[iAdded] = CE.FieldValue.Create(iAdded + 1, value);
        }

        public int Updated
        {
            get => this[iUpdated]?.AsInteger ?? default;
            set => this[iUpdated] = CE.FieldValue.Create(iUpdated + 1, value);
        }
    }
}
