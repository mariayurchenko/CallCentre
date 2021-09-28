using System.Runtime.Serialization;

namespace SB.SharedModels
{
    [DataContract]
    public class ResponseModel
    {
        [DataMember(Name = "@odata.context")]
        public string OdataContext { get; set; }
        [DataMember(Name = "Response")]
        public string Response { get; set; }
    }

    public class Response
    {
        public int Status { get; set; }
        public string Value { get; set; }
    }
}
