using System.Runtime.Serialization;

namespace SB.SharedModels.Authentication
{
    [DataContract]
    public class TokenModel
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}