using System;
using Microsoft.Xrm.Sdk;
using SB.Shared;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Actions;
using SB.SharedModels.PhoneCall;

namespace SB.Actions.Messages
{
    public class CloseOldPhoneCalls: IActionTracking
    {
        private readonly IOrganizationService _service;

        public CloseOldPhoneCalls(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, ref ActionResponse actionResponse)
        {
            var response = new CloseOldPhoneCallsResponse();

            try
            {
                Call.CloseOldPhoneCalls(_service);

                response.IsSucceeded = true;
            }
            catch (Exception e)
            {
                response.IsSucceeded = false;
                response.ValidationErrors = e.Message;
            }
            finally
            {
                var responseJson = JsonSerializer.Serialize(response);

                actionResponse.Value = responseJson;
            }
        }
    }
}