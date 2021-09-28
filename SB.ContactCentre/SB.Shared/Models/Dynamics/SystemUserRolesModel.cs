using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
    public class SystemUserRolesModel : EntityBase
    {
        public const string
            LogicalName = "systemuserroles";

        #region Attribute Names
        public static class Fields
        {
            public const string
                Role = "roleid",
                PrimaryId = "systemuserrolesid",
                SystemUser = "systemuserid";

            public static string[] All => new[] {
                Role,
                PrimaryId,
                SystemUser };
        }
        #endregion

        #region Enums

        #endregion

        #region Field Definitions
        public EntityReference Role
        {
            get => (EntityReference)this[Fields.Role];
            set => this[Fields.Role] = value;
        }
        public EntityReference PrimaryId
        {
            get => (EntityReference)this[Fields.PrimaryId];
            set => this[Fields.PrimaryId] = value;
        }

        #endregion

        #region Constructors
        protected SystemUserRolesModel()
            : base(LogicalName) { }
        protected SystemUserRolesModel(IOrganizationService service)
            : base(LogicalName, service) { }
        protected SystemUserRolesModel(Guid id, ColumnSet columnSet, IOrganizationService service)
            : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        protected SystemUserRolesModel(Guid id, IOrganizationService service)
            : base(LogicalName, id, service) { }
        protected SystemUserRolesModel(Entity entity, IOrganizationService service)
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