using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using SB.Shared.GraphApiModels;
using Newtonsoft.Json.Linq;
using SB.Shared;

namespace GraphApi.Service
{
    public class GraphApiService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tenant;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _token;

        private const string Url = "https://graph.microsoft.com";

        public GraphApiService(string clientId, string clientSecret, string tenant, string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new Exception($"{nameof(clientId)} is null or white-space");
            }
            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new Exception($"{nameof(clientSecret)} is null or white-space");
            }
            if (string.IsNullOrWhiteSpace(tenant))
            {
                throw new Exception($"{nameof(tenant)} is null or white-space");
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new Exception($"{nameof(userName)} is null or white-space");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception($"{nameof(password)} is null or white-space");
            }

            _clientId = clientId;
            _clientSecret = clientSecret;
            _tenant = tenant;
            _userName = userName;
            _password = password;

            _token = GetGraphToken();
        }

        public string CreateMeeting(Meeting meeting)
        {
            var json = JsonSerializer.Serialize(meeting);

            var body = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = httpClient.PostAsync(new Uri($"{Url}/v1.0/me/calendar/events"), body).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(responseString);
            }

            var id = JObject.Parse(responseString)["id"]?.ToString();

            return id;
        }

        public void UpdateMeeting(Meeting meeting, string id)
        {
            var json = JsonSerializer.Serialize(meeting);

            var body = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{Url}/v1.0/me/calendar/events/{id}")
            {
                Content = body 
            };

            var response = httpClient.SendAsync(request).Result;

            if (response.IsSuccessStatusCode) return;

            var responseString = response.Content.ReadAsStringAsync().Result;

            throw new Exception(responseString);
        }

        public void DeleteMeeting(string id)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = httpClient.DeleteAsync(new Uri($"{Url}/v1.0/me/calendar/events/{id}")).Result;

            if (response.IsSuccessStatusCode) return;

            var responseString = response.Content.ReadAsStringAsync().Result;

            throw new Exception(responseString);
        }

        public void CancelMeeting(string id)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = httpClient.PostAsync(new Uri($"{Url}/v1.0/me/calendar/events/{id}/cancel"), null).Result;

            if (response.IsSuccessStatusCode) return;

            var responseString = response.Content.ReadAsStringAsync().Result;

            throw new Exception(responseString);
        }

        private string GetGraphToken()
        {
            var content = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"client_secret", _clientSecret},
                {"client_id", _clientId},
                {"username", _userName},
                {"password", _password},
                {"scope", $"{Url}/.default"}
            };

            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{_tenant}/oauth2/v2.0/token")
            {
                Content = new FormUrlEncodedContent(content)
            };

            var response = httpClient.SendAsync(request).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(responseString);
            }

            var token = JObject.Parse(responseString)["access_token"]?.ToString();

            return token;
        }
    }
}