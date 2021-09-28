using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;

namespace SB.Shared.EntityProviders
{
    public class ActivityParty : ActivityPartyModel
    {
        public ActivityParty(IOrganizationService service) : base(service) { }
        public ActivityParty(IOrganizationService service, Guid id) : base(id, service) { }
        public ActivityParty(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public ActivityParty(Entity entity, IOrganizationService service) : base(entity, service) { }
    }
}