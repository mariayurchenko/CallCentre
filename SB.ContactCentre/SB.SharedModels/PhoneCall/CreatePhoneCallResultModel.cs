using System.Runtime.Serialization;

namespace SB.SharedModels.PhoneCall
{
    [DataContract]
    public class CreatePhoneCallResultModel
    {
        [DataMember(Name = "additionalInfo")]
        public PhoneCallModel AdditionalInfo { get; set; }
    }
}