using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AccountAddOrUpdResponseAdapter : CE_Adapter
    {
        public const int iAdded = 0;
        public const int iUpdated = 1;

        public AccountAddOrUpdResponseAdapter()
            : base(EntityType.AccountAddOrUpdResult)
        {
        }

        public AccountAddOrUpdResponseAdapter(CE data)
            : base((data.EntityType == EntityType.AccountAddOrUpdResult) ? data : new CE { EntityType = EntityType.AccountAddOrUpdResult })
        {
        }

        public static implicit operator AccountAddOrUpdResponseAdapter(CE entity)
            => new AccountAddOrUpdResponseAdapter(entity);

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
