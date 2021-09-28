using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class CallModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_call";

		#region Attribute Names
		public static class Fields
		{
			public const string
				Birthday = "sb_dateofbirth",
				CallType = "sb_calltype",
				Contact = "sb_contactid",
				CreatedOn = "createdon",
				Direction = "sb_direction",
				ID = "sb_id",
				Line = "sb_linenumberid",
				PhoneNumber = "sb_phonenumber",
				PrimaryId = "sb_callid",
				Status = "statecode",
				Task = "sb_taskid";

			public static string[] All => new[] { Birthday,
				CallType,
				Contact,
				CreatedOn,
				Direction,
				ID,
				Line,
				PhoneNumber,
				PrimaryId,
				Status,
				Task };
		}
		#endregion

		#region Enums
		public static class CallTypeEnum
		{
			public const int
				Успешный = 108550000,
				Связьпрервалась = 108550001,
				Недозвонились = 108550002;
		}

		public static class StatusEnum
		{
			public const int
				Open = 0,
				Сompleated = 1;
		}
		#endregion

		#region Field Definitions
		public DateTime? Birthday
		{
			get => (DateTime?)this[Fields.Birthday];
			set => this[Fields.Birthday] = value; 
		}
		public OptionSetValue CallType
		{
			get => (OptionSetValue)this[Fields.CallType];
			set => this[Fields.CallType] = value; 
		}
		public EntityReference Contact
		{
			get => (EntityReference)this[Fields.Contact];
			set => this[Fields.Contact] = value; 
		}
		public DateTime? CreatedOn
		{
			get => (DateTime?)this[Fields.CreatedOn];
			set => this[Fields.CreatedOn] = value; 
		}
		public bool? Direction
		{
			get => (bool?)this[Fields.Direction];
			set => this[Fields.Direction] = value; 
		}
		public string ID
		{
			get => (string)this[Fields.ID];
			set => this[Fields.ID] = value; 
		}
		public EntityReference Line
		{
			get => (EntityReference)this[Fields.Line];
			set => this[Fields.Line] = value; 
		}
		public string PhoneNumber
		{
			get => (string)this[Fields.PhoneNumber];
			set => this[Fields.PhoneNumber] = value; 
		}
		public new OptionSetValue Status
		{
			get => (OptionSetValue)this[Fields.Status];
			set => this[Fields.Status] = value; 
		}
		public EntityReference Task
		{
			get => (EntityReference)this[Fields.Task];
			set => this[Fields.Task] = value; 
		}
		#endregion

		#region Constructors
		protected CallModel()
			: base(LogicalName) { }
		protected CallModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected CallModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected CallModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected CallModel(Entity entity, IOrganizationService service)
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