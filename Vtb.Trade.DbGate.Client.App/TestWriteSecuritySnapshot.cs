using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vtb.Trade.Configuration.Client;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client.App
{


    using IS = IS<SCR>;

    public class SCR : IdentitySelectorContext { }
    public class SecurityAdapter_old : CE_Adapter
    {
        public SecurityAdapter_old() : base(101) { ((CE)this).Fields.Capacity = 26; }
        public SecurityAdapter_old(CE data) : base(
            (data.EntityType == 101) ? data : new CE { EntityType = 101 })
        { }

        public static implicit operator SecurityAdapter_old(CE value) => new SecurityAdapter_old(value);

        public override void RegIdentity(Action<CE.FieldValue> registration)
        {
            registration(SecCode);
            registration(Board);
            registration(Exchange);
            registration(Market);
            registration(SecClass);
            registration(SecType);
            registration(PriceType);
            registration(BaseCode);
            registration(GroupCode);
            registration(FaceValueCurrency);
        }

        public const int iSecCode = 0;
        public const int iBoard = 1;
        public const int iExchange = 2;
        public const int iMarket = 3;
        public const int iIsMain = 4;
        public const int iIsin = 5;
        public const int iName = 6;
        public const int iShortName = 7;
        public const int iSecClass = 8;
        public const int iSecurityType = 9;
        public const int iPriceStep = 10;
        public const int iLotSize = 11;
        public const int iExpiryDate = 12;
        public const int iBaseCode = 13;
        public const int iFaceValue = 14;
        public const int iPriceType = 15;
        public const int iFaceValueCurrency = 16;
        public const int iListLevel = 17;
        public const int iStrikePrice = 18;
        public const int iGroupCode = 19;
        public const int iCharTest = 20;

        public IS SecCode
        {
            get => this[iSecCode];
            set => this[iSecCode] = value.AsField(iSecCode + 1);
        }

        public IS Board
        {
            get => this[iBoard];
            set => this[iBoard] = value.AsField(iBoard + 1);
        }

        public IS Exchange
        {
            get => this[iExchange];
            set => this[iExchange] = value.AsField(iExchange + 1);
        }

        public bool IsMain
        {
            get => this[iIsMain]?.AsBoolean ?? default;
            set => this[iIsMain] = (iIsMain + 1, value);
        }

        public IS Market
        {
            get => this[iMarket];
            set => this[iMarket] = value.AsField(iMarket + 1);
        }

        public string Isin
        {
            get => this[iIsin]?.AsString ?? default;
            set => this[iIsin] = (iIsin + 1, value);
        }

        public string Name
        {
            get => this[iName]?.AsString ?? default;
            set => this[iName] = (iName + 1, value);
        }

        public string ShortName
        {
            get => this[iShortName]?.AsString ?? default;
            set => this[iShortName] = (iShortName + 1, value);
        }

        public IS SecClass
        {
            get => this[iSecClass];
            set => this[iSecClass] = value.AsField(iSecClass + 1);
        }

        public IS SecType
        {
            get => this[iSecurityType];
            set => this[iSecurityType] = value.AsField(iSecurityType + 1);
        }

        public decimal PriceStep
        {
            get => (decimal)(this[iPriceStep]?.AsDouble ?? default);
            set => this[iPriceStep] = (iPriceStep + 1, value);
        }

        public int LotSize
        {
            get => this[iLotSize]?.AsInteger ?? default;
            set => this[iLotSize] = (iLotSize + 1, value);
        }

        public DateTime ExpiryDate
        {
            get => this[iExpiryDate]?.AsDateTime ?? default;
            set => this[iExpiryDate] = (iExpiryDate + 1, value);
        }

        public IS BaseCode
        {
            get => this[iBaseCode];
            set => this[iBaseCode] = value.AsField(iBaseCode + 1);
        }

        public decimal FaceValue
        {
            get => (decimal)(this[iFaceValue]?.AsDouble ?? default);
            set => this[iFaceValue] = (iFaceValue + 1, value);
        }

        public IS PriceType
        {
            get => this[iPriceType];
            set => this[iPriceType] = value.AsField(iPriceType + 1);
        }

        public IS FaceValueCurrency
        {
            get => this[iFaceValueCurrency];
            set => this[iFaceValueCurrency] = value.AsField(iFaceValueCurrency + 1);
        }

        public int ListLevel
        {
            get => this[iListLevel]?.AsInteger ?? default;
            set => this[iListLevel] = (iListLevel + 1, value);
        }

        public decimal StrikePrice
        {
            get => (decimal)(this[iStrikePrice]?.AsDouble ?? default);
            set => this[iStrikePrice] = (iStrikePrice + 1, value);
        }

        public IS GroupCode
        {
            get => this[iGroupCode];
            set => this[iGroupCode] = value.AsField(iGroupCode + 1);
        }

        public char CharTest
        {
            get => this[iCharTest]?.AsChar ?? default;
            set => this[iCharTest] = (iCharTest + 1, value);
        }
    }

    partial class Program
    {
        private static async Task<int> TestWriteSecuritySnapshot()
        {
            var result = await new DbGateClient(() => ((ChannelProvider)route).DbGate, factory)
                    .OutcomeEx(                
                        ch => new SecuritySnapshotAddServiceClient(ch),
                        SendTestSnapshot);
            Console.WriteLine(result);
            return ((SecuritySnapshotAddRequestAdapter)result).SnapshotId;
        }

        private static void SendTestSnapshot((Func<CE, Task> send, Func<Task> cancel) forSend)
        {
            Task.Run(async () =>
            {
                await Task.Delay(10000);
                await forSend.send(0.CreateParameters(0));
            });

            Task.Run(async () =>
            {
                var start = new SecuritySnapshotAddRequestAdapter { Source = "MICEX", };
                await forSend.send(start);

                var @params = 0.CreateParameters(4);
                @params.Append(s_securitySnapshot);

                await forSend.send(@params);
            });
        }

        private static CE[] s_securitySnapshot = new[]
        {
            new SecurityAdapter_old
            {
                SecCode = "SBER",
                Board = "TQBR",
                Name = "Сбербанк",
                LotSize = 100,
                FaceValue = 100.1m,
                IsMain = true,
                ExpiryDate = DateTime.Now.AddDays(10).Date,
                CharTest = 'C',
            }.Entity,
            new SecurityAdapter_old
            {
                SecCode = "GAZP",
                Board = "TQBR",
                Name = "Газпром",
                LotSize = 10,
                FaceValue = 10.1m,
                IsMain = false,
                ExpiryDate = DateTime.Now.AddDays(4).Date,
                CharTest = 'A',
            }.Entity,
            new SecurityAdapter_old
            {
                SecCode = "VTBR",
                Board = "TQBR",
                Name = "ВТБ",
                LotSize = 1000,
                FaceValue = 510.1m,
                IsMain = true,
                ExpiryDate = DateTime.Now.AddDays(20).Date,
                CharTest = 'B',
            }.Entity,
        };
    }
}
