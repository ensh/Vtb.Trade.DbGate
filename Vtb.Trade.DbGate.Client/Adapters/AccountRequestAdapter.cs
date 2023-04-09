using System;
using System.Collections.Generic;
using System.Data;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AccountRequestAdapter : RequestAdapter
    {
        public AccountRequestAdapter() : base(EntityType.Account) { ((CE)this).Fields.Capacity = 7; }
        public AccountRequestAdapter(CE data) : base(
            (data.EntityType == EntityType.Account) ? data : new CE { EntityType = EntityType.Account })
        { }

        public const int iCommandTimeout = 0;
        public const int iAgreementNumber = 1;
        public const int iExchange = 2;
        public const int iCount = 3;
        public const int iColumns = 4;
        public const int iWithIdentity = 5;

        public static implicit operator AccountRequestAdapter(CE entity) => new AccountRequestAdapter(entity);

        public override CommandType CommandType { get => CommandType.StoredProcedure; }
        public override string CommandText { get => @"obr.GetTradingAccounts"; }
        public override int CommandTimeout
        {
            get => this[iCommandTimeout]?.AsInteger ?? 3600;
            set => this[iCommandTimeout] = (iCommandTimeout + 1, value);
        }

        public string AgreementNumber
        {
            get => this[iAgreementNumber]?.AsString ?? default;
            set => this[iAgreementNumber] = (iAgreementNumber + 1, value);
        }

        public string Exchange
        {
            get => this[iExchange]?.AsString ?? default;
            set => this[iExchange] = (iExchange + 1, value);
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
                if (CheckIndex(iAgreementNumber))
                    yield return ("@AgreementNumber", AgreementNumber);

                if (CheckIndex(iExchange))
                    yield return ("@Exchange", Exchange);

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
