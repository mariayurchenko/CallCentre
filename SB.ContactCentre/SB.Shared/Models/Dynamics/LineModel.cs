using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class LineModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_line";

		#region Attribute Names
		public static class Fields
		{
			public const string
				Line = "sb_line",
				Phonecallsfortheday = "sb_phonecallsfortheday",
				PhoneNumber = "sb_phonenumber",
				PrimaryId = "sb_lineid",
				PrimaryName = "sb_name";

			public static string[] All => new[] { Line,
				Phonecallsfortheday,
				PhoneNumber,
				PrimaryId,
				PrimaryName };
		}
		#endregion

		#region Enums
		
		#endregion

		#region Field Definitions
		public OptionSetValue Line
		{
			get => (OptionSetValue)this[Fields.Line];
			set => this[Fields.Line] = value; 
		}
		public int? Phonecallsfortheday
		{
			get => (int?)this[Fields.Phonecallsfortheday];
			set => this[Fields.Phonecallsfortheday] = value; 
		}
		public string PhoneNumber
		{
			get => (string)this[Fields.PhoneNumber];
			set => this[Fields.PhoneNumber] = value; 
		}
		#endregion

		#region Constructors
		protected LineModel()
			: base(LogicalName) { }
		protected LineModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected LineModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected LineModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected LineModel(Entity entity, IOrganizationService service)
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