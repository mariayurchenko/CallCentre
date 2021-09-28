using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;

namespace SB.Shared.EntityProviders
{
    public class Task : TaskModel
    {
        public Task(IOrganizationService service) : base(service) { }
        public Task(IOrganizationService service, Guid id) : base(id, service) { }
        public Task(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Task(Entity entity, IOrganizationService service) : base(entity, service) { }

    }
}
