using System;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class LastUpdateIdExternalAdapter : CE_Adapter
    {
        public LastUpdateIdExternalAdapter() : base(EntityType.LastUpdateId) { ((CE)this).Fields.Capacity = 2; } 
        public LastUpdateIdExternalAdapter(CE data) : base(
            (data.EntityType == EntityType.LastUpdateId) ? data : new CE { EntityType = EntityType.LastUpdateId })
        { }

        public static implicit operator LastUpdateIdExternalAdapter(CE entity) => new LastUpdateIdExternalAdapter(entity);

        public const int iLastUpdateId = 0;

        /// <summary>
        /// Id последнего обновления
        /// </summary>
        public int LastUpdateId
        {
            get => this[iLastUpdateId]?.AsInteger ?? default;
            set => this[iLastUpdateId] = (iLastUpdateId + 1, value);
        }
    }
}