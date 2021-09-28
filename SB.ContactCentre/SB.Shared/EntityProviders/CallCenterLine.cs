using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Extensions;
using SB.Shared.Models.Dynamics;

namespace SB.Shared.EntityProviders
{
    public class CallCenterLine : CallCenterLineModel
    {
        public CallCenterLine(IOrganizationService service) : base(service) { }
        public CallCenterLine(IOrganizationService service, Guid id) : base(id, service) { }
        public CallCenterLine(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public CallCenterLine(Entity entity, IOrganizationService service) : base(entity, service) { }

        public static CallCenterLine FindLineByNumberLineOrReturnNull(IOrganizationService service, string phoneNumber, params string[] columns)
        {
            var query = new QueryExpression(LogicalName);

            query.ColumnSet.AddColumns(columns);
            query.Criteria.AddCondition(Fields.LineNumber, ConditionOperator.Equal, phoneNumber);

            var line = service.RetrieveMultiple(query).Entities.Select(entity => new CallCenterLine(entity, service)).FirstOrDefault();

            return line;
        }

        public void IncreasePhoneCallsForTheDay()
        {
            if (Phonecallsfortheday == null)
            {
                Phonecallsfortheday = 0;
            }

            Phonecallsfortheday++;
        }

        public static void ResettingCallCounterOnLine(IOrganizationService service)
        {
            var query = new QueryExpression(LogicalName);
            query.ColumnSet.AddColumn(Fields.Phonecallsfortheday);

            var lines = service.RetrieveMultiple(query).ToEntityCollection<CallCenterLine>(service);

            foreach (var line in lines)
            {
                line.Phonecallsfortheday = 0;
                line.Save();
            }
        }
    }
}