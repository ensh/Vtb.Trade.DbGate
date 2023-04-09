using System;

using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    using IS = IS<ACC>;

    public class ACC : IdentitySelectorContext { }

    public class AccountAdapter : CE_Adapter
    {
        public AccountAdapter() : base(EntityType.Account) { ((CE)this).Fields.Capacity = 7; }
        public AccountAdapter(CE data) : base(
            (data.EntityType == EntityType.Account) ? data : new CE { EntityType = EntityType.Account })
        { }

        public override void RegIdentity(Action<CE.FieldValue> registration)
        {
            registration(FirmTradingAccountCode);
            registration(FortsTradingAccount);
            registration(ExchangeCode);
            registration(AgreementNumber);
            registration(TradingAccountStatusCode);
        }

        public const int iFirmTradingAccountCode = 0;
        public const int iFortsTradingAccount = 1;
        public const int iExchangeCode = 2;
        public const int iAgreementNumber = 3;
        public const int iTradingAccountStatusCode = 4;
        public const int iGroupsFlags = 5;

        public static implicit operator AccountAdapter(CE entity)
        => new AccountAdapter(entity);

        /// <summary>
        /// Код фирмы
        /// </summary>
        public IS FirmTradingAccountCode
        {
            get => this[iFirmTradingAccountCode];
            set => this[iFirmTradingAccountCode] = value.AsField(iFirmTradingAccountCode + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public IS FortsTradingAccount
        {
            get => this[iFortsTradingAccount];
            set => this[iFortsTradingAccount] = value.AsField(iFortsTradingAccount + 1);
        }

        /// <summary>
        /// Код биржи
        /// </summary>
        public IS ExchangeCode
        {
            get => this[iExchangeCode];
            set => this[iExchangeCode] = value.AsField(iExchangeCode + 1);
        }

        /// <summary>
        /// Номер соглашения	
        /// </summary>
        public IS AgreementNumber
        {
            get => this[iAgreementNumber];
            set => this[iAgreementNumber] = value.AsField(iAgreementNumber + 1);
        }

        /// <summary>
        /// Статус торгового счета
        /// </summary>
        public IS TradingAccountStatusCode
        {
            get => this[iTradingAccountStatusCode];
            set => this[iTradingAccountStatusCode] = value.AsField(iTradingAccountStatusCode + 1);
        }

        /// <summary>
        /// Флаги доступности
        /// </summary>
        public int GroupsFlags
        {
            get => this[iGroupsFlags]?.AsInteger ?? default;
            set => this[iGroupsFlags] = (iGroupsFlags + 1, value);
        }
    }
}
