using System;
using System.Collections.Generic;
using System.Data;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class LastUpdateIdExternalRequestAdapter : RequestAdapter
    {
        public LastUpdateIdExternalRequestAdapter() : base(EntityType.LastUpdateId) { ((CE)this).Fields.Capacity = 2; }
        public LastUpdateIdExternalRequestAdapter(CE data) : base(
            (data.EntityType == EntityType.LastUpdateId) ? data : new CE { EntityType = EntityType.LastUpdateId })
        { }

        public const int iCommandTimeout = 0;

        public static implicit operator LastUpdateIdExternalRequestAdapter(CE entity) => new LastUpdateIdExternalRequestAdapter(entity);

        public override CommandType CommandType { get => CommandType.StoredProcedure; }
        public override string CommandText { get => @"obr.GetLastUpdateId"; }
        public override int CommandTimeout
        {
            get => this[iCommandTimeout]?.AsInteger ?? 3600;
            set => this[iCommandTimeout] = (iCommandTimeout + 1, value);
        }

        public override IEnumerable<(string name, object value)> Parameters
        {
            get
            {
                yield break;
            }
        }

        public override IEnumerable<string> Fields
        {
            get
            {
                yield break;
            }
        }
    }
}
