using System;
using GraphApi.Service;
using Microsoft.Xrm.Sdk;
using SB.Shared.EntityProviders;
using SB.Shared.Extensions;
using SB.Shared.GraphApiModels;
using SB.Shared.Models.Dynamics;
using JsonSerializer = SB.Shared.JsonSerializer;

namespace Appointment.Messages
{
    public class PostUpdate : IPlugin
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

                var postImage = context.PostEntityImages["PostImage"];

                var entity = postImage.Merge(target);

                var appointment = new SB.Shared.EntityProviders.Appointment(entity, service);

                var appointmentChanged = new SB.Shared.EntityProviders.Appointment(target, service);

                var settings = SBCustomSettings.GetSettings(adminService, SBCustomSettings.MeetingSettingsFields);

                var graph = new GraphApiService(settings.ClientId, settings.ClientSecret, settings.Tenant, settings.Account, settings.Password);

                Meeting meeting;

                if (string.IsNullOrWhiteSpace(appointment.MeetingId))
                {
                    if (appointment.Status.Value == AppointmentModel.StatusEnum.Canceled ||
                        appointment.Status.Value == AppointmentModel.StatusEnum.Completed) return;

                    meeting = appointment.GetMeeting();

                    appointment.MeetingId = graph.CreateMeeting(meeting);

                    appointment.RemoveUnchangedAttributes();
                    appointment.Save();

                    return;
                }

                if (appointment.Status.Value == AppointmentModel.StatusEnum.Canceled)
                {
                    graph.CancelMeeting(appointment.MeetingId);

                    appointment.MeetingId = null;

                    appointment.RemoveUnchangedAttributes();
                    appointment.Save();

                    return;
                }

                meeting = appointmentChanged.GetUpdateMeeting(appointment.Owner.Id, appointment.RequiredAttendees.Entities, appointment.OptionalAttendees.Entities);
                
                graph.UpdateMeeting(meeting, appointment.MeetingId);

            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}