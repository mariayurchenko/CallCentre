var page = (function (window, document) {
    
	/************************************************************************************
    * Variables
    ************************************************************************************/
    var formContext;

    var layout = {
        Fields: {
            Contact: "sb_contactid",
            CallType: "sb_calltype",
            PhoneNumber: "sb_phonenumber",
            Task: "sb_taskid",
            Line: "sb_linenumberid"
        },
        FormName: {
            Service: "Service",
            Sale: "Sale"
        },
        ActionNames: {
            ActionTracking: "sb_ActionTracking",
            CreateTask: "CreateTask"
        },
        DefaultForm: "",
        SBCustomSettings: {
            LogicalName: "sb_sbcustomsettings",
            Fields: {
                DefaultPhoneCallForm: "sb_defaultphonecallform"
            },
            DefaultPhoneCallFormEnum: {
                Service: 108550000,
                Sale: 108550001
            }
        },
        CallCenterLine: {
            LogicalName: "sb_callcenterline",
            CallCentreLineName: "sb_line",
            CallCentreLineNameEnum: {
                Service: 108550000,
                Sale: 108550001
            }
        },
        Localization: {
            Source: "sb_callcentreLocalization",
            Keys: {
                PhoneNumberError: "PhoneNumberError"
            }
        }
    };

    /************************************************************************************
    * Page events
    ************************************************************************************/

     function onLoad(context) {
        try {
            formContext = context.getFormContext();

            var line = getAttribute(layout.Fields.Line).getValue();
            if (line !== null) {
                openForm();
            }

            var phoneNumber = getAttribute(layout.Fields.PhoneNumber);
            if (phoneNumber.getValue() === null) {
                phoneNumber.setValue("380");
            }

            var formName = formContext.ui.formSelector.getCurrentItem().getLabel();
            setRequiredFieldsOnForm(formName);

        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }
    };

     function onSave(context) {
         try {
             formContext = context.getFormContext();
             formContext.ui.refreshRibbon();

             var line = getAttribute(layout.Fields.Line).getValue();
             if (line !== null) {
                 openForm();
             }

         } catch (e) {
             console.log(e);
             showErrorMessage(e.message);
         }
     };

    /************************************************************************************
   * Ribbon events
   ************************************************************************************/

    function onTaskClick(context) {
        try {
            formContext = context;

            Xrm.Utility.showProgressIndicator("Loading...");

            var id = formContext.data.entity.getId();
            var request = {
                ActionName: layout.ActionNames.CreateTask,
                Parameters: id,

                getMetadata: function () {
                    return {
                        boundParameter: null,
                        parameterTypes: {
                            "ActionName": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            },
                            "Parameters": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            }
                        },
                        operationType: 0,
                        operationName: layout.ActionNames.ActionTracking
                    };
                }
            };

            Xrm.WebApi.online.execute(request).then(
                function success(result) {
                    if (result.ok) {
                        formContext.data.refresh(false).then(function (success) {
                            formContext.ui.refreshRibbon();
                            Xrm.Utility.closeProgressIndicator();
                        }, function (error) {
                            Xrm.Utility.alertDialog(error.message);
                            Xrm.Utility.closeProgressIndicator();
                        });
                    }
                },
                function (error) {
                    Xrm.Utility.alertDialog(error.message);
                    Xrm.Utility.closeProgressIndicator();
                }
            );

        } catch (e) {
            console.log(e);
            Xrm.Utility.closeProgressIndicator();
            showErrorMessage(e.message);
        }
    };

    function isTaskVisible(context) {
        formContext = context;
        var id = formContext.data.entity.getId();
        var task = getAttribute(layout.Fields.Task).getValue();

        if (id !== "" && task === null) {
            return true;
        }

        return false;
    };

    /************************************************************************************
     * Field events
     ************************************************************************************/

    function onPhoneNumberChange(context) {
        formContext = context.getFormContext();
        formContext.getControl(layout.Fields.PhoneNumber).clearNotification();

        var phoneNumber = getAttribute(layout.Fields.PhoneNumber);
        var phoneNumberValue = phoneNumber.getValue();
        var setPhoneNumber = getMaskForPhoneNumber(phoneNumberValue);

        if (!isValidPhoneNumber(setPhoneNumber)) {
            var message = Xrm.Utility.getResourceString(layout.Localization.Source, layout.Localization.Keys.PhoneNumberError);
            formContext.getControl(layout.Fields.PhoneNumber).setNotification(message);
        }
        if (setPhoneNumber !== phoneNumberValue) {
            phoneNumber.setValue(setPhoneNumber);
        }

    }

    /************************************************************************************
    * Helpers
    ************************************************************************************/

    function isValidPhoneNumber(phoneNumber) {
        var pattern = "^(380)[1-9]{1}[0-9]{8}$";

        return phoneNumber.match(pattern);
    }

    function getMaskForPhoneNumber(phoneNumber) {

        if (phoneNumber === null || phoneNumber === undefined) {
            return "380";
        }

        phoneNumber = phoneNumber.replace(" ", "");

        if (phoneNumber.startsWith("380")) {
            return phoneNumber;
        }
        if (phoneNumber === "3" || phoneNumber === "38" || phoneNumber === "") {
            return "380";
        }
        if (phoneNumber.startsWith("0")) {
            return "38" + phoneNumber;
        }
        if (!phoneNumber.startsWith("380")) {
            return "380" + phoneNumber;
        }

        return phoneNumber;
    }

    function openForm() {
        Xrm.WebApi.online.retrieveMultipleRecords(layout.SBCustomSettings.LogicalName).then(
            function success(results) {
                if (results.entities.length === 0) {
                    Xrm.Utility.alertDialog("SB Custom settings not found. Please configure system or contact the system administrator for support.");
                }
                var form = results.entities[0][layout.SBCustomSettings.Fields.DefaultPhoneCallForm];
                if (form == null) {
                    Xrm.Utility.alertDialog("Default Phone Call Form in SB Custom Settings is null. Please configure system or contact the system administrator for support.");
                }
                else {
                    var defaultFormName;

                    switch (form) {
                        case layout.SBCustomSettings.DefaultPhoneCallFormEnum.Sale:
                            defaultFormName = layout.FormName.Sale;
                            break;
                        case layout.SBCustomSettings.DefaultPhoneCallFormEnum.Service:
                            defaultFormName = layout.FormName.Service;
                            break;
                        default:
                            Xrm.Utility.alertDialog("Unknown form in SB Custom settings");
                            return "";
                    }

                    var line = getAttribute(layout.Fields.Line).getValue();
                    var lineId = line[0].id;
                    lineId = lineId.replace('{', '').replace('}', '').toLowerCase();
                    Xrm.WebApi.online.retrieveRecord(layout.CallCenterLine.LogicalName, lineId, "?$select="+layout.CallCenterLine.CallCentreLineName).then(
                        function success(result) {
                            var callCentreLineName = result[layout.CallCenterLine.CallCentreLineName];

                            var currentFormName = formContext.ui.formSelector.getCurrentItem().getLabel();
                            var parameters = {};
                            var serviceId, saleId;
                            var formItems = formContext.ui.formSelector.items.get();

                            for (var i = 0; i < formItems.length; i++) {
                                if (formItems[i]._label === layout.FormName.Service) {
                                    serviceId = formItems[i]._id.guid;
                                }
                                else if (formItems[i]._label === layout.FormName.Sale) {
                                    saleId = formItems[i]._id.guid;
                                }
                            }

                            if (serviceId == undefined) {
                                Xrm.Utility.alertDialog("Service Form not found");
                                return "";
                            }
                            if (saleId == undefined) {
                                Xrm.Utility.alertDialog("Cell Form not found");
                                return "";
                            }

                            var id = Xrm.Page.data.entity.getId();

                            switch (callCentreLineName) {
                                case layout.CallCenterLine.CallCentreLineNameEnum.Service:
                                    if (currentFormName !== layout.FormName.Service) {
                                        parameters["formid"] = serviceId;
                                        Xrm.Utility.openEntityForm(formContext.data.entity.getEntityName(), id, parameters, false);
                                    }
                                    break;
                                case layout.CallCenterLine.CallCentreLineNameEnum.Sale:
                                    if (currentFormName !== layout.FormName.Sale) {
                                        parameters["formid"] = saleId;
                                        Xrm.Utility.openEntityForm(formContext.data.entity.getEntityName(), id, parameters, false);
                                    }
                                    break;
                                default:
                                    if (defaultFormName === layout.FormName.Service && currentFormName !== layout.FormName.Service) {
                                        parameters["formid"] = serviceId;
                                        Xrm.Utility.openEntityForm(formContext.data.entity.getEntityName(), id, parameters, false);
                                    }
                                    else if (defaultFormName === layout.FormName.Sale && currentFormName !== layout.FormName.Sale) {
                                        parameters["formid"] = saleId;
                                        Xrm.Utility.openEntityForm(formContext.data.entity.getEntityName(), id, parameters, false);
                                    }
                                    break;
                            }
                        },
                        function (error) {
                            Xrm.Utility.alertDialog(error.message);
                        }
                    );
                    
                }
            },
            function (error) {
                Xrm.Utility.alertDialog(error.message);
            }
        );
    }

    function setRequiredFieldsOnForm(formName) {
        switch (formName) {
            case layout.FormName.Service:
                requiredField(layout.Fields.Contact);
                requiredField(layout.Fields.CallType);
                break;
            case layout.FormName.Sale:
                requiredField(layout.Fields.PhoneNumber);
                break;
            
        default:
        }
    }

    function requiredField(field) {
        /// <summary> 
        ///     make field required
        ///</summary >
        ///
        getAttribute(field).setRequiredLevel("required");
    }

    function getAttribute(attributeName) {
        /// <summary> Returns attribute. </summary>
        /// <param name="attributeName" type="string"> Attribute name. </param>
        /// <returns type="Object"> Return the attribute. </returns>

        var attribute = formContext.getAttribute(attributeName);
        if (attribute == null) {
            throw new Error("Data Field " + attributeName + " not found.");
        }

        return attribute;
    }

    function showErrorMessage(message) {

        var guid = Date.now().toString();

        formContext.ui.setFormNotification(message, "ERROR", guid);

        setTimeout(clearMessage, 5000, guid);
    }

    function clearMessage(guid) {
        try {

            formContext.ui.clearFormNotification(guid);

        } catch (e) {
            Xrm.Navigation.openAlertDialog({confirmButtonLabel: "Ok", text: e.message});
        }
    }

    return {
        onLoad: onLoad,
        onTaskClick: onTaskClick,
        isTaskVisible: isTaskVisible,
        onSave: onSave,
        onPhoneNumberChange: onPhoneNumberChange
    };
})(window, document);