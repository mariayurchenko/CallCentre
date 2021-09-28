using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;

namespace SB.Shared.EntityProviders
{
    public class Account : AccountModel
    {
        public static readonly ColumnSet EmailFields = new ColumnSet(Fields.Email, Fields.EmailAddress2, Fields.EmailAddress3);

        public Account(IOrganizationService service) : base(service) { }
        public Account(IOrganizationService service, Guid id) : base(id, service) { }
        public Account(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Account(Entity entity, IOrganizationService service) : base(entity, service) { }

        public string GetEmail()
        {
            if (!string.IsNullOrWhiteSpace(Email))
                return Email;

            return !string.IsNullOrWhiteSpace(EmailAddress2) ? EmailAddress2 : EmailAddress3;
        }
    }
}
