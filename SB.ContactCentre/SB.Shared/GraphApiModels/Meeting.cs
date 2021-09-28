using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SB.Shared.GraphApiModels
{
    [DataContract]
    public class Meeting
    {
        [DataMember(Name = "subject", EmitDefaultValue = false)]
        public string Subject { get; set; }

        [DataMember(Name = "body" , EmitDefaultValue = false)]
        public Body Body { get; set; }

        [DataMember(Name = "start", EmitDefaultValue = false)]
        public Date Start { get; set; }

        [DataMember(Name = "end", EmitDefaultValue = false)]
        public Date End { get; set; }

        [DataMember(Name = "location", EmitDefaultValue = false)]
        public Location Location { get; set; }

        [DataMember(Name = "attendees", EmitDefaultValue = false)]
        public List<Attendee> Attendees { get; set; }

        [DataMember(Name = "isOnlineMeeting", EmitDefaultValue = false)]
        public bool? IsOnlineMeeting { get; set; }
    }

    [DataContract]
    public class Body
    {
        [DataMember(Name = "content")]
        public string Content { get; set; }
    }

    [DataContract]
    public class Date
    {
        [DataMember(Name = "dateTime")]
        public DateTime DateTime { get; set; }

        [DataMember(Name = "timeZone")]
        public string TimeZone { get; set; }
    }

    [DataContract]
    public class Location
    {
        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }
    }

    [DataContract]
    public class EmailAddress
    {
        [DataMember(Name = "address")]
        public string Address { get; set; }
    }

    [DataContract]
    public class Attendee
    {
        [DataMember(Name = "emailAddress")]
        public EmailAddress EmailAddress { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}