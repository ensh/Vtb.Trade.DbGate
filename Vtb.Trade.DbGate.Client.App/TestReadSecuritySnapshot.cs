using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vtb.Trade.Configuration.Client;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client.App
{
    partial class Program
    {
        private static async Task TestReadSecuritySnapshot(int snapshotId, ResultObjectEnum resultObject = ResultObjectEnum.Adapter)
        {
            var request = DbClientRequests.CreateSecuritySnapshotRequest(snapshotId, resultObject);
            await new DbGateClient(() => ((ChannelProvider)route).DbGate, factory)
                .IncomeEx(
                    ch => new SecuritySnapshotServiceClient(ch),
                    request,
                    entity => Console.WriteLine(entity.ToString()));
        }
    }
}
