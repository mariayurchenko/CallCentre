using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared;
using SB.Shared.EntityProviders;
using SB.Shared.Helpers;
using SB.Shared.Models.Actions;
using SB.Shared.Models.Dynamics;
using SB.SharedModels.PhoneCall;

namespace SB.Actions.Messages
{
    public class CreatePhoneCall: IActionTracking
    {
        private readonly IOrganizationService _service;

        public CreatePhoneCall(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, ref ActionResponse actionResponse)
        {
            var response = new CreatePhoneCallResponse();

            try
            {
                ParameterCheckHelper.ValidateStringParameter(parameters, nameof(parameters));

                var request = JsonSerializer.Deserialize<CreatePhoneCallRequest>(parameters);

                ParameterCheckHelper.ValidateStringParameter(request.CallId, nameof(request.CallId));
                ParameterCheckHelper.ValidateStringParameter(request.ClientNumber, nameof(request.ClientNumber));
                ParameterCheckHelper.ValidateStringParameter(request.LineNumber, nameof(request.LineNumber));

                if (!request.Direction.HasValue)
                {
                    throw new Exception($"{nameof(request.Direction)} is null");
                }

                var line = CallCenterLine.FindLineByNumberLineOrReturnNull(_service, request.LineNumber);

                if (line == null)
                {
                    throw new Exception("Line with this Line Number not found");
                }

                var phoneCall = new Call(_service)
                {
                    ID = request.CallId,
                    PhoneNumber = request.ClientNumber,
                    Direction = request.Direction.Value,
                    Line = line.GetReference()
                };

                phoneCall.Save();

                if (phoneCall.Id == null || phoneCall.Id == Guid.Empty)
                {
                    throw new Exception("Phone Call was not created");
                }

                phoneCall = new Call(phoneCall.Id.Value, new ColumnSet(CallModel.Fields.Contact, CallModel.Fields.Birthday, CallModel.Fields.Line), _service);

                if (phoneCall.Contact == null || phoneCall.Contact.Id == Guid.Empty)
                {
                    throw new Exception($"{nameof(phoneCall.Contact)} is null or empty Guid");
                }

                var contact = new Contact(phoneCall.Contact.Id, new ColumnSet(ContactModel.Fields.Language, ContactModel.Fields.FullName), _service);

                var settings = SBCustomSettings.GetSettings(_service, SBCustomSettingsModel.Fields.D365URL, SBCustomSettingsModel.Fields.AppId);

                var url = phoneCall.GenerateCrmRecordUrl(settings.D365URL, settings.AppId);

                var phoneCallModel = new PhoneCallModel
                {
                    FullName = contact.FullName,
                    Language = contact.Language,
                    DateOfBirth = phoneCall.Birthday,
                    PhoneCallUrl = url
                };

                if (phoneCall.Id == null || phoneCall.Contact.Id == Guid.Empty)
                {
                    throw new Exception($"{nameof(phoneCall.Contact)} is null or empty Guid");
                }

                response.Id = phoneCall.Id.Value;
                response.IsSucceeded = true;
                response.Result = new CreatePhoneCallResultModel {AdditionalInfo = phoneCallModel};
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