using System;
using System.Runtime.Serialization;

namespace SB.SharedModels.PhoneCall
{
    [DataContract]
    public class PhoneCallModel
    {
        [DataMember(Name = "fullName")]
        public string FullName { get; set; }

        [DataMember(Name = "language")]
        public int? Language { get; set; }

        [DataMember(Name = "dateofbirth")]
        public DateTime? DateOfBirth { get; set; }

        [DataMember(Name = "phoneCallUrl")]
        public string PhoneCallUrl { get; set; }
    }
}