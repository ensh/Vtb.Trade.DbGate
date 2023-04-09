namespace Vtb.Trade.DbGate.Client
{
    using Vtb.Trade.DbGate.Common;
    using Vtb.Trade.Grpc.Common;
    public class FieldInfoAdapter : CE_Adapter
    {
        public FieldInfoAdapter() : base(EntityType.FieldInfo) { ((CE)this).Fields.Capacity = 4; }
        public FieldInfoAdapter(CE data) : base(
            (data.EntityType == EntityType.FieldInfo) ? data : new CE { EntityType = EntityType.FieldInfo } )
        { }

        public static implicit operator FieldInfoAdapter(CE entity) => new (entity);

        public const int iName = 0;
        public const int iTable = 1;
        public const int iOrdinal = 2; 

        public int Ordinal
        {
            get => this[iOrdinal]?.AsInteger ?? default;
            set => this[iOrdinal] = CE.FieldValue.Create(iOrdinal + 1, value);
        }

        public string Name
        {
            get => this[iName]?.AsString ?? default;
            set => this[iName] = CE.FieldValue.Create(iName + 1, value);
        }
        public string Table
        {
            get => this[iTable]?.AsString ?? default;
            set => this[iTable] = CE.FieldValue.Create(iName + 1, value);
        }
    }
}
