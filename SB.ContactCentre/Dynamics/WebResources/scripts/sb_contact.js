var page = (function (window, document) {
    
	/************************************************************************************
    * Variables
    ************************************************************************************/
    var formContext;

    var layout = {
        Fields: {
            AppId: "",
            EntityName: "",
            ClientUrl: "",
            IsDuplicate: true,
            Email: "emailaddress1"
        },
        SBCustomSettings: {
            LogicalName: "sb_sbcustomsettings",
            Fields: {
                SecurityRoleForChangedFieldName: "_sb_securityrole_value@OData.Community.Display.V1.FormattedValue"
            }
        }
    };

    /************************************************************************************
    * Page events
    ************************************************************************************/

    function onLoad(context) {
        try {
            formContext = context.getFormContext();
            formContext.getControl(layout.Fields.Email).setDisabled(true);

            var currentUserId = formContext.context.getUserId();
            if (currentUserId !== null && currentUserId != undefined) {
                currentUserId = currentUserId.slice(1, -1);
                checkSecurityRoleAndSetLockFields(currentUserId);
            }

            var globalContext = Xrm.Utility.getGlobalContext();
            globalContext.getCurrentAppProperties().then(
                function success(app) {
                    layout.Fields.AppId = app.appId;
                },
                function errorCallback() {
                    console.log("Error");
                });

            layout.Fields.EntityName = formContext.data.entity.getEntityName();
            layout.Fields.ClientUrl = Xrm.Page.context.getClientUrl();

        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }
    };

    function onSave(context) {
        try {
            var saveEvent = context.getEventArgs();

            formContext = context.getFormContext();

            var id = Xrm.Page.data.entity.getId();
            if (id != null) {
                id = id.replace('{', '').replace('}', '');
                id = id.toLowerCase();
            }

            var mobilePhone = formContext.getAttribute("mobilephone").getValue();
            if (mobilePhone !== null) {

                if (layout.Fields.IsDuplicate) {
                    saveEvent.preventDefault();
                }
                else {
                    layout.Fields.IsDuplicate = true;
                    return;
                }
                var birthdate = formContext.getAttribute("birthdate").getValue();
                if (birthdate !== null && birthdate !== undefined) {
                    var year = birthdate.getFullYear() + "";
                    var month = (birthdate.getMonth() + 1) + "";
                    var day = birthdate.getDate() + "";
                    birthdate = year + "-" + month + "-" + day;
                }

                Xrm.WebApi.online.retrieveMultipleRecords("contact", "?$filter=mobilephone eq '" + mobilePhone + "' and  birthdate eq " + birthdate).then(
                    function success(results) {
                        layout.Fields.IsDuplicate = false;
                        for (var i = 0; i < results.entities.length; i++) {
                            var contactId = results.entities[i]["contactid"];

                            if (contactId !== id) {
                                layout.Fields.IsDuplicate = true;
                                showDuplicateEntityMessage(contactId);
                                break;
                            }
                        }
                        if (!layout.Fields.IsDuplicate) {
                            Xrm.Page.data.entity.save();
                        }
                    },
                    function (error) {
                        Xrm.Utility.alertDialog(error.message);
                    }
                );
            }

        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }
    };

    /************************************************************************************
     * Field events
     ************************************************************************************/


    /************************************************************************************
    * Helpers
    ************************************************************************************/

    function checkSecurityRoleAndSetLockFields(currentUserId) {
        Xrm.WebApi.online.retrieveMultipleRecords(layout.SBCustomSettings.LogicalName).then(
            function success(results) {
                if (results.entities.length === 0) {
                    Xrm.Utility.alertDialog(
                        "SB Custom settings not found. Please configure system or contact the system administrator for support.");
                }
                var role = results.entities[0][layout.SBCustomSettings.Fields.SecurityRoleForChangedFieldName];
                if (role == null) {
                    Xrm.Utility.alertDialog("Security role for changed field in SB Custom Settings is null. Please configure system or contact the system administrator for support.");
                    return;
                }
                Xrm.WebApi.online.retrieveMultipleRecords("systemuserroles", `?$select=roleid&$filter=systemuserid eq ${currentUserId}`).then(
                    function success(results) {
                        for (var i = 0; i < results.entities.length; i++) {
                            var roleId = results.entities[i]["roleid"];
                            setLockFieldsByRole(roleId, role);
                        }
                    },
                    function (error) {
                        Xrm.Utility.alertDialog(error.message);
                    }
                );
            },
            function(error) {
                Xrm.Utility.alertDialog(error.message);
            }
        );
    }

    function setLockFieldsByRole(roleId, securityRole) {
        Xrm.WebApi.online.retrieveMultipleRecords("role", `?$filter=roleid eq ${roleId}`).then(
            function success(results) {
                if (results.entities[0]["name"] === securityRole) {
                    formContext.getControl(layout.Fields.Email).setDisabled(false);
                }
            },
            function (error) {
                Xrm.Utility.alertDialog(error.message);
            }
        );
    }

    function getEntityRecordUrl(id) {

        var entityRecordUrl = layout.Fields.ClientUrl + '/main.aspx?appid=' + layout.Fields.AppId + '&pagetype=entityrecord&etn=' + layout.Fields.EntityName + '&id=' + id;

        return entityRecordUrl;
    }

    function showDuplicateEntityMessage(id) {

        var message = "Duplicates found: " + getEntityRecordUrl(id);
        var alertStrings = { confirmButtonLabel: "Cancel", text: message, title: "Duplicate records found" };
        var alertOptions = { height: 260, width: 460 };

        Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
            function (success) {
                console.log("Alert dialog closed");
            },
            function (error) {
                console.log(error.message);
            }
        );
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
        onSave: onSave
    };
})(window, document);