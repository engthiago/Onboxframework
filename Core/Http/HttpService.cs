using Onbox.Core.V1.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V1.Http
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string endpoint, string token = null) where T : class;
        Task<T> DeleteAsync<T>(string endpoint, string token = null) where T : class;
        Task DeleteAsync(string endpoint, string token = null);
        Task<T> PutAsync<T>(string endpoint, object content, string token = null) where T : class;
        Task PutAsync(string endpoint, object content, string token = null);
        Task<T> PostAsync<T>(string endpoint, object content, string token = null) where T : class;
        Task PostAsync(string endpoint, object content, string token = null);
        Task<T> PatchAsync<T>(string endpoint, object content, string token = null) where T : class;
        Task PatchAsync(string endpoint, object content, string token = null);
    }

    public class HttpService : IHttpService
    {
        private readonly HttpClient client;
        private readonly IJsonService jsonService;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public HttpService(IJsonService jsonService, HttpSettings httpSettings)
        {
            this.client = new HttpClient();
            this.Configure(httpSettings);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                    SecurityProtocolType.Tls11 |
                                                    SecurityProtocolType.Tls12;

            this.jsonService = jsonService;
        }


        public async Task<T> GetAsync<T>(string endpoint, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var response = await this.client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return this.ConvertResponseToType<T>(json);
        }

        public async Task<T> DeleteAsync<T>(string endpoint, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var response = await this.client.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(json))
            {
                return this.ConvertResponseToType<T>(json);
            }

            return null;
        }

        public async Task DeleteAsync(string endpoint, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var response = await this.client.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> PutAsync<T>(string endpoint, object content, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await this.client.PutAsync(endpoint, jsonContent);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(json))
            {
                return this.ConvertResponseToType<T>(json);
            }

            return null;
        }

        public async Task PutAsync(string endpoint, object content, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await this.client.PutAsync(endpoint, jsonContent);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> PostAsync<T>(string endpoint, object content, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await this.client.PostAsync(endpoint, jsonContent);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(json))
            {
                return this.ConvertResponseToType<T>(json);
            }

            return null;
        }

        public async Task PostAsync(string endpoint, object content, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await this.client.PostAsync(endpoint, jsonContent);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> PatchAsync<T>(string endpoint, object content, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, endpoint)
            {
                Content = jsonContent
            };

            HttpResponseMessage response = new HttpResponseMessage();
            response = await this.client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(json))
            {
                return this.ConvertResponseToType<T>(json);
            }

            return null;
        }

        public async Task PatchAsync(string endpoint, object content, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, endpoint)
            {
                Content = jsonContent
            };

            HttpResponseMessage response = new HttpResponseMessage();
            response = await this.client.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }


        private void SetTokenHeaders(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                if (this.client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    this.client.DefaultRequestHeaders.Remove("Authorization");
                }

                this.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
            else
            {
                this.client.DefaultRequestHeaders.Remove("Authorization");
            }
        }

        private T ConvertResponseToType<T>(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new Exception("Response json is empty");
                }
                var data = this.jsonService.Deserialize<T>(json);
                return data;
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Could not convert response to {typeof(T).Name}");
            }
        }

        private bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        private void EnsureValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new HttpListenerException(400, "Could not get a valid response from the server.");
            }
        }

        private void EnsureIsConnected()
        {
            if (!this.IsConnectedToInternet())
            {
                throw new WebException("Could not connect to the internet.", WebExceptionStatus.ConnectFailure);
            }
        }

        private void Configure(HttpSettings settings)
        {
            this.client.Timeout = TimeSpan.FromMilliseconds(settings.Timeout);
            this.SetCacheHeaders(settings.AllowCache ? null : "no-cache");
        }

        private void SetCacheHeaders(string cacheValue)
        {
            if (this.client.DefaultRequestHeaders.Contains("cache-control"))
            {
                this.client.DefaultRequestHeaders.Remove("cache-control");
            }
            
            if (!string.IsNullOrWhiteSpace(cacheValue))
            {
                this.client.DefaultRequestHeaders.Add("cache-control", cacheValue);
            }
        }
    }
}
