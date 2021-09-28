using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using SB.Shared;
using SB.SharedModels.Actions;
using SB.SharedModels.PhoneCall;

namespace Tests.Dynamics.Actions
{
    [TestClass]
    public class CreatePhoneCallTests : DynamicsTestBase<SB.Actions.ActionTracking>
    {
        [TestMethod]
        public void SuccessfulCreatingPhoneCall()
        {
            var executionContext = XrmRealContext.GetDefaultPluginContext();

            executionContext.MessageName = ActionNames.ActionTracking;

            var phoneCall = new CreatePhoneCallRequest
            {
                CallId = "2188689",
                ClientNumber = "380680540921",
                LineNumber = "555",
                Direction = false
            };

            var body = JsonSerializer.Serialize(phoneCall);

            executionContext.InputParameters = new ParameterCollection
            {
                new KeyValuePair<string, object>("ActionName", ActionNames.ActionTrackingNames.CreatePhoneCall),
                new KeyValuePair<string, object>("Parameters", body)
            };

            XrmRealContext.ExecutePluginWith<SB.Actions.ActionTracking>(executionContext);
        }
    }
}