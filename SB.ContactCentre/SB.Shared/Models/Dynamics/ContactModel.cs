using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class ContactModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "contact";

		#region Attribute Names
		public static class Fields
		{
			public const string
				Birthday = "birthdate",
				Email = "emailaddress1",
				EmailAddress2 = "emailaddress2",
				EmailAddress3 = "emailaddress3",
				FirstName = "firstname",
				FullName = "fullname",
				Gender = "gendercode",
				Language = "sb_language",
				LastName = "lastname",
				MiddleName = "middlename",
				MobilePhone = "mobilephone",
				PrimaryId = "contactid";

			public static string[] All => new[] { Birthday,
				Email,
				EmailAddress2,
				EmailAddress3,
				FirstName,
				FullName,
				Gender,
				Language,
				LastName,
				MiddleName,
				MobilePhone,
				PrimaryId };
		}
		#endregion

		#region Enums
		public static class GenderEnum
		{
			public const int
				Male = 108550001,
				Female = 108550000;
		}
		#endregion

		#region Field Definitions
		public DateTime? Birthday
		{
			get => (DateTime?)this[Fields.Birthday];
			set => this[Fields.Birthday] = value; 
		}
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
		public string FirstName
		{
			get => (string)this[Fields.FirstName];
			set => this[Fields.FirstName] = value; 
		}
		public string FullName
		{
			get => (string)this[Fields.FullName];
			set => this[Fields.FullName] = value; 
		}
		public OptionSetValue Gender
		{
			get => (OptionSetValue)this[Fields.Gender];
			set => this[Fields.Gender] = value; 
		}
		public int? Language
		{
			get => (int?)this[Fields.Language];
			set => this[Fields.Language] = value; 
		}
		public string LastName
		{
			get => (string)this[Fields.LastName];
			set => this[Fields.LastName] = value; 
		}
		public string MiddleName
		{
			get => (string)this[Fields.MiddleName];
			set => this[Fields.MiddleName] = value; 
		}
		public string MobilePhone
		{
			get => (string)this[Fields.MobilePhone];
			set => this[Fields.MobilePhone] = value; 
		}
		#endregion

		#region Constructors
		protected ContactModel()
			: base(LogicalName) { }
		protected ContactModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected ContactModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected ContactModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected ContactModel(Entity entity, IOrganizationService service)
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