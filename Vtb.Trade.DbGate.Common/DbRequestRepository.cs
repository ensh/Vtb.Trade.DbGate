using Microsoft.SqlServer.Server;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Common
{
    public class DbRequestRepository
    {
        private readonly string ConnectionString;
        private readonly Action<CE.FieldValue> IdentityRegistration;
        private readonly ILogger<DbRequestRepository> Logger;

        public DbRequestRepository(RepositoryOptions repositoryOptions, Action<CE.FieldValue> identityAdd,
            ILogger<DbRequestRepository> logger)
        {
            ConnectionString = repositoryOptions.ConnectionString;
            IdentityRegistration = identityAdd;
            Logger = logger;
        }

        public IEnumerable<CE> Get(RequestOptions requestOptions)
            => Get(requestOptions, Enumerable.Empty<(string, object)>());

        public IEnumerable<CE> Get(RequestOptions requestOptions,
            IEnumerable<(string name, object value)> parameters)
        {
            foreach (var result in InternalGet(requestOptions, parameters))
                yield return result;
        }

        public IEnumerable<CE> Get(RequestOptions requestOptions, IEnumerable<CE> tableRows)
            => Get(requestOptions, Enumerable.Empty<(string name, object value)>(), tableRows);

        public IEnumerable<CE> Get(RequestOptions requestOptions,
            IEnumerable<(string name, object value)> parameters, IEnumerable<CE> tableRows)
        {
            var tableParameter = (requestOptions.MappingName, tableRows.AsTableParameter(requestOptions, Logger));

            foreach (var result in InternalGet(requestOptions, parameters.Append(tableParameter)))
            {
                yield return result;
            }
        }

        public IEnumerable<CE_Adapter> GetAdapters(RequestOptions requestOptions)
            => GetAdapters(requestOptions, Enumerable.Empty<(string, object)>());

        public IEnumerable<CE_Adapter> GetAdapters(RequestOptions requestOptions,
            IEnumerable<(string name, object value)> parameters)
        {
            foreach (var result in InternalGet(requestOptions, parameters))
                yield return result;
        }

        public IEnumerable<CE_Adapter> GetAdapters(RequestOptions requestOptions, IEnumerable<CE> tableRows)
            => GetAdapters(requestOptions, Enumerable.Empty<(string name, object value)>(), tableRows);

        public IEnumerable<CE_Adapter> GetAdapters(RequestOptions requestOptions,
            IEnumerable<(string name, object value)> parameters, IEnumerable<CE> tableRows)
        {
            var tableParameter = (requestOptions.MappingName, tableRows.AsTableParameter(requestOptions, Logger));

            foreach (var result in InternalGet(requestOptions, parameters.Append(tableParameter)))
            {
                yield return result;
            }
        }

        protected IEnumerable<CE_Adapter> InternalGet(RequestOptions requestOptions,
            IEnumerable<(string name, object value)> parameters)
        {
            const int iOrdinal = 0;
            const int iName = 1;
            const int iType = 2;
            const int iIdentity = 3;

            var fieldsInfo = requestOptions.Fields.Select(f => f.Split(',', StringSplitOptions.TrimEntries));

            var adapter = new CE_Adapter(requestOptions.EntityType);
            var fields = fieldsInfo.Select(f => f[iName]).ToHashSet();
            var identities = fieldsInfo.Where(f => f.Length > iIdentity && f[iIdentity] == "identity")
                .Select(f => int.Parse(f[iOrdinal])).ToArray();
            var copy = default(Action<SqlDataReader, CE_Adapter>);
            var sqlCommand = default(SqlCommand);

            using var sqlConnection = new SqlConnection(ConnectionString);

            sqlConnection.Open();
            using (sqlCommand = sqlConnection.CreateCommand())
            {
                var adapterColumns = fieldsInfo.Select(f 
                    => (f[iName], int.Parse(f[iOrdinal]), f[iType].GetFieldType()));

                using var sqlDataReader = sqlCommand.Apply(requestOptions, parameters
                    .Where(p => !(p.value is SqlParameter)))
                    .ExecuteReader(CommandBehavior.SchemaOnly);

                copy = SqlReaderMapper.CopyGetter(
                    sqlDataReader.GetColumnSchema()
                        .Where(c => fields.Contains(c.ColumnName)),
                    adapterColumns.ToArray());
            }

            using (sqlCommand = sqlConnection.CreateCommand())
            {
                using var sqlDataReader = sqlCommand.Apply(requestOptions, parameters)
                    .LogCommand(Logger)
                    .ExecuteReader();

                while (sqlDataReader.Read())
                {
                    copy(sqlDataReader, adapter);
                    if (requestOptions.WithIdentity && IdentityRegistration != null && identities.Length > 0)
                    {
                        for (int i = 0; i < identities.Length; i++)
                        {
                            var index = identities[i];
                            if (adapter.CheckIndex(index))
                                IdentityRegistration(adapter[index]);
                        }
                    }

                    yield return adapter;
                    adapter.Reset();
                }
            }
        }
    }

    public static class DbRequestRepositoryExtensions
    {
        public static SqlCommand Apply(this SqlCommand c, RequestOptions request,
            IEnumerable<(string name, object value)> parameters)
        {
            c.CommandType = Enum.Parse<CommandType>(request.CommandType);
            c.CommandText = request.CommandText;
            c.CommandTimeout = request.CommandTimeout;

            foreach (var parameter in parameters)
            {
                if (parameter.value is SqlParameter p)
                {
                    p.ParameterName = parameter.name;
                    c.Parameters.Add(p);
                }
                else
                {
                    c.Parameters.AddWithValue(parameter.name, parameter.value);
                }
            }
            return c;
        }

        public static SqlParameter AsTableParameter(this IEnumerable<CE> entities, RequestOptions requestOptions, ILogger logger = null)
            => new SqlParameter()
            {
                SqlDbType = SqlDbType.Structured,
                Value = entities?.Recordset(requestOptions.FieldMapping.GetMeta().ToArray(), logger),
            };

        public static SqlCommand LogCommand(this SqlCommand command, ILogger<DbRequestRepository> logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(command.AsText());
            }

            return command;
        }

        public static Type GetFieldType(this string text)
            => text switch
            {
                "bool" => typeof(bool),
                "char" => typeof(char),
                "DateTime" => typeof(DateTime),
                "double" => typeof(double),
                "int" => typeof(int),
                "long" => typeof(long),
                "string" => typeof(string),
                _ => Type.GetType(text) ??
                    throw new Exception("Тип поля не определен!")
            };

        public static SqlDbType GetParamType(this string text)
            => text switch
            {
                "bool" => SqlDbType.Bit,
                "byte" => SqlDbType.TinyInt,
                "short" => SqlDbType.SmallInt,
                "char" => SqlDbType.Char,
                "DateTime" => SqlDbType.DateTime,
                "Date" => SqlDbType.Date,
                "double" => SqlDbType.Float,
                "decimal" => SqlDbType.Decimal,
                "int" => SqlDbType.Int,
                "long" => SqlDbType.BigInt,
                "string" => SqlDbType.VarChar,
                _ => throw new Exception("Тип параметра не определен!")
            };

        public static IEnumerable<(int number, bool identity, SqlMetaData metaData)> GetMeta(this IEnumerable<string> parameters)
        {
            const int iNumber = 0;
            const int iName = 1;
            const int iType = 2;
            const int iLength = 3;
            const int iScale = 4;

            const int Param = 3;
            const int ParamWithLength = 4;
            const int ParamWithScale = 5;

            foreach (var parameter in parameters)
            {
                var @params = parameter.Split(',', StringSplitOptions.TrimEntries);

                var number = int.Parse(@params[iNumber]);
                switch (@params.Length)
                {
                    case Param:
                        if (@params[iType] == "char")
                            yield return (number, false, new SqlMetaData(@params[iName], GetParamType(@params[iType]), 1));
                        else
                        {
                            if (@params[iType] == "string")
                                yield return (number, false, new SqlMetaData(@params[iName], GetParamType(@params[iType]), 8000));
                            else
                                yield return (number, false, new SqlMetaData(@params[iName], GetParamType(@params[iType])));
                        }
                        break;
                    case ParamWithLength:
                        if (@params[iLength].All(c => c >= '0' && c <= '9'))
                        {
                            if (@params[iType] == "identity")
                            {
                                yield return (number, true, new SqlMetaData(@params[iName], SqlDbType.VarChar,
                                    int.Parse(@params[iLength])));
                            }
                            else
                            {
                                yield return (number, false, new SqlMetaData(@params[iName], GetParamType(@params[iType]),
                                    int.Parse(@params[iLength])));
                            }
                        }
                        else
                            throw new Exception($"Длинна параметра {@params[iName]} не задана !({@params[iLength]})!");
                        break;
                    case ParamWithScale:
                        if (@params[iLength].All(c => c >= '0' && c <= '9')) 
                        {
                            if (@params[iScale].All(c => c >= '0' && c <= '9'))
                            {
                                yield return (number, false, new SqlMetaData(@params[iName], GetParamType(@params[iType]),
                                    byte.Parse(@params[iLength]), byte.Parse(@params[iScale])));
                            }
                            else
                                throw new Exception($"Точность параметра {@params[iName]} не задана !({@params[iScale]})!");
                        }
                        else
                            throw new Exception($"Длинна параметра {@params[iName]} не задана !({@params[iLength]})!");
                        break;
                }
            }
        }

        private static IEnumerable<CE> PrepareIdentities(this IEnumerable<CE> entities,
            (int number, bool identity, SqlMetaData metaData)[] metaData)
        {
            var identities = metaData.Where(md => md.identity)
                .Select(md => md.number).ToHashSet<int>();

            foreach (var entity in entities)
            {
                for (int i = 0; i < entity.Fields.Count; i++)
                {
                    if (identities.Contains(entity.Fields[i].Number))
                    {
                        var identityField = entity.Fields[i];
                        identityField.AsString = identityField.GetIdentityText();
                    }
                }

                yield return entity;
            }
        }

        private static IEnumerable<SqlDataRecord> Recordset(this IEnumerable<CE> entities, 
            (int number, bool identity, SqlMetaData metaData)[] metaData, ILogger logger = null)
        {
            var createMeta = metaData.Select(md => md.metaData).ToArray();
            var metaInfo = metaData.Select((md, idx) => (md.number, idx))
                .ToDictionary(md => md.number, md => md.idx);

            var hasIdentities = metaData.Any(md => md.identity);

            if (hasIdentities)
                entities = entities.PrepareIdentities(metaData);

            foreach (var entity in entities)
            {
                var result = new SqlDataRecord(createMeta);
                var text = "";
                for (int i = 0; i < entity.Fields.Count; i++)
                {
                    var field = entity.Fields[i];
                    if (metaInfo.TryGetValue(field.Number, out var idx))
                    {
                        try
                        {
                            ApplyFieldValue((int)createMeta[idx].SqlDbType, result, field, idx);
                        }
                        catch (Exception ex)
                        {
                            if (logger != null)
                            {
                                logger.LogInformation(
                                    string.Join(Environment.NewLine,
                                new[]
                                {
                                    text,
                                    $"{createMeta[idx].Name}: {field.ValueType} => {createMeta[idx].SqlDbType} Value:{field}",
                                    ex.ToString()
                                }
                                ));
                            }
                            else
                                throw;
                        }
                    }
                } // for

                yield return result;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ApplyFieldValue(int dbType, SqlDataRecord result, CE.FieldValue field, int idx)
        {
            switch ((int)field.ValueType)
            {
                case (int)CE.ValueType.AsDouble:

                    switch (dbType)
                    {
                        case (int)SqlDbType.Float:
                            result.SetDouble(idx, field.AsDouble);
                            break;
                        case (int)SqlDbType.Decimal:
                            result.SetDecimal(idx, (decimal)field.AsDouble);
                            break;
                    }
                    break;
                case (int)CE.ValueType.AsInteger:
                    switch (dbType)
                    {
                        case (int)SqlDbType.Int:
                            result.SetInt32(idx, field.AsInteger);
                            break;
                        case (int)SqlDbType.TinyInt:
                            result.SetByte(idx, (byte)field.AsInteger);
                            break;
                        case (int)SqlDbType.SmallInt:
                            result.SetInt16(idx, (short)field.AsInteger);
                            break;
                    }
                    break;
                case (int)CE.ValueType.AsLong:
                    result.SetInt64(idx, field.AsLong);

                    break;
                case (int)CE.ValueType.AsBoolean:
                    result.SetBoolean(idx, field.AsBoolean);

                    break;
                case (int)CE.ValueType.AsString:
                    result.SetString(idx, field.AsString);

                    break;
                case (int)CE.ValueType.AsChar:
                    result.SetString(idx, "" + field.AsChar);

                    break;
                case (int)CE.ValueType.AsDateTime:
                    result.SetDateTime(idx, field.AsDateTime);

                    break;
                default:
                    break;
            }
        }

        public static string AsText(this SqlCommand command)
        {
            var text = new StringBuilder();
            switch (command.CommandType)
            {
                case CommandType.StoredProcedure:
                    text.Append("EXECUTE ");
                    goto case CommandType.Text;
                case CommandType.Text:
                    text.Append(command.CommandText);
                    if (command.Parameters.Count > 0)
                    {
                        text.Append(" ");
                        var s = string.Join(",", command.Parameters.OfType<SqlParameter>()
                            .Where(p => p.Direction == ParameterDirection.Input)
                            .Select(p => p.AsString()));

                        text.Append(s);
                        text.Append(";");
                    }
                    break;
            }
            return text.ToString();
        }

        private static string AsString(this SqlParameter p)
        {
            if (p.Value == null)
                return string.Concat(p.ParameterName, " = NULL");

            switch ((int)p.SqlDbType)
            {
                case (int)SqlDbType.Char:
                case (int)SqlDbType.VarChar:
                case (int)SqlDbType.Time:
                    return string.Concat(p.ParameterName, "='", p.Value.ToString(), "'");
                case (int)SqlDbType.Float:
                    return string.Concat(p.ParameterName, "=", ((float)p.Value).ToString(CultureInfo.InvariantCulture));
                case (int)SqlDbType.Decimal:
                    if (p.Value is double)
                        return string.Concat(p.ParameterName, "=", ((double)p.Value).ToString(CultureInfo.InvariantCulture));
                    if (p.Value is decimal)
                        return string.Concat(p.ParameterName, "=", ((decimal)p.Value).ToString(CultureInfo.InvariantCulture));
                    goto default;
                case (int)SqlDbType.DateTime:
                    return string.Concat(p.ParameterName, "='", ((DateTime)p.Value).ToString("yyyy-MM-dd HH:mm:ss"), "'");
                case (int)SqlDbType.Date:
                    return string.Concat(p.ParameterName, "='", ((DateTime)p.Value).ToString("yyyyMMdd"), "'");
                case (int)SqlDbType.Structured:
                    return string.Concat(p.ParameterName, ":", p.TypeName, "[]={",
                            string.Join(",", ((IEnumerable<SqlDataRecord>)p.Value).AsString()),
                        "}");
                default:
                    return string.Concat(p.ParameterName, "=", p.Value.ToString());
            }
        }

        private static IEnumerable<string> AsString(this IEnumerable<SqlDataRecord> records)
        {
            foreach (var record in records)
            {
                yield return string.Concat("(",string.Join(",", record.AsString()),")");
            }
        }

        private static IEnumerable<string> AsString(this SqlDataRecord record)
        {
            for (int i = 0, N = record.FieldCount; i < N; ++i)
            {
                if (record[i] != null)
                {
                    if (record[i] is string || record[i] is char)
                    {
                        yield return string.Concat("'", record[i].ToString(), "'");
                    }
                    else
                    {
                        if (record[i] is double doubleValue)
                        {
                            yield return doubleValue.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            if (record[i] is DateTime dateTimeValue)
                            {
                                yield return string.Concat("'", dateTimeValue.ToString("yyyyMMdd"), "'");
                            }
                            else
                            {
                                if (record[i] is decimal decimalValue)
                                {
                                    yield return decimalValue.ToString(CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    if (record[i] is DBNull)
                                    {
                                        yield return "NULL";
                                    }
                                    else
                                    {
                                        yield return record[i].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    yield return "NULL";
                }
            }
        }
    }

}
