using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vtb.Trade.Configuration.Client;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client.App
{
    public static partial class Program
    {
        private static async Task TestRequestService()
        {
            var request = 0.CreateParameters(1).Append("BoardsAdapter");

            request.Append(new CE_Param("@IsMain", true).Items);

            await new DbGateClient(() => ((ChannelProvider)route).DbGate, factory)
                .IncomeEx(
                    ch => new RequestServiceClient(ch),
                    request,
                    entity => Console.WriteLine((/*(BoardAdapter)*/entity).ToString()));
        }
    }
}