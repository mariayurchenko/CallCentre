using System;
using Microsoft.Xrm.Sdk;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Dynamics;

namespace SB.Call.Messages
{
    public class PreCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                var target = (Entity)context.InputParameters["Target"];

                var call = new Shared.EntityProviders.Call(target, service);

                if (call.PhoneNumber == null || call.Contact != null) return;

                var contact = Contact.FindContactByPhoneOrCreateContact(call.PhoneNumber, service, ContactModel.Fields.Birthday);

                if (contact.Id != null)
                {
                    target[CallModel.Fields.Contact] = new EntityReference(ContactModel.LogicalName, contact.Id.Value);
                }
                else
                {
                    throw new Exception("Contact was not found and created");
                }

                if (contact.Birthday != null)
                {
                    target[CallModel.Fields.Birthday] = contact.Birthday;
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}