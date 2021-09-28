using System;
using Microsoft.Xrm.Sdk;
using SB.Shared.Extensions;
using SB.Shared.Models.Dynamics;

namespace SB.Call.Messages
{
    public class PostUpdate : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                if (context.Depth >= 3) return;

                var target = (Entity) context.InputParameters["Target"];
                var postImage = context.PostEntityImages["PostImage"];
                var entity = postImage.Merge(target);
                var call = new Shared.EntityProviders.Call(entity, service);

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