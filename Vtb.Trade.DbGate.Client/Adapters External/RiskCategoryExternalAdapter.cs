using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;
using System;

namespace Vtb.Trade.DbGate.Client
{
    using IS = IS<RiskCategoryCtx>;
    public class RiskCategoryCtx : IdentitySelectorContext { }
    
    public class RiskCategoryExternalAdapter : CE_Adapter
    {
        public RiskCategoryExternalAdapter() : base(EntityType.RiskCategory) { ((CE)this).Fields.Capacity = 13; }
        public RiskCategoryExternalAdapter(CE data) : base(
            (data.EntityType == EntityType.RiskCategory) ? data : new CE { EntityType = EntityType.RiskCategory })
        { }

        public override void RegIdentity(Action<CE.FieldValue> registration)
        {
            registration(Code);
            registration(MarginInstrumentListCode);
        }


        public const int iId = 0;
        public const int iCode = 1;
        public const int iName = 2;
        public const int iMarginInstrumentListCode = 3;
        public const int iRate = 4;
        public const int iBaseRiskCategoryId = 5;
        public const int iQualifiedInvestor = 6;
        public const int iIndividualInvestAccount = 7;
        public const int iMarketplaces = 8;
        public const int iQuikTemplate = 9;
        public const int iQuikLeverageId = 10;
        public const int iIsPartnerQuik = 11;
        public const int iMarginInstrumentListId = 12;

        public long Id 
        {
            get => this[iId]?.AsLong ?? default;
            set => this[iId] = CE.FieldValue.Create(1, value);
        }

        public IS Code 
        {
            get => this[iCode];
            set => this[iCode] = CE.FieldValue.Create(2, value.Text);
        }

        public string Name 
        {
            get => this[iName]?.AsString ?? default;
            set => this[iName] = CE.FieldValue.Create(3, value);
        }

        public IS MarginInstrumentListCode 
        {
            get => this[iMarginInstrumentListCode];
            set => this[iMarginInstrumentListCode] = CE.FieldValue.Create(4, value.Text);
        }

        public decimal Rate 
        {
            get => (decimal)(this[iRate]?.AsDouble ?? default);
            set => this[iRate] = CE.FieldValue.Create(5, value);
        }

        public long BaseRiskCategoryId 
        {
            get => this[iBaseRiskCategoryId]?.AsLong ?? default;
            set => this[iBaseRiskCategoryId] = CE.FieldValue.Create(6, value);
        }

        public int QualifiedInvestor 
        {
            get => this[iQualifiedInvestor]?.AsInteger ?? default;
            set => this[iQualifiedInvestor] = CE.FieldValue.Create(7, value);
        }

        public int IndividualInvestmentAccount 
        {
            get => this[iIndividualInvestAccount]?.AsInteger ?? default;
            set => this[iIndividualInvestAccount] = CE.FieldValue.Create(8, value);
        }

        public string Marketplaces 
        {
            get => this[iMarketplaces]?.AsString ?? default;
            set => this[iMarketplaces] = CE.FieldValue.Create(9, value);
        }

        public string QuikTemplate 
        {
            get => this[iQuikTemplate]?.AsString ?? default;
            set => this[iQuikTemplate] = CE.FieldValue.Create(10, value);
        }

        public long QuikLeverageId
        {
            get => this[iQuikLeverageId]?.AsLong ?? default;
            set => this[iQuikLeverageId] = CE.FieldValue.Create(11, value);
        }

        public bool IsPartnerQuik 
        {
            get => this[iIsPartnerQuik]?.AsBoolean ?? false;
            set => this[iIsPartnerQuik] = CE.FieldValue.Create(12, value);
        }

        public long MarginInstrumentListId
        {
            get => this[iMarginInstrumentListId]?.AsLong ?? default;
            set => this[iMarginInstrumentListId] = CE.FieldValue.Create(13, value);
        }
    }
}
