using System.Runtime.Serialization;

namespace SB.SharedModels.PhoneCall
{
    [DataContract]
    public class ClosePhoneCallResponse
    {
        [DataMember(Name = "isSucceeded")]
        public bool IsSucceeded { get; set; }

        [DataMember(Name = "result")]
        public ClosePhoneCallResultModel Result { get; set; }

        [DataMember(Name = "validationErrors")]
        public object ValidationErrors { get; set; }
    }
}