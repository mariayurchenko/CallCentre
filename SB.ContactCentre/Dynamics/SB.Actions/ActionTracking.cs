using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using SB.Shared;
using SB.Shared.Models.Actions;
using SB.Actions.Messages;
using SB.SharedModels.Actions;

namespace SB.Actions
{
    public class ActionTracking : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            var factory = serviceProvider.Get<IOrganizationServiceFactory>();
            //var notificationService = serviceProvider.Get<IServiceEndpointNotificationService>();
            var service = factory.CreateOrganizationService(context.UserId);
            //var adminService = factory.CreateOrganizationService(null);
            var tracer = serviceProvider.Get<ITracingService>();

            try
            {
                var actionName = (string)context.InputParameters["ActionName"];
                var parameters = context.InputParameters.Contains("Parameters") ? (string)context.InputParameters["Parameters"] : string.Empty;

                var response = new ActionResponse();

                foreach (var parameter in context.InputParameters)
                {
                    tracer?.Trace($"{parameter.Key} = {parameter.Value}");
                }

                switch (actionName)
                {
                    case ActionNames.ActionTrackingNames.CreatePhoneCall:
                        new CreatePhoneCall(service).Execute(parameters, ref response);
                        break;
                    case ActionNames.ActionTrackingNames.ResettingCallCounterOnLine:
                        new ResettingCallCounterOnLine(service).Execute(parameters, ref response);
                        break;
                    case ActionNames.ActionTrackingNames.CurrentDateTime:
                        new CurrentDateTime().Execute(parameters, ref response);
                        break;
                    case ActionNames.ActionTrackingNames.CreateTask:
                        new CreateTask(service).Execute(parameters, ref response);
                        break;
                    case ActionNames.ActionTrackingNames.GetPhoneNumbersForUnclosedCards:
                        new GetPhoneNumbersForUnclosedCards(service).Execute(parameters, ref response);
                        break;
                    case ActionNames.ActionTrackingNames.ClosePhoneCall:
                        new ClosePhoneCall(service).Execute(parameters, ref response);
                        break;
                    case ActionNames.ActionTrackingNames.CloseOldPhoneCalls:
                        new CloseOldPhoneCalls(service).Execute(parameters, ref response);
                        break;

                    default:
                        response.Status = Status.Error;
                        response.Value = $"Acton message with name {actionName} wasn't found in {nameof(ActionTracking)}";
                        break;
                }

                context.OutputParameters["Response"] = JsonSerializer.Serialize(response);

                tracer?.Trace($"{nameof(context.OutputParameters)}:");
                foreach (var parameter in context.OutputParameters)
                {
                    tracer?.Trace($"{parameter.Key} = {parameter.Value}");
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}