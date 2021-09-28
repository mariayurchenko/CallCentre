using System;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;

namespace SB.Shared.EntityProviders
{
    public class SBCustomSettings : SBCustomSettingsModel
    {
        public static readonly string[] MeetingSettingsFields = {Fields.Account, Fields.Password, Fields.ClientId, Fields.ClientSecret, Fields.Tenant};

        public SBCustomSettings(IOrganizationService service) : base(service) { }
        public SBCustomSettings(IOrganizationService service, Guid id) : base(id, service) { }
        public SBCustomSettings(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public SBCustomSettings(Entity entity, IOrganizationService service) : base(entity, service) { }

        public static SBCustomSettings GetSettings(IOrganizationService service, params string[] columns)
        {
            var query = new QueryExpression(LogicalName)
            {
                ColumnSet = new ColumnSet(columns)
            };
            var settings = service.RetrieveMultiple(query).Entities.Select(entity => new SBCustomSettings(entity, service)).FirstOrDefault();

            if (settings == null)
            {
                throw new InvalidOperationException("SB Custom settings not found. Please configure system or contact the system administrator for support.");
            }

            return settings;
        }

        public bool UserHasRole(Guid userId, string roleName)
        {
            var query = new QueryExpression(SystemUserRolesModel.LogicalName);
            query.Criteria.AddCondition(SystemUserRolesModel.Fields.SystemUser, ConditionOperator.Equal, userId);
            var link = query.AddLink(SecurityRoleModel.LogicalName, SystemUserRolesModel.Fields.Role, SecurityRoleModel.Fields.PrimaryId, JoinOperator.Inner);
            link.LinkCriteria.AddCondition(SecurityRoleModel.Fields.Name, ConditionOperator.Equal, roleName);

            var result = _service.RetrieveMultiple(query).Entities.FirstOrDefault();

            return result != null;
        }

        private SBCustomSettings GetDirectionSettings(string directionName)
        {
            var query = new QueryExpression(LogicalName)
            {
                ColumnSet = new ColumnSet(Fields.All.Where(s => s.StartsWith(directionName)).ToArray())
            };

            var settings = _service.RetrieveMultiple(query).Entities.FirstOrDefault();

            return settings == null ? null : new SBCustomSettings(settings, _service);
        }

        public static string GetBasicHeader(string userName, string userPassword)
        {
            return "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + userPassword));
        }
    }
}
