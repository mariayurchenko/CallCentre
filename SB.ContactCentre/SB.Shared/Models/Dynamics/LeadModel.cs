using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class LeadModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "lead";

		#region Attribute Names
		public static class Fields
		{
			public const string
				Email = "emailaddress1",
				EmailAddress2 = "emailaddress2",
				EmailAddress3 = "emailaddress3",
				PrimaryId = "leadid",
				PrimaryName = "fullname";

			public static string[] All => new[] { Email,
				EmailAddress2,
				EmailAddress3,
				PrimaryId,
				PrimaryName };
		}
		#endregion

		#region Enums
		
		#endregion

		#region Field Definitions
		public string Email
		{
			get => (string)this[Fields.Email];
			set => this[Fields.Email] = value; 
		}
		public string EmailAddress2
		{
			get => (string)this[Fields.EmailAddress2];
			set => this[Fields.EmailAddress2] = value; 
		}
		public string EmailAddress3
		{
			get => (string)this[Fields.EmailAddress3];
			set => this[Fields.EmailAddress3] = value; 
		}
		#endregion

		#region Constructors
		protected LeadModel()
			: base(LogicalName) { }
		protected LeadModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected LeadModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected LeadModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected LeadModel(Entity entity, IOrganizationService service)
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