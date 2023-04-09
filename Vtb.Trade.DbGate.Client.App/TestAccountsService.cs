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
        private static async Task TestAccountsService()
        {
            try
            {
                int count = 0;
                await new DbGateClient(() => ((ChannelProvider)route).DbGate, factory)
                    .IncomeEx(
                        ch => new AccountServiceClient(ch),
                        DbClientRequests.CreateAccountsRequest(1000),
                        entity => Console.WriteLine($"{++count}: {entity.AsFlatString()}"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
