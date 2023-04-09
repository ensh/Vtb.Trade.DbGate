using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Grpc.Common;

namespace Vtb.Trade.DbGate.Client
{
    public class SecuritySnapshotAddRequestAdapter : SecuritySnapshotRequestAdapter
    {        
        
        public SecuritySnapshotAddRequestAdapter() : base()
        { }

        public SecuritySnapshotAddRequestAdapter(CE data) : base(data)
        { }

        public static implicit operator SecuritySnapshotAddRequestAdapter(CE entity) => new SecuritySnapshotAddRequestAdapter(entity);

        public override CommandType CommandType => CommandType.StoredProcedure;

        public override string CommandText => "gate.AddSecuritySnapshot";

        public bool Commit { get; set; }
        public IEnumerable<CE> SnapshotEnities { get; set; }

        public override IEnumerable<(string name, object value)> Parameters
            => base.Parameters.Concat(GetParameters());
        
        private IEnumerable<(string name, object value)> GetParameters()
        {
            yield return ("@SecuritySnapshotFields", SnapshotEnities.SecuritySnapshotFieldsParameter());
            if (Commit) yield return ("@Commit", true);
        }

        public override IEnumerable<string> Fields => Enumerable.Empty<string>();
    }
}
