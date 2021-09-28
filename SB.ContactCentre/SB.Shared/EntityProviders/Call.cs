using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Extensions;
using SB.Shared.Helpers;
using SB.Shared.Models.Actions;
using SB.Shared.Models.Dynamics;
//using SB.SharedModels.Actions;

namespace SB.Shared.EntityProviders
{
    public class Call : CallModel
    {
        public Call(IOrganizationService service) : base(service) { }
        public Call(IOrganizationService service, Guid id) : base(id, service) { }
        public Call(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Call(Entity entity, IOrganizationService service) : base(entity, service) { }

        public static void CloseOldPhoneCalls(IOrganizationService service)
        {
            var settings = SBCustomSettings.GetSettings(service, SBCustomSettingsModel.Fields.PhoneCallsClosing);

            if (settings.PhoneCallsClosing == null)
            {
                throw new Exception($"{nameof(settings.PhoneCallsClosing)} is null");
            }

            var req = new OrganizationRequest("sb_ActionTracking")//(ActionNames.ActionTracking)
            {
                ["ActionName"] = "CurrentDateTime"//ActionNames.ActionTrackingNames.CurrentDateTime
            };


            var response = service.Execute(req);

            var actionResponse =
                JsonSerializer.Deserialize<ActionResponse>(response.Results["Response"].ToString());

            if (actionResponse.Value == null)
            {
                throw new ArgumentNullException($"{nameof(actionResponse.Value)} is null");
            }

            var dateNow = Convert.ToDateTime(actionResponse.Value);

            var date = dateNow - (dateNow.AddMinutes(settings.PhoneCallsClosing.Value) - dateNow);

            var query = new QueryExpression(LogicalName);

            query.ColumnSet.AddColumn(Fields.PhoneNumber);
            query.Criteria.AddCondition(Fields.Status, ConditionOperator.Equal, StatusEnum.Open);
            query.Criteria.AddCondition(Fields.PhoneNumber, ConditionOperator.NotNull);
            query.Criteria.AddCondition(Fields.CreatedOn, ConditionOperator.LessEqual, date);

            var phoneCalls = service.RetrieveMultiple(query).ToEntityCollection<Call>(service);

            if (phoneCalls.Count == 0)
            {
                throw new Exception("Old Phone Calls not found");
            }

            for (var i = 0; i < phoneCalls.Count; i++)
            {
                for (var j = i+1; j < phoneCalls.Count;)
                {
                    if (phoneCalls[i].PhoneNumber == phoneCalls[j].PhoneNumber)
                    {
                        phoneCalls.Remove(phoneCalls[j]);
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            foreach (var phoneCall in phoneCalls)
            {
                phoneCall.Status = new OptionSetValue(StatusEnum.Сompleated);

                phoneCall.RemoveUnchangedAttributes();
                phoneCall.Save();
            }

        }

        public static string[] GetOpenPhoneCallsPhoneNumberOrReturnNull(IOrganizationService service)
        {
            var phoneNumbersList = new List<string>();

            var query = new QueryExpression(LogicalName);

            query.ColumnSet.AddColumn(Fields.PhoneNumber);
            query.Criteria.AddCondition(Fields.PhoneNumber, ConditionOperator.NotNull);
            query.Criteria.AddCondition(Fields.Status, ConditionOperator.Equal, StatusEnum.Open);

            var phoneCalls = service.RetrieveMultiple(query).ToEntityCollection<Call>(service);

            foreach (var phoneCall in phoneCalls.Where(phoneCall => !phoneNumbersList.Contains(phoneCall.PhoneNumber)))
            {
                phoneNumbersList.Add(phoneCall.PhoneNumber);
            }

            return phoneNumbersList.ToArray();
        }

        public static Call FindOpenCallByPhoneNumberOrReturnNull(IOrganizationService service, string phoneNumber, params string[] columns)
        {
            var query = new QueryExpression(LogicalName);

            query.ColumnSet.AddColumns(columns);
            query.Criteria.AddCondition(Fields.PhoneNumber, ConditionOperator.Equal, phoneNumber);
            query.Criteria.AddCondition(Fields.Status, ConditionOperator.Equal, StatusEnum.Open);

            var call = service.RetrieveMultiple(query).Entities.Select(entity => new Call(entity, service)).FirstOrDefault();

            return call;
        }

        public void CompletePhoneCallsByPhoneNumber()
        {
            if (PhoneNumber == null)
            {
                throw new ArgumentNullException($"{nameof(PhoneNumber)} is null");
            }

            var query = new QueryExpression(LogicalName);

            query.ColumnSet.AddColumn(Fields.Status);
            query.Criteria.AddCondition(Fields.PhoneNumber, ConditionOperator.Equal, PhoneNumber);
            query.Criteria.AddCondition(Fields.Status, ConditionOperator.Equal, StatusEnum.Open);

            var phoneCalls = _service.RetrieveMultiple(query).ToEntityCollection<Call>(_service);

            foreach (var phoneCall in phoneCalls)
            {
                phoneCall.Status = new OptionSetValue(StatusEnum.Сompleated);

                phoneCall.Save();
            }
        }

        public string GenerateCrmRecordUrl(string dynamicsUrl, string appId)
        {
            ParameterCheckHelper.ValidateStringParameter(appId, nameof(appId));
            ParameterCheckHelper.ValidateStringParameter(dynamicsUrl, nameof(dynamicsUrl));

            if (Id == null)
            {
                throw new ArgumentNullException($"{nameof(Id)} is null");
            }

            dynamicsUrl = dynamicsUrl.EndsWith("/") ? dynamicsUrl : dynamicsUrl + "/";

            if (Line == null)
                return $"{dynamicsUrl}main.aspx?appid={appId}&pagetype=entityrecord&etn={LogicalName}&id={Id.Value}";

            string parameters = null;

            var line = new CallCenterLine(Line.Id, new ColumnSet(CallCenterLineModel.Fields.CallCentreLineName), _service);

            switch (line.CallCentreLineName?.Value)
            {
                case CallCenterLineModel.CallCentreLineNameEnum.Service:
                    parameters = "formid=4518f06d-45b1-45a8-a8fe-81b9d6412712";
                    break;
                case CallCenterLineModel.CallCentreLineNameEnum.Sale:
                    parameters = "formid=b95006ae-b7e3-eb11-bacb-0022489baed1";
                    break;
                default:
                    var settings = SBCustomSettings.GetSettings(_service, SBCustomSettingsModel.Fields.DefaultPhoneCallForm);
                    
                    if (settings.DefaultPhoneCallForm == null)
                    {
                        throw new ArgumentNullException($"{nameof(settings.DefaultPhoneCallForm)} is null");
                    }

                    switch (settings.DefaultPhoneCallForm.Value)
                    {
                        case SBCustomSettingsModel.DefaultPhoneCallFormEnum.Service:
                            parameters = "formid=4518f06d-45b1-45a8-a8fe-81b9d6412712";
                            break;
                        case SBCustomSettingsModel.DefaultPhoneCallFormEnum.Sale:
                            parameters = "formid=b95006ae-b7e3-eb11-bacb-0022489baed1";
                            break;
                    }
                    break;
            }

            return $"{dynamicsUrl}main.aspx?appid={appId}&pagetype=entityrecord&etn={LogicalName}&id={Id.Value}&{parameters}";
        }

    }
}