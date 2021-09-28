/*using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using SB.Shared;
using SB.SharedModels.Actions;
using SB.Task.Messages;

namespace Tests.Dynamics.Plugins.SB.Task
{
    [TestClass]
    class PostCreateTests : DynamicsTestBase<PostCreate>
    {
        [TestMethod]
        public void Execute()
        {
            var executionContext = XrmRealContext.GetDefaultPluginContext();

            executionContext.MessageName = ActionNames.ActionTracking;

            var phoneCall = new Entity
            {
                LogicalName = "",
                ["CallId"] = "2188689"
            };

            var body = JsonSerializer.Serialize(phoneCall);

            executionContext.InputParameters = new ParameterCollection
            {
                new KeyValuePair<string, object>("ActionName", ActionNames.ActionTrackingNames.CreatePhoneCall),
                new KeyValuePair<string, object>("Parameters", body)
            };

            XrmRealContext.ExecutePluginWith<PostCreate>(executionContext);
        }
    }
}
*/