using System;

using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class AccountChangeExternalAdapter : CE_Adapter
    {
        public AccountChangeExternalAdapter() : base(EntityType.AccountChange) { ((CE)this).Fields.Capacity = 6; }
        public AccountChangeExternalAdapter(CE data) : base(
            (data.EntityType == EntityType.AccountChange) ? data : new CE { EntityType = EntityType.AccountChange }) { }

        public const int iId = 0;
        public const int iActionCode = 1;
        public const int iBody = 2;

        public static implicit operator AccountChangeExternalAdapter(CE entity)
        => new AccountChangeExternalAdapter(entity);

        /// <summary>
        /// Id торгового счета
        /// </summary>
        public long Id
        {
            get => this[iId]?.AsLong ?? default;
            set => this[iId] = (iId + 1, value);
        }

        /// <summary>
        /// Код операции
        /// </summary>
        public int Action
        {
            get => this[iActionCode]?.AsInteger ?? default;
            set => this[iActionCode] = (iActionCode + 1, value);
        }

        /// <summary>
        /// Данные в формате JSON
        /// </summary>
        public string Body
        {
            get => this[iBody]?.AsString ?? default;
            set => this[iBody] = (iBody + 1, value);
        }
    }
}
