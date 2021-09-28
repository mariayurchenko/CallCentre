using Microsoft.Xrm.Sdk;
using SB.Shared;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Actions;

namespace SB.Actions.Messages
{
    public class GetPhoneNumbersForUnclosedCards : IActionTracking
    {
        private readonly IOrganizationService _service;

        public GetPhoneNumbersForUnclosedCards(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, ref ActionResponse actionResponse)
        {
            var phoneNumbers = Call.GetOpenPhoneCallsPhoneNumberOrReturnNull(_service);

            actionResponse.Value = phoneNumbers == null ? "No phones for unclosed cards" : JsonSerializer.Serialize(phoneNumbers);
        }
    }
}