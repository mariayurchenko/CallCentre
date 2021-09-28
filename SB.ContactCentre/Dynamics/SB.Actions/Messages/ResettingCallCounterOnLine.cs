using Microsoft.Xrm.Sdk;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Actions;

namespace SB.Actions.Messages
{
    public class ResettingCallCounterOnLine: IActionTracking
    {
        private readonly IOrganizationService _service;

        public ResettingCallCounterOnLine(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, ref ActionResponse actionResponse)
        {
            CallCenterLine.ResettingCallCounterOnLine(_service);
        }
    }
}