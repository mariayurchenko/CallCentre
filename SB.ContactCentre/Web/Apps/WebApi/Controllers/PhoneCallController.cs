using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SB.SharedModels;
using SB.SharedModels.Actions;
using SB.SharedModels.PhoneCall;
using SB.SharedModels.Helpers;
using SB.WebShared.DynamicsAuthentication;
using Response = SB.SharedModels.Response;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PhoneCallController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public PhoneCallController(IAuthenticationService authentication)
        {
            _authenticationService = authentication;
        }

        [HttpGet]
        public async Task<IActionResult> GetOpenPhoneCalls()
        {
            try
            {
                var token = await _authenticationService.GetToken();
                var serviceUrl = _authenticationService.GetServiceUri();

                var json = JsonCreatorService.FormStringContent("", ActionNames.ActionTrackingNames.GetPhoneNumbersForUnclosedCards);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = httpClient.PostAsync(serviceUrl, json).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(responseString);
                }

                var responseModel = JsonConvert.DeserializeObject<ResponseModel>(responseString);

                if (responseModel == null) return BadRequest("Not Deserialize");

                var responseMessage = JsonConvert.DeserializeObject<Response>(responseModel.Response);

                if (responseMessage == null) return BadRequest("Not Deserialize");

                var phoneNumbers = JsonConvert.DeserializeObject<string[]>(responseMessage.Value);

                return Ok(phoneNumbers);

            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePhoneCall([FromBody] CreatePhoneCallRequest phoneCall)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneCall.CallId))
                {
                    return BadRequest($"{nameof(phoneCall.CallId)} is null, empty or white-space");
                }
                if (string.IsNullOrWhiteSpace(phoneCall.ClientNumber))
                {
                    return BadRequest($"{nameof(phoneCall.ClientNumber)} is null, empty or white-space");
                }
                if (string.IsNullOrWhiteSpace(phoneCall.LineNumber))
                {
                    return BadRequest($"{nameof(phoneCall.LineNumber)} is null, empty or white-space");
                }
                if (!phoneCall.Direction.HasValue)
                {
                    return BadRequest($"{nameof(phoneCall.Direction)} is null");
                }


                var token = await _authenticationService.GetToken();
                var serviceUrl = _authenticationService.GetServiceUri();

                var json = JsonCreatorService.FormStringContent(phoneCall, ActionNames.ActionTrackingNames.CreatePhoneCall);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response =  httpClient.PostAsync(serviceUrl, json).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(responseString);
                }

                var responseModel = JsonConvert.DeserializeObject<ResponseModel>(responseString);

                if (responseModel == null) return BadRequest("Not Deserialize");

                var responseMessage = JsonConvert.DeserializeObject<Response>(responseModel.Response);

                if (responseMessage == null) return BadRequest("Not Deserialize");

                var createPhoneCallResponse = JsonConvert.DeserializeObject<CreatePhoneCallResponse>(responseMessage.Value);

                return Ok(createPhoneCallResponse);

            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPut("close")]
        public async Task<IActionResult> ClosePhoneCall([FromBody] string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    return BadRequest($"{phoneNumber} is null, empty or white-space");
                }

                phoneNumber = VariableCheck.ValidatePhoneNumber(phoneNumber);

                var token = await _authenticationService.GetToken();
                var serviceUrl = _authenticationService.GetServiceUri();

                var json = JsonCreatorService.FormStringContent(phoneNumber, ActionNames.ActionTrackingNames.ClosePhoneCall);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = httpClient.PostAsync(serviceUrl, json).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(responseString);
                }

                var responseModel = JsonConvert.DeserializeObject<ResponseModel>(responseString);

                if (responseModel == null) return BadRequest("Not Deserialize");

                var responseMessage = JsonConvert.DeserializeObject<Response>(responseModel.Response);

                if (responseMessage == null) return BadRequest("Not Deserialize");

                var closePhoneCallResponse = JsonConvert.DeserializeObject<ClosePhoneCallResponse>(responseMessage.Value);

                return Ok(closePhoneCallResponse);

            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPut("close/oldphonecalls")]
        public async Task<IActionResult> CloseOldPhoneCalls()
        {
            try
            {
                var token = await _authenticationService.GetToken();
                var serviceUrl = _authenticationService.GetServiceUri();

                var json = JsonCreatorService.FormStringContent("", ActionNames.ActionTrackingNames.CloseOldPhoneCalls);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = httpClient.PostAsync(serviceUrl, json).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(responseString);
                }

                var responseModel = JsonConvert.DeserializeObject<ResponseModel>(responseString);

                if (responseModel == null) return BadRequest("Not Deserialize");

                var responseMessage = JsonConvert.DeserializeObject<Response>(responseModel.Response);

                if (responseMessage == null) return BadRequest("Not Deserialize");

                var closePhoneCallResponse = JsonConvert.DeserializeObject<CloseOldPhoneCallsResponse>(responseMessage.Value);

                return Ok(closePhoneCallResponse);

            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}