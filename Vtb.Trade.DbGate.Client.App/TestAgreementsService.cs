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
        private static async Task TestAgreementsService()
        {
            await new DbGateClient(() => ((ChannelProvider)route).DbGate, factory)
                .IncomeEx(
                    ch => new AgreementServiceClient(ch), 
                    DbClientRequests.CreateAgreementsRequest(100), 
                    entity => Console.WriteLine(entity.ToString()));
        }
    }
}
