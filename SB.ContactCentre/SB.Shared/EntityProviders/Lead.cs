using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;

namespace SB.Shared.EntityProviders
{
    public class Lead : LeadModel
    {
        public static readonly ColumnSet EmailFields = new ColumnSet(Fields.Email, Fields.EmailAddress2, Fields.EmailAddress3);

        public Lead(IOrganizationService service) : base(service) { }
        public Lead(IOrganizationService service, Guid id) : base(id, service) { }
        public Lead(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Lead(Entity entity, IOrganizationService service) : base(entity, service) { }

        public string GetEmail()
        {
            if (!string.IsNullOrWhiteSpace(Email))
                return Email;

            return !string.IsNullOrWhiteSpace(EmailAddress2) ? EmailAddress2 : EmailAddress3;
        }
    }
}
