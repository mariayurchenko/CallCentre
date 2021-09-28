using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.EntityProviders;
using SB.Shared.Extensions;
using SB.Shared.Helpers;
using SB.Shared.Models.Actions;
using SB.Shared.Models.Dynamics;

namespace SB.Actions.Messages
{
    public class CreateTask : IActionTracking
    {
        private readonly IOrganizationService _service;

        public CreateTask(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, ref ActionResponse actionResponse)
        {
            ParameterCheckHelper.ValidateStringParameter(parameters, nameof(parameters));

            if (Guid.TryParse(parameters, out var id))
            {
                if (id == Guid.Empty)
                {
                    throw new Exception($"{nameof(id)} is empty Guid");
                }

                var call = _service.Retrieve(CallModel.LogicalName, id, new ColumnSet(CallModel.Fields.Contact, CallModel.Fields.PhoneNumber)).ToEntity<Call>(_service);

                var task = new Task(_service)
                {
                    Contact = call.Contact,
                    PhoneCall = call.GetReference(),
                    PhoneNumber = call.PhoneNumber,
                    TaskType = new OptionSetValue(TaskModel.TaskTypeEnum.Перезвонить)
                };

                task.Save();

                call.Task = task.GetReference();
                call.CallType = new OptionSetValue(CallModel.CallTypeEnum.Успешный);

                call.Save();
            }
            else
            {
                throw new Exception($"{nameof(parameters)} not parse to Guid");
            }
        }
    }
}