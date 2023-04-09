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
        private static async Task TestQueryCategory()
        {
            var request = 0.CreateParameters(1).Append("AccountCategory");

            //request.Append(new CE_Param("@IsMain", true).Items);
            /*
65001:1045EY:*
65002:1045FA:*
65003:1045FD:*
65004:1045FE:*
65005:1045FG:*
65006:1045FK:*
65007:1045FR:*
65008:1045G6:*
65009:1045GA:*
             */

            var textNumbers = new[] 
            {
                "1045EY", "1045FA", "1045FD", "1045FE", "1045FG", "1045FK", "1045FR", "1045G6", "1045GA",
            };

            var idNumbers = new[]
            {
                65001, 65002, 65003, 65004, 65005, 65006, 65007, 65008, 65009,
            };

            //request.Append(textNumbers.Select(txt => CE.Create((1, txt))));
            request.Append(idNumbers.Select(id => CE.Create((1, id))));

            await new DbGateClient(() => ((ChannelProvider)route).DbGate, factory)
                .Query(
                    ch => new RequestServiceClient(ch),
                    new[] { request },
                    entity => Console.WriteLine(entity.ToString()));
        }
    }
}
