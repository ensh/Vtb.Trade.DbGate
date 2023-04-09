using System;
using System.Collections.Generic;
using System.Data;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class SetProcessedExternalRequestAdapter : RequestAdapter
    {
        public SetProcessedExternalRequestAdapter() : base(EntityType.SetProcessed) { ((CE)this).Fields.Capacity = 2; }
        public SetProcessedExternalRequestAdapter(CE data) : base(
            (data.EntityType == EntityType.SetProcessed) ? data : new CE { EntityType = EntityType.SetProcessed })
        { }

        public const int iCommandTimeout = 0;

        public static implicit operator SetProcessedExternalRequestAdapter(CE entity) => new SetProcessedExternalRequestAdapter(entity);

        public override CommandType CommandType { get => CommandType.StoredProcedure; }
        public override string CommandText { get => @"obr.SetAccountChangesProcessed"; }
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
