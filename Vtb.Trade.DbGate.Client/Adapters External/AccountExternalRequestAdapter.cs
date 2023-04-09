using System;
using System.Collections.Generic;
using System.Data;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AccountExternalRequestAdapter : RequestAdapter
    {
        public AccountExternalRequestAdapter() : base(EntityType.Account) { ((CE)this).Fields.Capacity = 7; }
        public AccountExternalRequestAdapter(CE data) : base(
            (data.EntityType == EntityType.AccountExternalRequest) ? data : new CE { EntityType = EntityType.AccountExternalRequest })
        { }

        public const int iCommandTimeout = 0;
        public const int iColumns = 1;
        public const int iWithIdentity = 2;

        public static implicit operator AccountExternalRequestAdapter(CE entity) => new AccountExternalRequestAdapter(entity);

        public override CommandType CommandType { get => CommandType.StoredProcedure; }
        public override string CommandText { get => @"obr.GetAccounts"; }
        public override int CommandTimeout
        {
            get => this[iCommandTimeout]?.AsInteger ?? 3600;
            set => this[iCommandTimeout] = (iCommandTimeout + 1, value);
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
                yield break;
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
