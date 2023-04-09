using System;

using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    using IS = IS<ACCA>;

    public class ACCA : IdentitySelectorContext { }

    public class AccountExternalAdapter : CE_Adapter
    {
        public AccountExternalAdapter() : base(EntityType.AccountExternal) { ((CE)this).Fields.Capacity = 17; }

        public AccountExternalAdapter(CE data) : base(
            (data.EntityType == EntityType.AccountExternal) ? data : new CE { EntityType = EntityType.AccountExternal })
        { }

        public override void RegIdentity(Action<CE.FieldValue> registration)
        {
            registration(FirmTradingAccountCode);
            registration(FortsTradingAccount);
            registration(ExchangeCode);
            registration(AgreementNumber);
            registration(ClientCode);
            registration(ParentAgreementNumber);
            registration(ClientTypeCode);
            registration(PackageName);
            registration(ResidentTypeCode);
            registration(RiskCategory);
            registration(TradingAccountStatusCode);
        }

        public const int iFirmTradingAccountCode = 0;
        public const int iFortsTradingAccount = 1;
        public const int iExchangeCode = 2;
        public const int iAgreementNumber = 3;
        public const int iClientCode = 4;
        public const int iParentAgreementNumber = 5;
        public const int iClientTypeCode = 6;
        public const int iGkk = 7;
        public const int iPackageName = 8;
        public const int iFullName = 9;
        public const int iSourcePlatformCode = 10;
        public const int iResidentTypeCode = 11;
        public const int iRiskCategory = 12;
        public const int iAgreementGroupsFlags = 13;
        public const int iTradingAccountStatusCode = 14;
        public const int iGroupsFlags = 15;

        public static implicit operator AccountExternalAdapter(CE entity) => new AccountExternalAdapter(entity);

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
        
        public IS ClientCode
        {
            get => this[iClientCode];
            set => this[iClientCode] = value.AsField(iClientCode + 1);
        }

        public IS ParentAgreementNumber
        {
            get => this[iParentAgreementNumber];
            set => this[iParentAgreementNumber] = value.AsField(iParentAgreementNumber + 1);
        }

        public IS ClientTypeCode
        {
            get => this[iClientTypeCode];
            set => this[iClientTypeCode] = value.AsField(iClientTypeCode + 1);
        }

        public int Gkk
        {
            get => this[iGkk]?.AsInteger ?? default;
            set => this[iGkk] = (iGkk + 1, value);
        }

        public IS PackageName
        {
            get => this[iPackageName];
            set => this[iPackageName] = value.AsField(iPackageName + 1);
        }

        public string FullName
        {
            get => this[iFullName]?.AsString ?? default;
            set => this[iFullName] = (iFullName + 1, value);
        }

        public string SourcePlatformCode
        {
            get => this[iSourcePlatformCode]?.AsString ?? default;
            set => this[iSourcePlatformCode] = (iSourcePlatformCode + 1, value);
        }

        public IS ResidentTypeCode
        {
            get => this[iResidentTypeCode];
            set => this[iResidentTypeCode] = value.AsField(iResidentTypeCode + 1);
        }

        public IS RiskCategory
        {
            get => this[iRiskCategory];
            set => this[iRiskCategory] = value.AsField(iRiskCategory + 1);
        }

        public int AgreementGroupsFlags
        {
            get => this[iAgreementGroupsFlags]?.AsInteger ?? default;
            set => this[iAgreementGroupsFlags] = (iAgreementGroupsFlags + 1, value);
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
