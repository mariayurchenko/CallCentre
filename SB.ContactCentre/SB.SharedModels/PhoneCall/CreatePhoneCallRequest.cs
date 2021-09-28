using System.Runtime.Serialization;

namespace SB.SharedModels.PhoneCall
{
    [DataContract]
    public class CreatePhoneCallRequest
    {
        [DataMember(Name = "callId")]
        public string CallId { get; set; }

        [DataMember(Name = "clientNumber")]
        public string ClientNumber { get; set; }

        [DataMember(Name = "lineNumber")]
        public string LineNumber { get; set; }

        [DataMember(Name = "direction")]
        public bool? Direction { get; set; }
    }
}