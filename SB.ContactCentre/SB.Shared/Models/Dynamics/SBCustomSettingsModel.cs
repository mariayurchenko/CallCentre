using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class SBCustomSettingsModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_sbcustomsettings";

		#region Attribute Names
		public static class Fields
		{
			public const string
				Account = "sb_account",
				AppId = "sb_appid",
				Callbacktime = "sb_callbacktime",
				ClientId = "sb_clientid",
				ClientSecret = "sb_clientsecret",
				D365URL = "sb_d365url",
				DefaultPhoneCallForm = "sb_defaultphonecallform",
				Password = "sb_password",
				PhoneCallsClosing = "sb_phonecallsclosing",
				PrimaryId = "sb_sbcustomsettingsid",
				PrimaryName = "sb_name",
				Securityroleforchangedfield = "sb_securityrole",
				Tenant = "sb_tenant",
				TokenLifetime = "sb_tokenlifetime",
				UserName = "sb_username",
				UserPassword = "sb_userpassword";

			public static string[] All => new[] { Account,
				AppId,
				Callbacktime,
				ClientId,
				ClientSecret,
				D365URL,
				DefaultPhoneCallForm,
				Password,
				PhoneCallsClosing,
				PrimaryId,
				PrimaryName,
				Securityroleforchangedfield,
				Tenant,
				TokenLifetime,
				UserName,
				UserPassword };
		}
		#endregion

		#region Enums
		public static class DefaultPhoneCallFormEnum
		{
			public const int
				Service = 108550000,
				Sale = 108550001;
		}
		#endregion

		#region Field Definitions
		public string Account
		{
			get => (string)this[Fields.Account];
			set => this[Fields.Account] = value; 
		}
		public string AppId
		{
			get => (string)this[Fields.AppId];
			set => this[Fields.AppId] = value; 
		}
		public int? Callbacktime
		{
			get => (int?)this[Fields.Callbacktime];
			set => this[Fields.Callbacktime] = value; 
		}
		public string ClientId
		{
			get => (string)this[Fields.ClientId];
			set => this[Fields.ClientId] = value; 
		}
		public string ClientSecret
		{
			get => (string)this[Fields.ClientSecret];
			set => this[Fields.ClientSecret] = value; 
		}
		public string D365URL
		{
			get => (string)this[Fields.D365URL];
			set => this[Fields.D365URL] = value; 
		}
		public OptionSetValue DefaultPhoneCallForm
		{
			get => (OptionSetValue)this[Fields.DefaultPhoneCallForm];
			set => this[Fields.DefaultPhoneCallForm] = value; 
		}
		public string Password
		{
			get => (string)this[Fields.Password];
			set => this[Fields.Password] = value; 
		}
		public int? PhoneCallsClosing
		{
			get => (int?)this[Fields.PhoneCallsClosing];
			set => this[Fields.PhoneCallsClosing] = value; 
		}
		public EntityReference Securityroleforchangedfield
		{
			get => (EntityReference)this[Fields.Securityroleforchangedfield];
			set => this[Fields.Securityroleforchangedfield] = value; 
		}
		public string Tenant
		{
			get => (string)this[Fields.Tenant];
			set => this[Fields.Tenant] = value; 
		}
		public int? TokenLifetime
		{
			get => (int?)this[Fields.TokenLifetime];
			set => this[Fields.TokenLifetime] = value; 
		}
		public string UserName
		{
			get => (string)this[Fields.UserName];
			set => this[Fields.UserName] = value; 
		}
		public string UserPassword
		{
			get => (string)this[Fields.UserPassword];
			set => this[Fields.UserPassword] = value; 
		}
		#endregion

		#region Constructors
		protected SBCustomSettingsModel()
			: base(LogicalName) { }
		protected SBCustomSettingsModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected SBCustomSettingsModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected SBCustomSettingsModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected SBCustomSettingsModel(Entity entity, IOrganizationService service)
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