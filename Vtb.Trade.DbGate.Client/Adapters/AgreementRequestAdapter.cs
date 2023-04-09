using System;
using System.Collections.Generic;
using System.Data;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AgreementRequestAdapter : RequestAdapter
    {
        public AgreementRequestAdapter() : base(EntityType.Agreement) { ((CE)this).Fields.Capacity = 5; }
        public AgreementRequestAdapter(CE data) : base(
            (data.EntityType == EntityType.Agreement) ? data : new CE { EntityType = EntityType.Agreement })
        { }

        public const int iCommandTimeout = 0;
        public const int iCount = 1;
        public const int iColumns = 2;
        public const int iWithIdentity = 3;

        public static implicit operator AgreementRequestAdapter(CE entity) => new AgreementRequestAdapter(entity);

        public override CommandType CommandType { get => CommandType.StoredProcedure; }
        public override string CommandText { get => @"obr.GetAgreements"; }
        public override int CommandTimeout
        {
            get => this[iCommandTimeout]?.AsInteger ?? 3600;
            set => this[iCommandTimeout] = (iCommandTimeout + 1, value);
        }

        public int Count
        {
            get => this[iCount]?.AsInteger ?? default;
            set => this[iCount] = (iCount + 1, value);
        }

        public string Columns
        {
            get => this[iColumns]?.AsString ?? default;
            set => this[iColumns] = (iColumns + 1, value);
        }

        public override bool WithIdentity
        {
            get => this[iWithIdentity]?.AsBoolean ?? true;
            set => this[iWithIdentity] = (iWithIdentity + 1, value);
        }

        public override IEnumerable<(string name, object value)> Parameters
        {
            get
            {
                if (CheckIndex(iCount))
                    yield return ("@Count", Count);
            }
        }

        public override IEnumerable<string> Fields
        {
            get
            {
                if (CheckIndex(iColumns))
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
