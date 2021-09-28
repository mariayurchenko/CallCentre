using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using SB.SharedModels.Authentication;
using SB.SharedModels.PhoneCall;

namespace SB.ClosePhoneCall
{
    public class PhoneCallService
    {
        private readonly string _userName;
        private readonly string _userPassword;
        private string _token;

        public PhoneCallService(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new Exception($"{nameof(userName)} is null, empty or whitespace");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception($"{nameof(password)} is null, empty or whitespace");
            }

            _userName = GetHash(userName);
            _userPassword = GetHash(password);
            _token = GetToken();
        }

        public string GetOpenPhoneNumbers()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = httpClient.GetAsync(WebApplication.PhoneCallController.GetOpenPhoneCalls).Result;

            var responseMessage = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = "Request returned unsuccessful status code. ";

                if (responseMessage != null)
                    errorMessage += responseMessage;

                throw new Exception(errorMessage);
            }

            if (response.StatusCode != HttpStatusCode.Unauthorized) 
                return responseMessage;

            _token = GetToken();
            responseMessage = GetOpenPhoneNumbers();

            return responseMessage;
        }

        public CloseOldPhoneCallsResponse CloseOldPhoneCalls()
        {
            var json = JsonConvert.SerializeObject("");
            var result = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = httpClient.PutAsync(WebApplication.PhoneCallController.CloseOldPhoneCalls, result).Result;

            var responseMessage = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = "Request returned unsuccessful status code. ";

                if (responseMessage != null)
                    errorMessage += responseMessage;

                throw new Exception(errorMessage);
            }

            if (string.IsNullOrWhiteSpace(responseMessage))
            {
                throw new Exception("Response is null");
            }

            var closeOldPhoneCallsResponse = JsonConvert.DeserializeObject<CloseOldPhoneCallsResponse>(responseMessage);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return closeOldPhoneCallsResponse;

            _token = GetToken();
            closeOldPhoneCallsResponse = CloseOldPhoneCalls();

            return closeOldPhoneCallsResponse;
        }
        public ClosePhoneCallResponse ClosePhoneCall(string phoneNumber)
        {
            var json = JsonConvert.SerializeObject(phoneNumber);
            var result = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = httpClient.PutAsync(WebApplication.PhoneCallController.ClosePhoneCall, result).Result;

            var responseMessage = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = "Request returned unsuccessful status code. ";

                if (responseMessage != null)
                    errorMessage += responseMessage;

                throw new Exception(errorMessage);
            }

            if (string.IsNullOrWhiteSpace(responseMessage))
            {
                throw new Exception("Response is null");
            }

            var closePhoneCallResponse = JsonConvert.DeserializeObject<ClosePhoneCallResponse>(responseMessage);

            if (closePhoneCallResponse == null)
            {
                throw new Exception("Not Deserialize");
            }

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return closePhoneCallResponse;

            _token = GetToken();
            closePhoneCallResponse = ClosePhoneCall(phoneNumber);

            return closePhoneCallResponse;
        }

        private string GetHash(string text)
        {
            using var sha256 = SHA256.Create();

            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

            var hashedString = BitConverter.ToString(hashedBytes).Replace("-", "");

            return hashedString;
        }

        private string GetToken()
        {
            var login = new LoginDataModel
            {
                UserName = _userName,
                UserPassword = _userPassword
            };

            var json = JsonConvert.SerializeObject(login);

            var result = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();

            var response = httpClient.PostAsync(WebApplication.AuthenticationController.Post, result).Result;

            var responseMessage =  response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage ?? "Request returned unsuccessful status code");
            }

            if (string.IsNullOrWhiteSpace(responseMessage))
            {
                throw new Exception("Response is null");
            }

            var token = JsonConvert.DeserializeObject<TokenModel>(responseMessage);

            if (token?.Token == null)
            {
                throw new Exception("Token is null");
            }

            return token.Token;
        }
    }
}