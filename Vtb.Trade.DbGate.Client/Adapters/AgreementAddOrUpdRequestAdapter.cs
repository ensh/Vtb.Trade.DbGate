using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AgreementAddOrUpdRequestAdapter : RequestAdapter
    {
        public static readonly SqlMetaData[] MetaDataFields;

        public const int ifAgreementNumber = 0;
        public const int ifClientCode = 1;
        public const int ifParentAgreementNumber = 2;
        public const int ifClientTypeCode = 3;
        public const int ifGkk = 4;
        public const int ifPackageName = 5;
        public const int ifFullName = 6;
        public const int ifSourcePlatformCode = 7;
        public const int ifResidentTypeCode = 8;
        public const int ifRiskCategory = 9;
        public const int ifGroupsFlags = 10;

        public const int iCommandTimeout = 0;

        static AgreementAddOrUpdRequestAdapter()
        {
            MetaDataFields = new SqlMetaData[11];

            MetaDataFields[ifAgreementNumber] = new SqlMetaData("AgreementNumber", SqlDbType.VarChar, 8000);
            MetaDataFields[ifClientCode] = new SqlMetaData("ClientCode", SqlDbType.VarChar, 8000);
            MetaDataFields[ifParentAgreementNumber] = new SqlMetaData("ParentAgreementNumber", SqlDbType.VarChar, 8000);
            MetaDataFields[ifClientTypeCode] = new SqlMetaData("ClientTypeCode", SqlDbType.VarChar, 8000);
            MetaDataFields[ifGkk] = new SqlMetaData("Gkk", SqlDbType.Int);
            MetaDataFields[ifPackageName] = new SqlMetaData("PackageName", SqlDbType.VarChar, 8000);
            MetaDataFields[ifFullName] = new SqlMetaData("FullName", SqlDbType.VarChar, 8000);
            MetaDataFields[ifSourcePlatformCode] = new SqlMetaData("SourcePlatformCode", SqlDbType.VarChar, 8000);
            MetaDataFields[ifResidentTypeCode] = new SqlMetaData("ResidentTypeCode", SqlDbType.VarChar, 8000);
            MetaDataFields[ifRiskCategory] = new SqlMetaData("RiskCategory", SqlDbType.VarChar, 8000);
            MetaDataFields[ifGroupsFlags] = new SqlMetaData("GroupsFlags", SqlDbType.Int);
        }

        public AgreementAddOrUpdRequestAdapter()
            : base(EntityType.AgreementAddOrUpdRequest)
        { }

        public AgreementAddOrUpdRequestAdapter(CE data)
            : base((data.EntityType == EntityType.AgreementAddOrUpdRequest) ? data : new CE { EntityType = EntityType.AgreementAddOrUpdRequest })
        { }

        public static implicit operator AgreementAddOrUpdRequestAdapter(CE entity)
            => new AgreementAddOrUpdRequestAdapter(entity);

        public override CommandType CommandType => CommandType.StoredProcedure;

        public override string CommandText => "obr.AddOrUpdAgreements";

        public override int CommandTimeout
        {
            get => this[iCommandTimeout]?.AsInteger ?? 3600;
            set => this[iCommandTimeout] = (iCommandTimeout + 1, value);
        }

        public override IEnumerable<(string name, object value)> Parameters => GetParameters();

        public override IEnumerable<string> Fields => Enumerable.Empty<string>();

        public IEnumerable<CE> Agreements { get; set; }

        private IEnumerable<(string name, object value)> GetParameters()
        {
            yield return ("@Agreements", GetAgreementsParameter(Agreements));
        }

        private static SqlParameter GetAgreementsParameter(IEnumerable<CE> agreements)
        {
            SqlParameter prm = new SqlParameter()
            {
                SqlDbType = SqlDbType.Structured,
                Value = GatAgreementsParameterValue(agreements)
            };

            return prm;
        }

        private static IEnumerable<SqlDataRecord> GatAgreementsParameterValue(IEnumerable<CE> agreements)
        {
            foreach (CE ce in agreements)
            {
                SqlDataRecord record = CreateRecord(ce);

                yield return record;
            }
        }

        private static SqlDataRecord CreateRecord(AgreementAdapter agreement)
        {
            SqlDataRecord rec = new SqlDataRecord(MetaDataFields);

            rec.SetString(ifAgreementNumber, agreement.AgreementNumber);
            rec.SetString(ifClientCode, agreement.ClientCode);
            rec.SetString(ifParentAgreementNumber, agreement.ParentAgreementNumber);
            rec.SetString(ifClientTypeCode, agreement.ClientTypeCode);
            rec.SetInt32(ifGkk, agreement.Gkk);

            if (agreement.CheckIndex(AgreementAdapter.iPackageName))
            {
                rec.SetString(ifPackageName, agreement.PackageName);
            }
            else
            {
                rec.SetDBNull(ifPackageName);
            }

            rec.SetString(ifFullName, agreement.FullName);
            rec.SetString(ifSourcePlatformCode, agreement.SourcePlatformCode);
            rec.SetString(ifResidentTypeCode, agreement.ResidentTypeCode);

            if (agreement.CheckIndex(AgreementAdapter.iRiskCategory))
            {
                rec.SetString(ifRiskCategory, agreement.RiskCategory);
            }
            else
            {
                rec.SetDBNull(ifRiskCategory);
            }

            rec.SetInt32(ifGroupsFlags, agreement.GroupsFlags);

            return rec;
        }
    }
}
