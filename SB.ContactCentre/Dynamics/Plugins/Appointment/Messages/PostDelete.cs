using System;
using Microsoft.Xrm.Sdk;
using GraphApi.Service;
using SB.Shared.EntityProviders;

namespace Appointment.Messages
{
    public class PostDelete : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var adminService = factory.CreateOrganizationService(null);

            try
            {
                var preImage = context.PreEntityImages["PreImage"];

                var appointment = new SB.Shared.EntityProviders.Appointment(preImage, service);

                if (string.IsNullOrWhiteSpace(appointment.MeetingId)) return;

                var settings = SBCustomSettings.GetSettings(adminService, SBCustomSettings.MeetingSettingsFields);

                var graph = new GraphApiService(settings.ClientId, settings.ClientSecret, settings.Tenant, settings.Account, settings.Password);

                graph.DeleteMeeting(appointment.MeetingId);

            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}