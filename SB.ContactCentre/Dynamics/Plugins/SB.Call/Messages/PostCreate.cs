using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Dynamics;

namespace SB.Call.Messages
{
    public class PostCreate : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                var target = (Entity)context.InputParameters["Target"];

                var call = new Shared.EntityProviders.Call(target, service);

                if (call.Line != null)
                {
                    var line = new CallCenterLine(call.Line.Id, new ColumnSet(CallCenterLineModel.Fields.Phonecallsfortheday), service);

                    line.IncreasePhoneCallsForTheDay();
                    line.RemoveUnchangedAttributes();
                    line.Save();
                }

                if (call.Status.Value == CallModel.StatusEnum.Сompleated && call.PhoneNumber != null)
                {
                    call.CompletePhoneCallsByPhoneNumber();
                }

            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}