using Microsoft.SqlServer.Server;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Common
{
    public static class CE_FieldsExtensions
    {
        public static readonly SqlMetaData[] MetaDataFields;

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

        static CE_FieldsExtensions()
        {
            MetaDataFields = new SqlMetaData[12];

            MetaDataFields[iEntityId] = new SqlMetaData("EntityId", SqlDbType.Int);
            MetaDataFields[iEntityType] = new SqlMetaData("EntityType", SqlDbType.Int);
            MetaDataFields[iFieldNumber] = new SqlMetaData("FieldNumber", SqlDbType.TinyInt);
            MetaDataFields[iFieldType] = new SqlMetaData("FieldType", SqlDbType.TinyInt);
            MetaDataFields[iAsBoolean] = new SqlMetaData("AsBoolean", SqlDbType.Bit);
            MetaDataFields[iAsChar] = new SqlMetaData("AsChar", SqlDbType.Char, 1);
            MetaDataFields[iAsDateTime] = new SqlMetaData("AsDateTime", SqlDbType.DateTime);
            MetaDataFields[iAsDouble] = new SqlMetaData("AsDouble", SqlDbType.Float);
            MetaDataFields[iAsEntity] = new SqlMetaData("AsEntity", SqlDbType.Int);
            MetaDataFields[iAsInt] = new SqlMetaData("AsInt", SqlDbType.Int);
            MetaDataFields[iAsLong] = new SqlMetaData("AsLong", SqlDbType.BigInt);
            MetaDataFields[iAsString] = new SqlMetaData("AsString", SqlDbType.VarChar, 8000);
        }

        public static SqlParameter SecuritySnapshotFieldsParameter(this IEnumerable<CE> entities)
        {
            return new SqlParameter()
            {
                SqlDbType = SqlDbType.Structured,
                Value = entities?.SecuritySnapshotFields()
            };
        }
        public static IEnumerable<SqlDataRecord> SecuritySnapshotFields(this IEnumerable<CE> entities)
        {
            var i = 1;
            foreach (var entity in entities)
            {
                var e = (i++, entity.EntityType);
                // Первое поле служебное (флаги). Пропускаем его.
                for (int j = 1; j < entity.Fields.Count; j++)
                {
                    var field = entity.Fields[j];
                    var record = field.CreateRecord(e);
                    yield return record;
                }
            }
        }
        private static SqlDataRecord CreateRecord(this CE.FieldValue field, (int id, int @type) entity)
        {
            var rec = new SqlDataRecord(MetaDataFields);

            rec.SetInt32(iEntityId, entity.id);
            rec.SetInt32(iEntityType, entity.@type);           
            rec.SetByte(iFieldNumber, (byte)field.Number);
            rec.SetByte(iFieldType, (byte)field.ValueType);

            switch ((int)field.ValueType)
            {
                case (int)CE.ValueType.AsDouble:
                    rec.SetDouble(iAsDouble, field.AsDouble);

                    break;
                case (int)CE.ValueType.AsInteger:
                    rec.SetInt32(iAsInt, field.AsInteger);

                    break;
                case (int)CE.ValueType.AsLong:
                    rec.SetInt64(iAsLong, field.AsLong);

                    break;
                case (int)CE.ValueType.AsBoolean:
                    rec.SetBoolean(iAsBoolean, field.AsBoolean);

                    break;
                case (int)CE.ValueType.AsString:
                    rec.SetString(iAsString, field.AsString);

                    break;
                case (int)CE.ValueType.AsChar:
                    rec.SetString(iAsChar, "" +field.AsChar);

                    break;
                case (int)CE.ValueType.AsDateTime:
                    rec.SetDateTime(iAsDateTime, field.AsDateTime);

                    break;
                default:
                    break;
            }

            return rec;
        }

    }
}
