using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SB.SharedModels.Actions;
using SB.WebShared.DynamicsAuthentication;

namespace FunctionApp
{
    public class Functions
    {
        private const string Cron = "%CRON_EXPRESSION%";

        private readonly IAuthenticationService _authenticationService;

        public Functions(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(IAuthenticationService));
        }

        [FunctionName("ResettingCallCounterOnLine")]
        public async Task Run([TimerTrigger(Cron)] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var token = await _authenticationService.GetToken();
            var serviceUrl = _authenticationService.GetServiceUri();

            var json = JsonCreatorService.FormStringContent("", ActionNames.ActionTrackingNames.ResettingCallCounterOnLine);

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.PostAsync(serviceUrl, json);

            log.LogInformation(response.IsSuccessStatusCode
                ? $"ResettingCallCounterOnLine action success finished "
                : $"ResettingCallCounterOnLine action finished with {response.StatusCode}");
        }
    }
}