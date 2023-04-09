namespace Vtb.Trade.DbGate.Client
{
    using System;
    using System.Collections.Generic;
    using Vtb.Trade.DbGate.Common;
    using Vtb.Trade.Grpc.Common;

    public class FieldValueAdapter : CE_Adapter
    {
        public FieldValueAdapter() : base(EntityType.FieldValue) { ((CE)this).Fields.Capacity = 4; }
        public FieldValueAdapter(CE data) : base((data.EntityType == EntityType.FieldValue) ? data : new CE { EntityType = EntityType.FieldValue }) { }

        public static implicit operator FieldValueAdapter(CE entity) => new (entity);

        public const int iOrdinal = 0;
        public const int iValue = 1;

        public int Ordinal
        {
            get => this[iOrdinal]?.AsInteger ?? default;
            set => this[iOrdinal] = CE.FieldValue.Create(iOrdinal + 1, value);
        }

        public CE.FieldValue Value
        {
            get => this[iValue];
            set
            {
                value.Number = iValue + 1;
                this[iValue] = value; 
            }
        }
    }
}
