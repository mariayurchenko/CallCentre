using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class CallCenterLineModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_callcenterline";

		#region Attribute Names
		public static class Fields
		{
			public const string
				CallCentreLineName = "sb_line",
				LineNumber = "sb_linenumber",
				Phonecallsfortheday = "sb_phonecallsfortheday",
				PrimaryId = "sb_callcenterlineid";

			public static string[] All => new[] { CallCentreLineName,
				LineNumber,
				Phonecallsfortheday,
				PrimaryId };
		}
		#endregion

		#region Enums
		public static class CallCentreLineNameEnum
		{
			public const int
				Service = 108550000,
				Sale = 108550001;
		}
		#endregion

		#region Field Definitions
		public OptionSetValue CallCentreLineName
		{
			get => (OptionSetValue)this[Fields.CallCentreLineName];
			set => this[Fields.CallCentreLineName] = value; 
		}
		public string LineNumber
		{
			get => (string)this[Fields.LineNumber];
			set => this[Fields.LineNumber] = value; 
		}
		public int? Phonecallsfortheday
		{
			get => (int?)this[Fields.Phonecallsfortheday];
			set => this[Fields.Phonecallsfortheday] = value; 
		}
		#endregion

		#region Constructors
		protected CallCenterLineModel()
			: base(LogicalName) { }
		protected CallCenterLineModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected CallCenterLineModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected CallCenterLineModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected CallCenterLineModel(Entity entity, IOrganizationService service)
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