using System;

using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class SecuritySnapshotAdapter : CE_Adapter
    {
        public SecuritySnapshotAdapter() : base(Common.EntityType.SecuritySnapshot)
        { }

        public SecuritySnapshotAdapter(CE data) : base(
            (data.EntityType == Common.EntityType.SecuritySnapshot) ? data : new CE { EntityType = Common.EntityType.SecuritySnapshot })
        { }

        public static implicit operator SecuritySnapshotAdapter(CE value) => new SecuritySnapshotAdapter(value);

        public const int iEntityId = 0;
        public const int iEntityType = 1;
        public const int iFieldNumber = 2;
        public const int iFieldType = 3;
        public const int iAsBoolean = 4;
        public const int iAsChar = 5;
        public const int iAsDateTime = 6;
        public const int iAsDouble = 7;
        public const int iAsEntity = 8;
        public const int iAsInt = 9;
        public const int iAsLong = 10;
        public const int iAsString = 11;

        public int SnapshotId { get; set; }

        public int EntityId
        {
            get => this[iEntityId]?.AsInteger ?? default;
            set => this[iEntityId] = (iEntityId + 1, value);
        }

        public int EntityType
        {
            get => this[iEntityType]?.AsInteger ?? default;
            set => this[iEntityType] = (iEntityType + 1, value);
        }

        public int FieldNumber
        {
            get => this[iFieldNumber]?.AsInteger ?? default;
            set => this[iFieldNumber] = (iFieldNumber + 1, value);
        }

        public int FieldType
        {
            get => this[iFieldType]?.AsInteger ?? default;
            set => this[iFieldType] = (iFieldType + 1, value);
        }

        public bool AsBoolean
        {
            get => this[iAsBoolean]?.AsBoolean ?? default;
            set => this[iAsBoolean] = (iAsBoolean + 1, value);
        }

        public char AsChar
        {
            get => this[iAsChar]?.AsChar ?? default;
            set => this[iAsChar] = (iAsChar + 1, value);
        }

        public DateTime AsDateTime
        {
            get => this[iAsDateTime]?.AsDateTime ?? default;
            set => this[iAsDateTime] = (iAsDateTime + 1, value);
        }

        public double AsDouble
        {
            get => this[iAsDouble]?.AsDouble ?? default;
            set => this[iAsDouble] = (iAsDouble + 1, value);
        }

        public int AsEntity
        {
            get => this[iAsEntity]?.AsInteger ?? default;
            set => this[iAsEntity] = (iAsEntity + 1, value);
        }

        public int AsInt
        {
            get => this[iAsInt]?.AsInteger ?? default;
            set => this[iAsInt] = (iAsInt + 1, value);
        }

        public long AsLong
        {
            get => this[iAsLong]?.AsLong ?? default;
            set => this[iAsLong] = (iAsLong + 1, value);
        }

        public string AsString
        {
            get => this[iAsString]?.AsString ?? default;
            set => this[iAsString] = (iAsString + 1, value);
        }
    }
}
