using System;
using System.Collections.Generic;
using System.Data;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AccountChangeExternalRequestAdapter : RequestAdapter
    {
        public AccountChangeExternalRequestAdapter() : base(EntityType.AccountChange) { ((CE)this).Fields.Capacity = 5; }
        public AccountChangeExternalRequestAdapter(CE data) : base(
            (data.EntityType == EntityType.AccountChange) ? data : new CE { EntityType = EntityType.AccountChange })
        { }

        public const int iCommandTimeout = 0;
        public const int iLastUpdateId = 1;
        public const int iCount = 2;
        public const int iColumns = 3;

        public static implicit operator AccountChangeExternalRequestAdapter(CE entity) => new AccountChangeExternalRequestAdapter(entity);

        public override CommandType CommandType { get => CommandType.StoredProcedure; }
        public override string CommandText { get => @"obr.GetAccountChanges"; }
        public override int CommandTimeout
        {
            get => this[iCommandTimeout]?.AsInteger ?? 3600;
            set => this[iCommandTimeout] = (iCommandTimeout + 1, value);
        }

        public int LastUpdateId
        {
            get => this[iLastUpdateId]?.AsInteger ?? default;
            set => this[iLastUpdateId] = (iLastUpdateId + 1, value);
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

        public override IEnumerable<(string name, object value)> Parameters
        {
            get
            {
                if (CheckIndex(iLastUpdateId))
                    yield return ("@LastUpdateId", LastUpdateId);

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
