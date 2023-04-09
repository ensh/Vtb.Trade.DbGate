using System;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;


namespace Vtb.Trade.DbGate.Client
{
    using IS = IS<MarginInstrumentsCtx>;

    public class MarginInstrumentsCtx : IdentitySelectorContext { }
    public class MarginInstrumentsExternalAdapter : CE_Adapter
    {
        public MarginInstrumentsExternalAdapter() : base(EntityType.MarginInstruments) { ((CE)this).Fields.Capacity = 15; }
        public MarginInstrumentsExternalAdapter(CE data) : base(
            (data.EntityType == EntityType.MarginInstruments) ? data : new CE { EntityType = EntityType.MarginInstruments })
        { }

        public override void RegIdentity(Action<CE.FieldValue> registration)
        {
            registration(Ticker);
            registration(Isin);
            registration(QuoteExchangeSelector);
        }

        private const int iISIN = 0;
        private const int iSHORT_NAME = 1;
        private const int iTICKER = 2;
        private const int iLONG_LIMIT = 3;
        private const int iMARGIN_PRIORITY = 4;
        private const int iREPO_RATE = 5;
        private const int iSHORT_LIMIT = 6;
        private const int iDISCOUNT = 7;
        private const int iCLOSE_PRICE = 8;
        private const int iIS_MARGINAL = 9;
        private const int iIS_LONG = 10;
        private const int iIS_SHORT = 11;
        private const int iQUOTE_EXCHANGE_SELECTOR = 12;

        public IS Isin
        {
            get => this[iISIN];
            set => this[iISIN] = CE.FieldValue.Create(iISIN + 1, value.Text);
        }

        public string ShortName
        {
            get => this[iSHORT_NAME]?.AsString ?? default;
            set => this[iSHORT_NAME] = CE.FieldValue.Create(iSHORT_NAME + 1, value);
        }

        public IS Ticker
        {
            get => this[iTICKER];
            set => this[iTICKER] = CE.FieldValue.Create(iTICKER + 1, value.Text);
        }

        public decimal LongLimit
        {
            get => (decimal)(this[iLONG_LIMIT]?.AsDouble ?? default);
            set => this[iLONG_LIMIT] = CE.FieldValue.Create(iLONG_LIMIT + 1, value);
        }

        public int MarginPriority
        {
            get => this[iMARGIN_PRIORITY]?.AsInteger ?? default ;
            set => this[iMARGIN_PRIORITY] = CE.FieldValue.Create(iMARGIN_PRIORITY + 1, value);
        }

        public decimal RepoRate
        {
            get => (decimal)(this[iREPO_RATE]?.AsDouble ?? default);
            set => this[iREPO_RATE] = CE.FieldValue.Create(iREPO_RATE + 1, value);
        }

        public decimal ShortLimit
        {
            get => (decimal)(this[iSHORT_LIMIT]?.AsDouble ?? default);
            set => this[iSHORT_LIMIT] = CE.FieldValue.Create(iSHORT_LIMIT + 1, value);
        }

        public decimal Discount
        {
            get => (decimal)(this[iDISCOUNT]?.AsDouble ?? default);
            set => this[iDISCOUNT] = CE.FieldValue.Create(iDISCOUNT + 1, value);
        }

        public decimal ClosePrice
        {
            get => (decimal)(this[iCLOSE_PRICE]?.AsDouble ?? default);
            set => this[iCLOSE_PRICE] = CE.FieldValue.Create(iCLOSE_PRICE + 1, value);
        }

        public bool IsMarginal
        {
            get => this[iIS_MARGINAL]?.AsBoolean ?? default;
            set => this[iIS_MARGINAL] = CE.FieldValue.Create(iIS_MARGINAL + 1, value);
        }

        public bool IsLong
        {
            get => this[iIS_LONG]?.AsBoolean ?? default;
            set => this[iIS_LONG] = CE.FieldValue.Create(iIS_LONG + 1, value);
        }

        public bool IsShort
        {
            get => this[iIS_SHORT]?.AsBoolean ?? default;
            set => this[iIS_SHORT] = CE.FieldValue.Create(iIS_SHORT + 1, value);
        }

        public IS QuoteExchangeSelector
        {
            get => this[iQUOTE_EXCHANGE_SELECTOR]?.AsString ?? default;
            set => this[iQUOTE_EXCHANGE_SELECTOR] = CE.FieldValue.Create(iQUOTE_EXCHANGE_SELECTOR + 1, value.Text);
        }
    }
}

