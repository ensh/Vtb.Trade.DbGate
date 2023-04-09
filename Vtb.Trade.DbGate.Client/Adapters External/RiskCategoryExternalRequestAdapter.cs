namespace Vtb.Trade.DbGate.Client
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Vtb.Trade.DbGate.Common;
    using Vtb.Trade.Grpc.Common;

    public class RiskCategoryExternalRequestAdapter : RequestAdapter
    {
        public RiskCategoryExternalRequestAdapter() : base(EntityType.RiskCategory) { ((CE)this).Fields.Capacity = 4; }
        public RiskCategoryExternalRequestAdapter(CE data) : base(
            (data.EntityType == EntityType.RiskCategory) ? data : new CE { EntityType = EntityType.RiskCategory })
        { }

        public static implicit operator RiskCategoryExternalRequestAdapter(CE entity) => new RiskCategoryExternalRequestAdapter(entity);

        public override CommandType CommandType { get => CommandType.StoredProcedure; }
        public override string CommandText { get => @"olb.vtb_get_risk_categories"; }
        public override int CommandTimeout
        {
            get => this[0]?.AsInteger ?? 3600;
            set => this[0] = CE.FieldValue.Create(1, value);
        }

        public string Columns
        {
            get => this[1]?.AsString ?? default;
            set => this[1] = CE.FieldValue.Create(2, value);
        }

        public override IEnumerable<(string name, object value)> Parameters
        {
            get
            {
                yield break;
            }
        }

        public override IEnumerable<string> Fields
        {
            get
            {
                var fields = (Columns ?? "").Split(';', StringSplitOptions.RemoveEmptyEntries);

                foreach (var f in fields)
                {
                    yield return f;
                }
            }
        }
    }
}
