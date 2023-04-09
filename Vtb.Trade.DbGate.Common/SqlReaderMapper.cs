using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Common
{
    public static class SqlReaderMapper 
    {
        public static IEnumerable<string> AdapterFields<TAdapter>(bool withIdentity = false)
            where TAdapter : CE_Adapter
        {
            var indexConstants = typeof(TAdapter).FieldIndexConstants();
            var identityProperties = typeof(TAdapter).IdentityProperties();

            foreach (var fieldIndex in indexConstants)
            {
                var index = (int)fieldIndex.GetRawConstantValue();
                var fieldName = fieldIndex.Name.Substring(1);
                const BindingFlags propertyBinding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
                var property = typeof(TAdapter).GetProperty(fieldName, propertyBinding);

                if (property is PropertyInfo)
                {
                    if (identityProperties.Contains(fieldName))
                        yield return $"{index}, {fieldName}, string, {(withIdentity? "identity" : "")}";
                    else
                        yield return $"{index}, {fieldName}, {property.PropertyType}";
                }
            }
        }

        private static ISet<string> IdentityProperties(this Type type)
        {
            const BindingFlags propertyBinding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
            return type.GetProperties(propertyBinding)
                .Where(p => p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericTypeDefinition() == typeof(IS<>))
                .Select(p => p.Name)
                .ToHashSet();
        }

        public static IEnumerable<(int Ordinal, string Name, Type type)> MetaInfo
            (this IEnumerable<DbColumn> columns)
        {
            foreach (var col in columns)
            {
                yield return ((int)col.ColumnOrdinal, col.ColumnName, col.DataType);
            }
        }

        public static Action<SqlDataReader, TAdapter> CopyGetter<TAdapter>(this IEnumerable<DbColumn> columns)
            where TAdapter : CE_Adapter
        {
            const BindingFlags propertyBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
            var propertyMatch = columns.MetaInfo()
                .Join(typeof(TAdapter).GetProperties(propertyBindingFlags | BindingFlags.SetField), p => p.Name, p => p.Name,
                (src, dest) => new { src, dest });

            var readerParameter = Expression.Parameter(typeof(SqlDataReader), "r");
            var adapterParameter = Expression.Parameter(typeof(TAdapter), "a");

            var assigns = propertyMatch.Select(pm =>
                //if (!reader.DbNull(ordinal))
                //{
                //      adapter.prop = (PropertType)reader.GetValue(ordinal);
                //}
                Expression.IfThen(Expression.IsFalse(readerParameter.IsDBNull(pm.src.Ordinal)),
                        Expression.Assign(Expression.Property(adapterParameter, pm.dest)
                        , Expression.Convert(readerParameter.GetValue(pm.src.Ordinal, 
                        (pm.dest.PropertyType != typeof(char)) ? pm.src.type : typeof(char)), pm.dest.PropertyType)))
                ).ToArray();


            var actionExpression = Expression.Lambda<Action<SqlDataReader, TAdapter>>(Expression.Block(assigns), readerParameter, adapterParameter);

            return actionExpression.Compile();
        }

        public static Action<SqlDataReader, CE_Adapter> CopyGetter(this IEnumerable<DbColumn> columns,
            (string Name, int Index, Type PropertyType)[] properties)
        {
            var propertyMatch = columns.MetaInfo()
                .Join(properties, p => p.Name, p => p.Name,
                (src, dest) => new { src, dest });

            var readerParameter = Expression.Parameter(typeof(SqlDataReader), "r");
            var adapterParameter = Expression.Parameter(typeof(CE_Adapter), "a");

            var assigns = propertyMatch.Select(pm =>
            {
                var index = Expression.Constant(pm.dest.Index);
                var sourceType = (pm.dest.PropertyType != typeof(char)) ? pm.src.type : typeof(char);
                //if (!reader.DbNull(ordinal))
                //{
                //      adapter[index] = CE.Create(index + 1, (PropertyType)reader.GetValue(ordinal));
                //}
                return Expression.IfThen(Expression.IsFalse(readerParameter.IsDBNull(pm.src.Ordinal)),
                        Expression.Assign(Expression.Property(adapterParameter, SqlReaderExtensions.This, index), 
                            pm.dest.Index.CreateField(
                                Expression.Convert(
                                    readerParameter.GetValue(pm.src.Ordinal, sourceType), pm.dest.PropertyType))));

            }).ToArray();


            var actionExpression = Expression.Lambda<Action<SqlDataReader, CE_Adapter>>(Expression.Block(assigns), readerParameter, adapterParameter);

            return actionExpression.Compile();
        }
    }

    internal static class SqlReaderExtensions
    {
        public static PropertyInfo This => typeof(CE_Adapter).GetProperty("Item");

        public static Expression CreateField(this int index, Expression value)
        {
            return Expression.Call(null, typeof(CE.FieldValue).GetMethod("Create",
                BindingFlags.Public | BindingFlags.Static, new[] { typeof(int), value.Type }),
                Expression.Constant(index + 1), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression IsDBNull(this Expression reader, int ordinal)
            => Expression.Call(reader, typeof(SqlDataReader).GetMethod("IsDBNull", new[] { typeof(int) }), Expression.Constant(ordinal, typeof(int)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression Get(this Expression reader, string methodName, Expression ordinal)
            => Expression.Call(reader, typeof(SqlDataReader).GetMethod(methodName, new[] { typeof(int) }), ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetString(this Expression reader, Expression ordinal)
            => reader.Get("GetString", ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetChar(this Expression reader, Expression ordinal)
            => Expression.Property(reader.Get("GetString", ordinal), "Chars", Expression.Constant(0));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetBoolean(this Expression reader, Expression ordinal)
            => reader.Get("GetBoolean", ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetInt(this Expression reader, Expression ordinal)
            => reader.Get("GetInt32", ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetShort(this Expression reader, Expression ordinal)
            => reader.Get("GetInt16", ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetByte(this Expression reader, Expression ordinal)
            => reader.Get("GetByte", ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetLong(this Expression reader, Expression ordinal)
            => reader.Get("GetInt64", ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetDouble(this Expression reader, Expression ordinal)
            => reader.Get("GetDouble", ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetDecimal(this Expression reader, Expression ordinal)
            => reader.Get("GetDecimal", ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetFloat(this Expression reader, Expression ordinal)
            => reader.Get("GetFloat", ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression GetDateTime(this Expression reader, Expression ordinal)
            => reader.Get("GetDateTime", ordinal);

        public static Expression GetValue(this Expression reader, int iordinal, Type dataType)
        {
            var ordinal = Expression.Constant(iordinal, typeof(int));

            if (dataType == typeof(string))
            {
                return reader.GetString(ordinal);
            }
            else
            if (dataType == typeof(DateTime))
            {
                return reader.GetDateTime(ordinal);
            }
            else
            if (dataType == typeof(int))
            {
                return reader.GetInt(ordinal);
            }
            else
            if (dataType == typeof(long))
            {
                return reader.GetLong(ordinal);
            }
            else
            if (dataType == typeof(float))
            {
                return reader.GetFloat(ordinal);
            }
            else
            if (dataType == typeof(double))
            {
                return reader.GetDouble(ordinal);
            }
            else
            if (dataType == typeof(decimal))
            {
                return reader.GetDecimal(ordinal);
            }
            else
            if (dataType == typeof(bool))
            {
                return reader.GetBoolean(ordinal);
            }
            else
            if (dataType == typeof(short))
            {
                return reader.GetShort(ordinal);
            }
            else
            if (dataType == typeof(byte))
            {
                return reader.GetByte(ordinal);
            }
            else if (dataType == typeof(char))
            {
                return reader.GetChar(ordinal);
            }

            throw new Exception($"ошибка сопоставления {dataType}");
        }
    }
}