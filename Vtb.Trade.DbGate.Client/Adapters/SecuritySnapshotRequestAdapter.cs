using System.Collections.Generic;
using System.Data;
using System.Linq;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{

    public enum ResultObjectEnum { Adapter = 0, Entity = 1 };

    public class SecuritySnapshotRequestAdapter : RequestAdapter
    {        
        public SecuritySnapshotRequestAdapter() : base(EntityType.SecuritySnapshot)
        { }

        public SecuritySnapshotRequestAdapter(CE data) : base(
            (data.EntityType == EntityType.SecuritySnapshot) ? data : new CE { EntityType = EntityType.SecuritySnapshot })
        { }

        public const int iCommandTimeout = 0;
        public const int iSnapshotId = 1;
        public const int iSource = 2;
        public const int iResultObject = 3;

        public override CommandType CommandType => CommandType.StoredProcedure;

        public string CommandTextSelector { get; set; }

        public override string CommandText => "gate.GetSecuritySnapshot";

        public override int CommandTimeout
        {
            get => this[iCommandTimeout]?.AsInteger ?? 3600;
            set => this[iCommandTimeout] = (iCommandTimeout + 1, value);
        }

        public override IEnumerable<(string name, object value)> Parameters => GetParameters();

        public override IEnumerable<string> Fields => Enumerable.Empty<string>();

        public int SnapshotId
        {
            get => this[iSnapshotId]?.AsInteger ?? default;
            set => this[iSnapshotId] = (iSnapshotId + 1, value);
        }

        public string Source
        {
            get => this[iSource]?.AsString ?? default;
            set => this[iSource] = (iSource + 1, value);
        }

        public ResultObjectEnum ResultsObject
        {
            get => (ResultObjectEnum)(this[iResultObject]?.AsInteger ?? default);
            set => this[iResultObject] = (iResultObject + 1, (int)value);
        }

        private IEnumerable<(string name, object value)> GetParameters()
        {
            if (CheckIndex(iSnapshotId))
            {
                yield return ("@SnapshotId", SnapshotId);
            }

            if (CheckIndex(iSource))
            {
                yield return ("@Source", Source);
            }
        }

        public static implicit operator SecuritySnapshotRequestAdapter(CE entity) 
            => new SecuritySnapshotRequestAdapter(entity);
    }
}
