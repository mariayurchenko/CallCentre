using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using SB.WebShared.DynamicsAuthentication;

namespace FunctionApp.Tests
{
    public class TestFactory
    {
        private static IConfigurationRoot _config;
        private static CrmClientOptions _crmClientOptions;
        private static MemoryStream _memoryStream;

        public static CrmClientOptions CreateCrmClientOptions()
        {
            if (_crmClientOptions != null) return _crmClientOptions;

            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            _crmClientOptions = new CrmClientOptions().BindStringMembersFromConnectionString(configuration.GetValue<string>("ClientOptions")) as CrmClientOptions;

            return _crmClientOptions;
        }

        public static Mock<HttpRequest> CreateMockRequest(object body)
        {
            var json = JsonConvert.SerializeObject(body);
            var byteArray = Encoding.UTF8.GetBytes(json);

            _memoryStream = new MemoryStream(byteArray);
            _memoryStream.Flush();
            _memoryStream.Position = 0;

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Body).Returns(_memoryStream);

            return mockRequest;
        }

        public static IConfigurationRoot GetConfigurationRoot()
        {
            if (_config != null) return _config;

            _config = new ConfigurationBuilder()
                .AddUserSecrets("06512aa6-4bd6-44fd-a96d-7c1bd3a0ffbb")
                .AddEnvironmentVariables()
                .Build();

            return _config;
        }
    }
}