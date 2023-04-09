using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Vtb.Trade.Configuration.Client;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client.App
{
    public static partial class Program
    {
        private static async Task TestRemainsService(string commandName, int regime)
        {
            var request = 0.CreateParameters(1).Append(commandName)
                .Append(new CE_Param("@regime", regime).Items);

            Console.WriteLine($"Start: {request.AsFlatString()}");
            var t = Stopwatch.StartNew();
            var count = 1;
            await new DbGateClient(() => ((ChannelProvider)route).DbGate, factory)
                .IncomeEx(
                    ch => new RequestServiceClient(ch),
                    request,
                    entity =>
                    {
                        count++;
                        /*Console.WriteLine(entity.AsFlatString())*/

                        if (count % 50000 == 0)
                            Console.WriteLine($"loaded: {count}");

                    },
                    () => Console.WriteLine($"Finish {count} remains at {t.ElapsedMilliseconds / 1000}s"));
        }

        private static async Task TestMoneyRemainsService(int regime)
            => await TestRemainsService("MoneyRemains", regime);

        private static async Task TestSecurityRemainsService(int regime)
            => await TestRemainsService("SecurityRemains", regime);
    }
}
