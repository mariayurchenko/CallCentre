using System.Runtime.Serialization;

namespace SB.SharedModels.PhoneCall
{
    [DataContract]
    public class ClosePhoneCallResultModel
    {
        [DataMember(Name = "number")]
        public string Number { get; set; }

        [DataMember(Name = "callCenterLine")]
        public string CallCenterLine { get; set; }
    }
}