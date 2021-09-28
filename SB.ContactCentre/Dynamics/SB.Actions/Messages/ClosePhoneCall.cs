using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared;
using SB.Shared.EntityProviders;
using SB.Shared.Helpers;
using SB.SharedModels.Helpers;
using SB.Shared.Models.Actions;
using SB.Shared.Models.Dynamics;
using SB.SharedModels.PhoneCall;

namespace SB.Actions.Messages
{
    public class ClosePhoneCall: IActionTracking
    {
        private readonly IOrganizationService _service;

        public ClosePhoneCall(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, ref ActionResponse actionResponse)
        {
            var response = new ClosePhoneCallResponse();

            try
            {
                ParameterCheckHelper.ValidateStringParameter(parameters, nameof(parameters));

                var phoneNumber = VariableCheck.ValidatePhoneNumber(parameters);

                var phoneCall = Call.FindOpenCallByPhoneNumberOrReturnNull(_service, phoneNumber, CallModel.Fields.Line, CallModel.Fields.PhoneNumber);

                if (phoneCall == null)
                {
                    throw new Exception($"Open Call Phone by Phone Number: {phoneNumber} not found");
                }

                phoneCall.Status = new OptionSetValue(CallModel.StatusEnum.Сompleated);

                phoneCall.Save();

                response.Result = new ClosePhoneCallResultModel
                {
                    Number = phoneCall.PhoneNumber
                };

                if (phoneCall.Line != null)
                {
                    var line = new CallCenterLine(phoneCall.Line.Id, new ColumnSet(CallCenterLineModel.Fields.LineNumber), _service);

                    if (line.LineNumber != null)
                    {
                        response.Result.CallCenterLine = line.LineNumber;
                    }
                }

                response.IsSucceeded = true;

            }
            catch(Exception e)
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