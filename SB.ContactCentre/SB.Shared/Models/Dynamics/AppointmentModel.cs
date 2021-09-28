using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class AppointmentModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "appointment";

		#region Attribute Names
		public static class Fields
		{
			public const string
				AllDayEvent = "isalldayevent",
				Description = "description",
				EndTime = "scheduledend",
				Location = "location",
				MeetingId = "sb_meetingid",
				OnlineMeeting = "sb_isonlinemeeting",
				OptionalAttendees = "optionalattendees",
				Owner = "ownerid",
				PrimaryId = "activityid",
				Regarding = "regardingobjectid",
				RequiredAttendees = "requiredattendees",
				StartTime = "scheduledstart",
				Status = "statecode",
				Subject = "subject";

			public static string[] All => new[] { AllDayEvent,
				Description,
				EndTime,
				Location,
				MeetingId,
				OnlineMeeting,
				OptionalAttendees,
				Owner,
				PrimaryId,
				Regarding,
				RequiredAttendees,
				StartTime,
				Status,
				Subject };
		}
		#endregion

		#region Enums
		public static class StatusEnum
		{
			public const int
				Open = 0,
				Completed = 1,
				Canceled = 2,
				Scheduled = 3;
		}
		#endregion

		#region Field Definitions
		public bool? AllDayEvent
		{
			get => (bool?)this[Fields.AllDayEvent];
			set => this[Fields.AllDayEvent] = value; 
		}
		public string Description
		{
			get => (string)this[Fields.Description];
			set => this[Fields.Description] = value; 
		}
		public DateTime? EndTime
		{
			get => (DateTime?)this[Fields.EndTime];
			set => this[Fields.EndTime] = value; 
		}
		public string Location
		{
			get => (string)this[Fields.Location];
			set => this[Fields.Location] = value; 
		}
		public string MeetingId
		{
			get => (string)this[Fields.MeetingId];
			set => this[Fields.MeetingId] = value; 
		}
		public bool? OnlineMeeting
		{
			get => (bool?)this[Fields.OnlineMeeting];
			set => this[Fields.OnlineMeeting] = value; 
		}
		public EntityCollection OptionalAttendees
		{
			get => (EntityCollection)this[Fields.OptionalAttendees];
			set => this[Fields.OptionalAttendees] = value; 
		}
		public EntityReference Owner
		{
			get => (EntityReference)this[Fields.Owner];
			set => this[Fields.Owner] = value; 
		}
		public EntityReference Regarding
		{
			get => (EntityReference)this[Fields.Regarding];
			set => this[Fields.Regarding] = value; 
		}
		public EntityCollection RequiredAttendees
		{
			get => (EntityCollection)this[Fields.RequiredAttendees];
			set => this[Fields.RequiredAttendees] = value; 
		}
		public DateTime? StartTime
		{
			get => (DateTime?)this[Fields.StartTime];
			set => this[Fields.StartTime] = value; 
		}
		public new OptionSetValue Status
		{
			get => (OptionSetValue)this[Fields.Status];
			set => this[Fields.Status] = value; 
		}
		public string Subject
		{
			get => (string)this[Fields.Subject];
			set => this[Fields.Subject] = value; 
		}
		#endregion

		#region Constructors
		protected AppointmentModel()
			: base(LogicalName) { }
		protected AppointmentModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected AppointmentModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected AppointmentModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected AppointmentModel(Entity entity, IOrganizationService service)
			: base(entity, service) { }
		#endregion

		#region Public Methods
		public override string GetPrimaryAttribute()
        {
            return Fields.PrimaryId;
        }
		#endregion
	}
}