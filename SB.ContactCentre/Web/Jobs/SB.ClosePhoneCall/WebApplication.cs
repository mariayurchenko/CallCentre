namespace SB.ClosePhoneCall
{
    public class WebApplication
    {
        private const string Url = "https://mycontactcentree.azurewebsites.net";

        public class AuthenticationController
        {
            public static readonly string Post = $"{WebApplication.Url}/api/login";
        }

        public class PhoneCallController
        {
            private const string Prefix = "phonecall";
            public static readonly string GetOpenPhoneCalls = $"{WebApplication.Url}/api/{Prefix}";
            public static readonly string ClosePhoneCall = $"{WebApplication.Url}/api/{Prefix}/close";
            public static readonly string CloseOldPhoneCalls = $"{WebApplication.Url}/api/{Prefix}/close/oldphonecalls";
        }
    }
}