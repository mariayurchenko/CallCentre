using Microsoft.Xrm.Sdk;
using System;
using GraphApi.Service;
using SB.Shared.EntityProviders;

namespace Appointment.Messages
{
    public class PostCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var adminService = factory.CreateOrganizationService(null);

            try
            {
                var target = (Entity)context.InputParameters["Target"];

                var appointment = new SB.Shared.EntityProviders.Appointment(target, service);

                var settings = SBCustomSettings.GetSettings(adminService, SBCustomSettings.MeetingSettingsFields);

                var graph = new GraphApiService(settings.ClientId, settings.ClientSecret, settings.Tenant, settings.Account, settings.Password);

                var meeting = appointment.GetMeeting();

                appointment.MeetingId = graph.CreateMeeting(meeting);

                appointment.RemoveUnchangedAttributes();
                appointment.Save();

            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}