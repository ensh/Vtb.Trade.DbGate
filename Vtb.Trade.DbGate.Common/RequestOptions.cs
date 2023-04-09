using System.Collections.Generic;
using System.Data;

using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Common
{
    public class RequestOptions
    {
        public string CommandType { get; set; } = "StoredProcedure"; //string(System.Data.CommandType)
        public string CommandText { get; set; }
        public int CommandTimeout { get; set; } = 600; // секунды
        public int PacketSize { get; set; } = 200;
        public int PacketTimeout { get; set; } = 1000; // миллисекунды
        public bool WithIdentity { get; set; } = true;
        public string MappingName { get; set; }
        public string [] FieldMapping { get; set; } //int(fieldindex),sstring(FieldName),string(System.Data.SqlDbType),int(length)
        public int EntityType { get; set; } = 0;
        public string[] Fields { get; set; } //int(Index),string(FieldName),string(Int, string etc), identity       
    }
}
