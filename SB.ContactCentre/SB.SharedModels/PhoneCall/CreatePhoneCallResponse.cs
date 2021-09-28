using System;
using System.Runtime.Serialization;

namespace SB.SharedModels.PhoneCall
{
    [DataContract]
    public class CreatePhoneCallResponse
    {
        [DataMember(Name = "id")]
        public Guid? Id { get; set; }

        [DataMember(Name = "isSucceeded")]
        public bool IsSucceeded { get; set; }

        [DataMember(Name = "result")]
        public CreatePhoneCallResultModel Result { get; set; }

        [DataMember(Name = "validationErrors")]
        public object ValidationErrors { get; set; }
    }
}