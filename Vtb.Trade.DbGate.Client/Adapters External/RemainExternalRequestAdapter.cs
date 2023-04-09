using System;
using System.Collections.Generic;
using System.Data;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class RemainExternalRequestAdapter : RequestAdapter
    {
        public RemainExternalRequestAdapter() : base(EntityType.Remain) { ((CE)this).Fields.Capacity = 5; }
        public RemainExternalRequestAdapter(CE data) : base(
            (data.EntityType == EntityType.Remain) ? data : new CE { EntityType = EntityType.Remain })
        {
        }

        private int _commandTimeout = 0;
        private int _columnsIdx = 1;
        private int _datesIdx = 2;
        private int _placeIdIdx = 3;

        public static implicit operator RemainExternalRequestAdapter(CE entity) => new RemainExternalRequestAdapter(entity);

        public override CommandType CommandType { get => CommandType.StoredProcedure; }
        public override string CommandText { get => @"olb.vtb_get_remain_range_tradecore"; }
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
        /// Строка с перечнем дат в формате 'YYYY-MM-DD' с разделителем "запятая"
        /// </summary>
        public string Dates
        {
            get => this[_datesIdx]?.AsString ?? default;
            set => this[_datesIdx] = CE.FieldValue.Create(3, value);
        }

        /// <summary>
        /// Id площадки (2 по-умолчанию)
        /// </summary>
        public int PlaceId
        {
            get => this[_placeIdIdx]?.AsInteger ?? 2;
            set => this[_placeIdIdx] = CE.FieldValue.Create(4, value);
        }


        public override IEnumerable<(string name, object value)> Parameters
        {
            get
            {
                if (CheckIndex(_datesIdx))
                {
                    yield return ("@Dates", Dates);
                }
                if (CheckIndex(_placeIdIdx))
                {
                    yield return ("@PlaceId", PlaceId);
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
