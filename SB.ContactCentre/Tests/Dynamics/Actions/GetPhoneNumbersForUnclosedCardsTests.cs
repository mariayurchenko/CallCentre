using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using SB.SharedModels.Actions;

namespace Tests.Dynamics.Actions
{
    [TestClass]
    public class GetPhoneNumbersForUnclosedCardsTests : DynamicsTestBase<SB.Actions.ActionTracking>
    {
        [TestMethod]
        public void Successful()
        {
            var executionContext = XrmRealContext.GetDefaultPluginContext();

            executionContext.MessageName = ActionNames.ActionTracking;

            executionContext.InputParameters = new ParameterCollection
            {
                new KeyValuePair<string, object>("ActionName", ActionNames.ActionTrackingNames.GetPhoneNumbersForUnclosedCards)
            };

            XrmRealContext.ExecutePluginWith<SB.Actions.ActionTracking>(executionContext);
        }
    }
}