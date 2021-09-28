using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;

namespace SB.Shared.EntityProviders
{
    public class SecurityRole : SecurityRoleModel
    {
        public SecurityRole(IOrganizationService service) : base(service) { }
        public SecurityRole(IOrganizationService service, Guid id) : base(id, service) { }
        public SecurityRole(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public SecurityRole(Entity entity, IOrganizationService service) : base(entity, service) { }
    }
}
