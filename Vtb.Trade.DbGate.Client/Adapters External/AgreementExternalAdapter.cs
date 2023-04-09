using System;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    using IS = IS<AGRA>;

    public class AGRA : IdentitySelectorContext { }

    public class AgreementExternalAdapter : CE_Adapter
    {
        public AgreementExternalAdapter() : base(EntityType.AgreementExternal) { ((CE)this).Fields.Capacity = 12; }
        public AgreementExternalAdapter(CE data) : base(
            (data.EntityType == EntityType.AgreementExternal) ? data : new CE { EntityType = EntityType.AgreementExternal })
        { }

        public override void RegIdentity(Action<CE.FieldValue> registration)
        {
            registration(AgreementNumber);
            registration(ParentAgreementNumber);
            registration(ClientCode);
            registration(ClientTypeCode);
            registration(PackageName);
            registration(ResidentTypeCode);
            registration(RiskCategory);
        }

        public const int iAgreementNumber = 0;
        public const int iClientCode = 1;
        public const int iParentAgreementNumber = 2;
        public const int iClientTypeCode = 3;
        public const int iGkk = 4;
        public const int iPackageName = 5;
        public const int iFullName = 6;
        public const int iSourcePlatformCode = 7;
        public const int iResidentTypeCode = 8;
        public const int iRiskCategory = 9;
        public const int iGroupsFlags = 10;

        public static implicit operator AgreementExternalAdapter(CE entity)
        => new AgreementExternalAdapter(entity);

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

        public int GroupsFlags
        {
            get => this[iGroupsFlags]?.AsInteger ?? default;
            set => this[iGroupsFlags] = (iGroupsFlags + 1, value);
        }
    }
}