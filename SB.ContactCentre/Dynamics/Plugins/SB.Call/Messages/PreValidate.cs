using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;
using SB.SharedModels.Helpers;
using SB.Shared.Helpers;

namespace SB.Call.Messages
{
    public class PreValidate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                var target = (Entity) context.InputParameters["Target"];

                var phoneNumber = target.GetAttributeValue<string>(CallModel.Fields.PhoneNumber);

                if (phoneNumber != null)
                {
                    target[CallModel.Fields.PhoneNumber] = VariableCheck.ValidatePhoneNumber(phoneNumber);
                }

            }
            catch (Exception e) when (e.Message.Contains("Wrong phone number format"))
            {
                var userSettings = service.Retrieve("usersettings", context.UserId, new ColumnSet("uilanguageid"));

                var resourceName = $"sb_callcentreLocalization.{userSettings.GetAttributeValue<int>("uilanguageid")}.resx";

                var webResourceManager = new WebResourceManager(resourceName, service);

                var message = webResourceManager.GetStringValue("PhoneNumberError");

                if (string.IsNullOrWhiteSpace(message))
                {
                    message = e.Message;
                }

                throw new InvalidPluginExecutionException(message);
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}