using System;
using System.Collections.Generic;
using System.Data;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class MarginInstrumentsExternalRequestAdapter : RequestAdapter
    {
        public MarginInstrumentsExternalRequestAdapter() : base(EntityType.MarginInstruments) { ((CE)this).Fields.Capacity = 5; }
        public MarginInstrumentsExternalRequestAdapter(CE data) : base(
            (data.EntityType == EntityType.MarginInstruments) ? data : new CE { EntityType = EntityType.MarginInstruments })
        { }

        public static implicit operator MarginInstrumentsExternalRequestAdapter(CE entity) => new MarginInstrumentsExternalRequestAdapter(entity);

        private int _commandTimeout = 0;
        private int _columnsIdx = 1;
        private int _dateIdx = 2;

        public override CommandType CommandType { get => CommandType.StoredProcedure; }
        public override string CommandText { get => @"olb.vtb_get_margin_instruments"; }
        public override int CommandTimeout
        {
            get => this[_commandTimeout]?.AsInteger ?? 3600;
            set => this[_commandTimeout] = CE.FieldValue.Create(1, value);
        }

        public string Columns
        {
            get => this[_columnsIdx]?.AsString ?? default;
            set => this[_columnsIdx] = CE.FieldValue.Create(2, value);
        }

        /// <summary>
        /// Дата, на которую извлекается список
        /// </summary>
        public DateTime Date
        {
            get => this[_dateIdx]?.AsDateTime ?? DateTime.Now.Date;
            set => this[_dateIdx] = CE.FieldValue.Create(3, value);
        }

        public override IEnumerable<(string name, object value)> Parameters
        {
            get
            {
                if (CheckIndex(_dateIdx))
                {
                    yield return ("@Date", Date);
                }
            }
        }

        public override IEnumerable<string> Fields
        {
            get
            {
                if (CheckIndex(_columnsIdx))
                {
                    var fields = (Columns ?? "").Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var f in fields)
                    {
                        yield return f;
                    }
                }
            }
        }
    }
}