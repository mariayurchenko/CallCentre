using Microsoft.Xrm.Sdk;
using System;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Dynamics;

namespace Contact.Messages
{
    public class PreValidation : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var adminService = factory.CreateOrganizationService(context.UserId);

            try
            {
                var settings = SBCustomSettings.GetSettings(adminService, SBCustomSettingsModel.Fields.Securityroleforchangedfield);

                if (settings.Securityroleforchangedfield == null)
                {
                    throw new Exception($"{SBCustomSettingsModel.Fields.Securityroleforchangedfield} is null");
                }

                var role = new SecurityRole(settings.Securityroleforchangedfield.Id, new ColumnSet(SecurityRoleModel.Fields.Name), service);

                if (!settings.UserHasRole(context.UserId, role.Name))
                {
                    throw new Exception($"The user does not have permission to change '{ContactModel.Fields.Email}' field");
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}