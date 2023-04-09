using Microsoft.Extensions.Logging;

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

        static ILoggerFactory factory;
        static RouteEndpoints route;
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Press ENTER to request data...");
            //Console.ReadLine();

            factory = LoggerFactory.Create(logBuilder =>
            {
                logBuilder.ClearProviders();
                logBuilder.AddConsole();
                logBuilder.SetMinimumLevel(LogLevel.Debug);
            });

            route = (args[0], args[1]).QueryConfiguration(factory.CreateLogger<ConfigurationClient>())
                .Route();

            AppDomain.CurrentDomain.UnhandledException +=
                (s, e) => Console.WriteLine(e.ExceptionObject.ToString());


            //await TestMoneyRemainsService(0);
            //await TestMoneyRemainsService(1);
            //await TestMoneyRemainsService(2);
            //await TestMoneyRemainsService(3);
            //await TestSecurityRemainsService(0);
            //await TestSecurityRemainsService(1);
            //await TestSecurityRemainsService(2);
            //await TestSecurityRemainsService(3);

            //await TestRequestService();
            //await TestAccountsService();
            //await TestAgreementsService();
            //await TestBoardService();
            //var snapshotId = await TestWriteSecuritySnapshot();
            //await TestReadSecuritySnapshot(snapshotId);
            //await TestReadSecuritySnapshot(21);

            await TestQueryCategory();

            Console.ReadLine();
        }

    }
}
