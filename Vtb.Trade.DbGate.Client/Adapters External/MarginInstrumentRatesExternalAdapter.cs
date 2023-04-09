using System;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;


namespace Vtb.Trade.DbGate.Client
{
    using IS = IS<MarginInstrumentRatesCtx>;

    public class MarginInstrumentRatesCtx : IdentitySelectorContext { }
    public class MarginInstrumentRatesExternalAdapter : CE_Adapter
    {
        public MarginInstrumentRatesExternalAdapter() : base(EntityType.MarginInstrumentRates) { ((CE)this).Fields.Capacity = 15; }
        public MarginInstrumentRatesExternalAdapter(CE data) : base(
            (data.EntityType == EntityType.MarginInstrumentRates) ? data : new CE { EntityType = EntityType.MarginInstrumentRates })
        { }

        public override void RegIdentity(Action<CE.FieldValue> registration)
        {
            registration(Ticker);
            registration(Isin);
            registration(ListCode);
        }

        private const int iTICKER = 0;
        private const int iLIST_CODE = 1;
        private const int iISIN = 2;
        private const int iIS_VTB_ONLY = 3;
        private const int iIS_EXCHANGE_REPO = 4;
        private const int iIS_DELETED = 5;
        private const int iSHORT_NAME = 6;
        private const int iRATE_SHORT = 7;
        private const int iRATE_LONG = 8;
        private const int iRATE_SHORT_STANDART = 9;
        private const int iRATE_LONG_STANDART = 10;
        private const int iIS_LONG = 11;
        private const int iIS_SHORT = 12;
        private const int iIS_MARGINAL = 13;
        private const int iIS_RESTRICTED = 14;

        public IS Ticker
        {
            get => this[iTICKER];
            set => this[iTICKER] = CE.FieldValue.Create(iTICKER + 1, value.Text);
        }

        public IS ListCode
        {
            get => this[iLIST_CODE];
            set => this[iLIST_CODE] = CE.FieldValue.Create(iLIST_CODE + 1, value.Text);
        }

        public IS Isin
        {
            get => this[iISIN];
            set => this[iISIN] = CE.FieldValue.Create(iISIN + 1, value.Text);
        }

        public bool IsVtbOnly
        {
            get => this[iIS_VTB_ONLY]?.AsBoolean ?? false;
            set => this[iIS_VTB_ONLY] = CE.FieldValue.Create(iIS_VTB_ONLY + 1, value);
        }

        public bool IsExchangeRepo
        {
            get => this[iIS_EXCHANGE_REPO]?.AsBoolean ?? false;
            set => this[iIS_EXCHANGE_REPO] = CE.FieldValue.Create(iIS_EXCHANGE_REPO + 1, value);
        }

        public bool IsDeleted
        {
            get => this[iIS_DELETED]?.AsBoolean ?? false;
            set => this[iIS_DELETED] = CE.FieldValue.Create(iIS_DELETED + 1, value);
        }

        public string ShortName
        {
            get => this[iSHORT_NAME]?.AsString ?? string.Empty;
            set => this[iSHORT_NAME] = CE.FieldValue.Create(iSHORT_NAME + 1, value);
        }

        public decimal RateShort
        {
            get => (decimal)(this[iRATE_SHORT]?.AsDouble ?? default);
            set => this[iRATE_SHORT] = CE.FieldValue.Create(iRATE_SHORT + 1, value);
        }

        public decimal RateLong
        {
            get => (decimal)(this[iRATE_LONG]?.AsDouble ?? default);
            set => this[iRATE_LONG] = CE.FieldValue.Create(iRATE_LONG + 1, value);
        }

        public decimal RateShortStandart
        {
            get => (decimal)(this[iRATE_SHORT_STANDART]?.AsDouble ?? default);
            set => this[iRATE_SHORT_STANDART] = CE.FieldValue.Create(iRATE_SHORT_STANDART + 1, value);
        }

        public decimal RateLongStandart
        {
            get => (decimal)(this[iRATE_LONG_STANDART]?.AsDouble ?? default);
            set => this[iRATE_LONG_STANDART] = CE.FieldValue.Create(iRATE_LONG_STANDART + 1, value);
        }

        public bool IsLong
        {
            get => this[iIS_LONG]?.AsBoolean ?? false;
            set => this[iIS_LONG] = CE.FieldValue.Create(iIS_LONG + 1, value);
        }

        public bool IsShort
        {
            get => this[iIS_SHORT]?.AsBoolean ?? false;
            set => this[iIS_SHORT] = CE.FieldValue.Create(iIS_SHORT + 1, value);
        }

        public bool IsMarginal
        {
            get => this[iIS_MARGINAL]?.AsBoolean ?? false;
            set => this[iIS_MARGINAL] = CE.FieldValue.Create(iIS_MARGINAL + 1, value);
        }

        public bool IsRestricted
        {
            get => this[iIS_RESTRICTED]?.AsBoolean ?? false;
            set => this[iIS_RESTRICTED] = CE.FieldValue.Create(iIS_RESTRICTED + 1, value);
        }
    } 
}