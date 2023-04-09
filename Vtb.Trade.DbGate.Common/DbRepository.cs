using Microsoft.SqlServer.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Common
{
    [Obsolete("Use DbRequestRepository")]
    public abstract class DbRepository<TAdapter, TRequest> where TAdapter : CE_Adapter where TRequest : RequestAdapter
    {
        private readonly string ConnectionString;
        private readonly Action<CE.FieldValue> IdentityRegistration;

        public DbRepository(RepositoryOptions repositoryOptions, Action<CE.FieldValue> identityAdd)
        {
            ConnectionString = repositoryOptions.ConnectionString;
            IdentityRegistration = identityAdd;
        }

        public abstract TAdapter CreateAdapter();

        public IEnumerable<CE> Get(TRequest request)
        {
            foreach (var result in InternalGet(request))
                yield return result;
        }

        public IEnumerable<TAdapter> GetAdapters(TRequest request)
        {
            foreach (var result in InternalGet(request))
                yield return result;
        }

        protected IEnumerable<TAdapter> InternalGet(TRequest request)
        {
            var adapter = CreateAdapter();
            var fields = request.Fields.ToHashSet();
            var copy = default(Action<SqlDataReader, TAdapter>);
            var sqlCommand = default(SqlCommand);

            using var sqlConnection = new SqlConnection(ConnectionString);

            sqlConnection.Open();
            using (sqlCommand = sqlConnection.CreateCommand())
            {

                using var sqlDataReader = sqlCommand.Apply(request).ExecuteReader(CommandBehavior.SchemaOnly);
                copy = SqlReaderMapper.CopyGetter<TAdapter>(
                    sqlDataReader.GetColumnSchema()
                        .Where(c => fields.Count == 0 || fields.Contains(c.ColumnName)));
            }

            using (sqlCommand = sqlConnection.CreateCommand())
            {
                using var sqlDataReader = sqlCommand.Apply(request)
                    .ExecuteReader();
                while (sqlDataReader.Read())
                {
                    copy(sqlDataReader, adapter);
                    if (request.WithIdentity && IdentityRegistration != null)
                        adapter.RegIdentity(IdentityRegistration);
                    yield return adapter;
                    adapter.Reset();
                }
            }
        }
    }

    public static class DbRepositoryExtensions
    {
        public static SqlCommand Apply(this SqlCommand c, RequestAdapter request)
        {
            c.CommandType = request.CommandType;
            c.CommandText = request.CommandText;
            c.CommandTimeout = request.CommandTimeout;

            foreach (var parameter in request.Parameters)
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
    }
}