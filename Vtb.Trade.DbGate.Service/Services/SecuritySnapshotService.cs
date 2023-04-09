using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Vtb.Trade.Grpc.Common;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.DbGate.Client;
using Vtb.Trade.DbGate.Service.Repositories;
using Vtb.Trade.DbGate.Service.Registration;

namespace Vtb.Trade.DbGate.Service
{
    public class SecuritySnapshotService : SecuritySnapshotGetBase
    {
        private readonly SecuritySnapshotRepository m_repository;
        private readonly ILogger<SecuritySnapshotService> m_logger;

        public SecuritySnapshotService(
            ILogger<SecuritySnapshotService> logger,
            IOptions<RepositoryOptions> repositoryOptions)
        {
            m_logger = logger;
            m_repository = new SecuritySnapshotRepository(repositoryOptions.Value);
        }

        public override async Task IncomeEx(CE request, IServerStreamWriter<CE> responseStream, ServerCallContext context)
        {
            m_logger.LogInformation($"{DateTime.Now}\tConnect Host:{context.Host} Remote:{context.Peer}\n\t" +
                $"Request:{request.AsFlatString()}");

            var requestAdapter = (SecuritySnapshotRequestAdapter)request;
            switch (requestAdapter.ResultsObject)
            {
                case ResultObjectEnum.Adapter:
                    await m_repository.GetAdapters(requestAdapter)
                        .GetAdapters().IncomeEx(responseStream, packetSize: requestAdapter.PacketSize);
                    break;
                case ResultObjectEnum.Entity:
                    await m_repository.GetAdapters(requestAdapter)
                        .GetEntities().IncomeEx(responseStream, packetSize: requestAdapter.PacketSize);
                    break;
            }
        }
    }

    public static class ConvertExtension
    {
        public static IEnumerable<CE> GetAdapters(this IEnumerable<SecuritySnapshotAdapter> snapshotAdapters)
        {
            using var snapshotEnumerator = snapshotAdapters.GetEnumerator();

            var entityId = -1;
            var destination = default(CE_Adapter);

            while (snapshotEnumerator.MoveNext())
            {
                var source = snapshotEnumerator.Current;
                if (entityId != source.EntityId)
                {
                    if (destination != null)
                        yield return destination;

                    entityId = source.EntityId;
                    destination = new CE_Adapter(source.EntityType);
                }
                source.CopyFieldValue(destination);
            }

            if (destination != null)
                yield return destination;
        }

        public static IEnumerable<CE> GetEntities(this IEnumerable<SecuritySnapshotAdapter> snapshotAdapters)
        {
            using var snapshotEnumerator = snapshotAdapters.GetEnumerator();

            var entityId = -1;
            var destination = default(CE);

            while (snapshotEnumerator.MoveNext())
            {
                var source = snapshotEnumerator.Current;
                if (entityId != source.EntityId)
                {
                    if (destination != null)
                        yield return destination;

                    entityId = source.EntityId;
                    destination = new CE() { EntityType = source.EntityType };
                }
                source.CopyFieldValue(destination);
            }

            if (destination != null)
                yield return destination;
        }


        public static Dictionary<int, Func<SecuritySnapshotAdapter, CE.FieldValue>> s_fieldConverter =
            new Dictionary<int, Func<SecuritySnapshotAdapter, CE.FieldValue>>
            {
                { (int)CE.ValueType.AsBoolean, a => (a.FieldNumber, a.AsBoolean)},
                { (int)CE.ValueType.AsChar, a => (a.FieldNumber, a.AsChar) },
                { (int)CE.ValueType.AsDateTime, a => (a.FieldNumber, a.AsDateTime)},
                { (int)CE.ValueType.AsDouble, a => (a.FieldNumber, a.AsDouble)},
                { (int)CE.ValueType.AsInteger, a => (a.FieldNumber, a.AsInt)},
                { (int)CE.ValueType.AsLong, a => (a.FieldNumber, a.AsLong)},
                { (int)CE.ValueType.AsString, a => (a.FieldNumber, a.AsString)},
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFieldValue(this SecuritySnapshotAdapter src, CE_Adapter dest)
        {
            if (s_fieldConverter.TryGetValue(src.FieldType, out var getter))
                dest[src.FieldNumber - 1] = getter(src);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFieldValue(this SecuritySnapshotAdapter src, CE dest)
        {
            if (s_fieldConverter.TryGetValue(src.FieldType, out var getter))
                dest.Fields.Add(getter(src));
        }
    }
}
