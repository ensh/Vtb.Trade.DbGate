using System.Collections.Generic;
using System.Data;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Common
{
    public abstract class RequestAdapter : CE_Adapter
    {
        public RequestAdapter(int entityType) : base(entityType) { }
        public RequestAdapter(CE data) : base(data) { }

        public abstract CommandType CommandType { get; }
        public abstract string CommandText { get; }
        public abstract int CommandTimeout { get; set; }
        public virtual bool WithIdentity { get; set; } = false;
        public virtual int PacketSize { get; set; } = 100;
        public abstract IEnumerable<(string name, object value)> Parameters
        {
            get;
        }

        public abstract IEnumerable<string> Fields
        {
            get;
        }
    }
}
