using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Actions;
using SB.Shared.Models.Dynamics;
using SB.SharedModels.Actions;

namespace SB.Task.Messages
{
    public class PostCreate: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                var target = (Entity)context.InputParameters["Target"];

                var task = new Shared.EntityProviders.Task(target, service);

                if (task.PhoneCall != null)
                {
                     var call = new Call(task.PhoneCall.Id, new ColumnSet(CallModel.Fields.Task, CallModel.Fields.Contact, CallModel.Fields.PhoneNumber), service);

                     if (task.Id != null) call.Task = new EntityReference(TaskModel.LogicalName, task.Id.Value);

                     call.Save();

                     if (task.PhoneNumber == null && call.PhoneNumber != null)
                     {
                         task.PhoneNumber = call.PhoneNumber;
                     }
                     if (task.Contact == null && call.Contact != null)
                     {
                         task.Contact = new EntityReference(ContactModel.LogicalName, call.Contact.Id);
                     }
                }
                if (task.PhoneNumber != null && task.Contact == null)
                {
                    var contact = Contact.FindContactByPhoneOrCreateContact(task.PhoneNumber, service);

                    if (contact.Id != null)
                    {
                        task.Contact = new EntityReference(ContactModel.LogicalName, contact.Id.Value);
                    }
                    else
                    {
                        throw new Exception("Contact was not found and created");
                    }

                }
                else if (task.PhoneNumber == null && task.Contact != null)
                {
                    var contact = new Contact(task.Contact.Id, new ColumnSet(ContactModel.Fields.MobilePhone), service);

                    task.PhoneNumber = contact.MobilePhone;
                }
                if (task.TaskType?.Value == TaskModel.TaskTypeEnum.Перезвонить && task.Callbackdate == null &&
                    task.PhoneCall != null)
                {
                    var settings = SBCustomSettings.GetSettings(service, SBCustomSettingsModel.Fields.Callbacktime);

                    if (settings.Callbacktime == null)
                    {
                        throw new ArgumentNullException($"{nameof(settings.Callbacktime)} is null");
                    }

                    var req = new OrganizationRequest(ActionNames.ActionTracking)
                    {
                        ["ActionName"] = ActionNames.ActionTrackingNames.CurrentDateTime
                    };


                    var response = service.Execute(req);

                    var actionResponse =
                        JsonSerializer.Deserialize<ActionResponse>(response.Results["Response"].ToString());

                    if (actionResponse.Value == null)
                    {
                        throw new ArgumentNullException($"{nameof(actionResponse.Value)} is null");
                    }

                    var date = Convert.ToDateTime(actionResponse.Value);

                    task.Callbackdate = date.AddMinutes(settings.Callbacktime.Value);
                }

                task.RemoveUnchangedAttributes();
                task.Save();

            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}