using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class TaskModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_task";

		#region Attribute Names
		public static class Fields
		{
			public const string
				Callbackdate = "sb_callbackdate",
				Contact = "sb_contactid",
				PhoneCall = "sb_phonecallid",
				PhoneNumber = "sb_phonenumber",
				PrimaryId = "sb_taskid",
				PrimaryName = "sb_name",
				TaskType = "sb_tasktype";

			public static string[] All => new[] { Callbackdate,
				Contact,
				PhoneCall,
				PhoneNumber,
				PrimaryId,
				PrimaryName,
				TaskType };
		}
		#endregion

		#region Enums
		public static class TaskTypeEnum
		{
			public const int
				Перезвонить = 108550000,
				Закрыть = 108550001;
		}
		#endregion

		#region Field Definitions
		public DateTime? Callbackdate
		{
			get => (DateTime?)this[Fields.Callbackdate];
			set => this[Fields.Callbackdate] = value; 
		}
		public EntityReference Contact
		{
			get => (EntityReference)this[Fields.Contact];
			set => this[Fields.Contact] = value; 
		}
		public EntityReference PhoneCall
		{
			get => (EntityReference)this[Fields.PhoneCall];
			set => this[Fields.PhoneCall] = value; 
		}
		public string PhoneNumber
		{
			get => (string)this[Fields.PhoneNumber];
			set => this[Fields.PhoneNumber] = value; 
		}
		public OptionSetValue TaskType
		{
			get => (OptionSetValue)this[Fields.TaskType];
			set => this[Fields.TaskType] = value; 
		}
		#endregion

		#region Constructors
		protected TaskModel()
			: base(LogicalName) { }
		protected TaskModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected TaskModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected TaskModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected TaskModel(Entity entity, IOrganizationService service)
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