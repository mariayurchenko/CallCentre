using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Extensions;
using SB.Shared.Models.Dynamics;

namespace SB.Shared.EntityProviders
{
    public class Contact : ContactModel
    {
        public static readonly ColumnSet EmailFields = new ColumnSet(Fields.Email, Fields.EmailAddress2, Fields.EmailAddress3);

        public Contact(IOrganizationService service) : base(service) { }
        public Contact(IOrganizationService service, Guid id) : base(id, service) { }
        public Contact(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Contact(Entity entity, IOrganizationService service) : base(entity, service) { }

        public static Contact FindContactByPhoneOrCreateContact(string phoneNumber, IOrganizationService service, params string[] columns)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new Exception($"{phoneNumber} is null, empty or white-space");
            }

            var query = new QueryExpression(LogicalName);

            query.Criteria.AddCondition(Fields.MobilePhone, ConditionOperator.Equal, phoneNumber);
            query.ColumnSet.AddColumns(columns);

            var contact = service.RetrieveMultiple(query).ToEntityCollection<Contact>(service).FirstOrDefault();

            if (contact != null) 
                return contact;

            contact = new Contact(service)
            {
                MobilePhone = phoneNumber,
                FirstName = "Входящий звонок"
            };

            contact.Save();

            return contact;
        }

        public string GetEmail()
        {
            if (!string.IsNullOrWhiteSpace(Email))
                return Email;

            return !string.IsNullOrWhiteSpace(EmailAddress2) ? EmailAddress2 : EmailAddress3;
        }

    }
}