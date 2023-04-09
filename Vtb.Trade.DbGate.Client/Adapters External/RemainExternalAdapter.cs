using System;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    using IS = IS<RemainCtx>;

    public class RemainCtx : IdentitySelectorContext { }
    public class RemainExternalAdapter : CE_Adapter
    {
        public RemainExternalAdapter() : base(EntityType.Remain) { ((CE)this).Fields.Capacity = 6; } 
        public RemainExternalAdapter(CE data) : base(
            (data.EntityType == EntityType.Remain) ? data : new CE { EntityType = EntityType.Remain })
        { }

        public override void RegIdentity(Action<CE.FieldValue> registration)
        {
            registration(Account);
            registration(TradeKey);
            registration(SecurityCode);

        }

        private int _accountIdx = 0;
        private int _tradeKeyIdx = 1;
        private int _securityCodeIdx = 2;
        private int _dateIdx = 3;
        private int _remainIdx = 4;
        

        public IS Account 
        {
            get => this[_accountIdx];
            set => this[_accountIdx] = CE.FieldValue.Create(1, value.Text);
        }

        public IS TradeKey
        {
            get => this[_tradeKeyIdx];
            set => this[_tradeKeyIdx] = CE.FieldValue.Create(2, value.Text);
        }

        public IS SecurityCode
        {
            get => this[_securityCodeIdx];
            set => this[_securityCodeIdx] = CE.FieldValue.Create(3, value.Text);
        }

        public DateTime OnDate
        {
            get => this[_dateIdx]?.AsDateTime ?? default;
            set => this[_dateIdx] = CE.FieldValue.Create(4, value);
        }

        public decimal Remain
        {
            get => (decimal)(this[_remainIdx]?.AsDouble ?? default);
            set => this[_remainIdx] = CE.FieldValue.Create(5, value);
        }
    }
}