using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.GraphApiModels;
using SB.Shared.Models.Dynamics;

namespace SB.Shared.EntityProviders
{
    public class Appointment : AppointmentModel
    {
        public Appointment(IOrganizationService service) : base(service) { }
        public Appointment(IOrganizationService service, Guid id) : base(id, service) { }
        public Appointment(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Appointment(Entity entity, IOrganizationService service) : base(entity, service) { }

        public List<string> GetEntitiesEmails(DataCollection<Entity> entities)
        {
            var emailsList = new List<string>();

            foreach (var entity in entities)
            {
                var activityParty = new ActivityParty(entity, _service);

                if (string.IsNullOrEmpty(activityParty.Address))
                {
                    string email = null;

                    switch (activityParty.Party.LogicalName)
                    {
                        case ContactModel.LogicalName:
                            var contact = new Contact(activityParty.Party.Id, Contact.EmailFields, _service);
                            email = contact.GetEmail();
                            break;
                        case UserModel.LogicalName:
                            var user = new User(activityParty.Party.Id, User.EmailFields, _service);
                            email = user.GetEmail();
                            break;
                        case AccountModel.LogicalName:
                            var account = new Account(activityParty.Party.Id, Account.EmailFields, _service);
                            email = account.GetEmail();
                            break;
                        case LeadModel.LogicalName:
                            var lead = new Lead(activityParty.Party.Id, Lead.EmailFields, _service);
                            email = lead.GetEmail();
                            break;
                    }

                    if (!string.IsNullOrWhiteSpace(email))
                        emailsList.Add(email);
                }
                else
                {
                    emailsList.Add(activityParty.Address);
                }
            }

            return emailsList;
        }

        public Meeting GetMeeting()
        {
            var user = new User(Owner.Id, new ColumnSet(UserModel.Fields.PrimaryEmail), _service);

            var meeting = new Meeting
            {
                Subject = Subject,
                Body = new Body { Content = Description },
                Location = new Location { DisplayName = Location },
                IsOnlineMeeting = OnlineMeeting ?? false,
                Start = new Date
                {
                    TimeZone = "Pacific Standard Time",
                    DateTime = StartTime ??
                               throw new Exception($"{nameof(StartTime)} is null")
                },
                End = new Date
                {
                    TimeZone = "Pacific Standard Time",
                    DateTime = EndTime ??
                               throw new Exception($"{nameof(EndTime)} is null")
                },
                Attendees = new List<Attendee> { new Attendee { EmailAddress = new EmailAddress { Address = user.PrimaryEmail }, Type = "required" } }
            };

            if (RequiredAttendees.Entities.Count > 0)
            {
                var emails = GetEntitiesEmails(RequiredAttendees.Entities);

                foreach (var email in emails)
                {
                    meeting.Attendees.Add(new Attendee { EmailAddress = new EmailAddress { Address = email }, Type = "required" });
                }
            }

            if (OptionalAttendees.Entities.Count > 0)
            {
                var emails = GetEntitiesEmails(OptionalAttendees.Entities);

                foreach (var email in emails)
                {
                    meeting.Attendees.Add(new Attendee { EmailAddress = new EmailAddress { Address = email }, Type = "optional" });
                }
            }

            return meeting;
        }

        public Meeting GetUpdateMeeting(Guid ownerId, DataCollection<Entity> requiredAttendees, DataCollection<Entity> optionalAttendees)
        {
            var meeting = new Meeting();

            if (!string.IsNullOrEmpty(Subject))
            {
                meeting.Subject = Subject;
            }
            if (!string.IsNullOrEmpty(Description))
            {
                meeting.Body = new Body { Content = Description };
            }
            if (!string.IsNullOrEmpty(Location))
            {
                meeting.Location = new Location { DisplayName = Location };
            }
            if (OnlineMeeting != null)
            {
                meeting.IsOnlineMeeting = OnlineMeeting.Value;
            }
            if (StartTime != null)
            {
                meeting.Start = new Date
                {
                    TimeZone = "Pacific Standard Time",
                    DateTime = StartTime.Value
                };
            }
            if (EndTime != null)
            {
                meeting.End = new Date
                {
                    TimeZone = "Pacific Standard Time",
                    DateTime = EndTime.Value
                };
            }

            if (RequiredAttendees.Entities.Count > 0 || OptionalAttendees.Entities.Count > 0)
            {
                var user = new User(ownerId, new ColumnSet(UserModel.Fields.PrimaryEmail), _service);

                meeting.Attendees = new List<Attendee>
                    {new Attendee {EmailAddress = new EmailAddress {Address = user.PrimaryEmail}, Type = "required"}};

                var emails = GetEntitiesEmails(requiredAttendees);

                foreach (var email in emails)
                {
                    meeting.Attendees.Add(new Attendee { EmailAddress = new EmailAddress { Address = email }, Type = "required" });
                }

                emails = GetEntitiesEmails(optionalAttendees);

                foreach (var email in emails)
                {
                    meeting.Attendees.Add(new Attendee { EmailAddress = new EmailAddress { Address = email }, Type = "optional" });
                }
            }
            
            return meeting;
        }
    }
}