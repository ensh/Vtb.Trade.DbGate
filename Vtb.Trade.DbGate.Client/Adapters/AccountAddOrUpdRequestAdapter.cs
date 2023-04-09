using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AccountAddOrUpdRequestAdapter : RequestAdapter
    {
        public static readonly SqlMetaData[] MetaDataFields;

        public const int ifFirmTradingAccountCode = 0;
        public const int ifFortsTradingAccount = 1;
        public const int ifExchangeCode = 2;
        public const int ifAgreementNumber = 3;
        public const int ifTradingAccountStatusCode = 4;
        public const int ifGroupsFlags = 5;

        public const int iCommandTimeout = 0;

        static AccountAddOrUpdRequestAdapter()
        {
            MetaDataFields = new SqlMetaData[6];

            MetaDataFields[ifFirmTradingAccountCode] = new SqlMetaData("FirmTradingAccountCode", SqlDbType.VarChar, 8000);
            MetaDataFields[ifFortsTradingAccount] = new SqlMetaData("FortsTradingAccount", SqlDbType.VarChar, 8000);
            MetaDataFields[ifExchangeCode] = new SqlMetaData("ExchangeCode", SqlDbType.VarChar, 8000);
            MetaDataFields[ifAgreementNumber] = new SqlMetaData("AgreementNumber", SqlDbType.VarChar, 8000);
            MetaDataFields[ifTradingAccountStatusCode] = new SqlMetaData("TradingAccountStatusCode", SqlDbType.VarChar, 8000);
            MetaDataFields[ifGroupsFlags] = new SqlMetaData("GroupsFlags", SqlDbType.Int);
        }

        public AccountAddOrUpdRequestAdapter()
            : base(EntityType.AccountAddOrUpdRequest)
        { }

        public AccountAddOrUpdRequestAdapter(CE data)
            : base((data.EntityType == EntityType.AccountAddOrUpdRequest) ? data : new CE { EntityType = EntityType.AccountAddOrUpdRequest })
        { }

        public static implicit operator AccountAddOrUpdRequestAdapter(CE entity)
            => new AccountAddOrUpdRequestAdapter(entity);

        public override CommandType CommandType => CommandType.StoredProcedure;

        public override string CommandText => "obr.AddOrUpdTradingAccounts";

        public override int CommandTimeout
        {
            get => this[iCommandTimeout]?.AsInteger ?? 3600;
            set => this[iCommandTimeout] = (iCommandTimeout + 1, value);
        }

        public override IEnumerable<(string name, object value)> Parameters => GetParameters();

        public override IEnumerable<string> Fields => Enumerable.Empty<string>();

        public IEnumerable<CE> Accounts { get; set; }

        private IEnumerable<(string name, object value)> GetParameters()
        {
            yield return ("@Accounts", GetAccountsParameter(Accounts));
        }

        private static SqlParameter GetAccountsParameter(IEnumerable<CE> accounts)
        {
            SqlParameter prm = new SqlParameter()
            {
                SqlDbType = SqlDbType.Structured,
                Value = GatAccountsParameterValue(accounts)
            };

            return prm;
        }

        private static IEnumerable<SqlDataRecord> GatAccountsParameterValue(IEnumerable<CE> accounts)
        {
            foreach (CE ce in accounts)
            {
                SqlDataRecord record = CreateRecord(ce);

                yield return record;
            }
        }

        private static SqlDataRecord CreateRecord(AccountAdapter account)
        {
            SqlDataRecord rec = new SqlDataRecord(MetaDataFields);

            rec.SetString(ifFirmTradingAccountCode, account.FirmTradingAccountCode);

            if (account.CheckIndex(AccountAdapter.iFortsTradingAccount))
            {
                rec.SetString(ifFortsTradingAccount, account.FortsTradingAccount);
            }
            else
            {
                rec.SetDBNull(ifFortsTradingAccount);
            }

            rec.SetString(ifExchangeCode, account.ExchangeCode);
            rec.SetString(ifAgreementNumber, account.AgreementNumber);
            rec.SetString(ifTradingAccountStatusCode, account.TradingAccountStatusCode);
            rec.SetInt32(ifGroupsFlags, account.GroupsFlags);

            return rec;
        }
    }
}
