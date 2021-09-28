using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using SB.SharedModels.Actions;

namespace Tests.Dynamics.Actions
{
    [TestClass]
    public class ClosePhoneCallTests : DynamicsTestBase<SB.Actions.ActionTracking>
    {
        [TestMethod]
        public void SuccessfulClosingPhoneCall()
        {
            var executionContext = XrmRealContext.GetDefaultPluginContext();

            executionContext.MessageName = ActionNames.ActionTracking;

            var body = "380680540921";

            executionContext.InputParameters = new ParameterCollection
            {
                new KeyValuePair<string, object>("ActionName", ActionNames.ActionTrackingNames.ClosePhoneCall),
                new KeyValuePair<string, object>("Parameters", body)
            };

            XrmRealContext.ExecutePluginWith<SB.Actions.ActionTracking>(executionContext);
        }
    }
}