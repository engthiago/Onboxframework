using Onbox.Abstractions.V7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V7.Http
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient client;
        private readonly IJsonService jsonService;
        private readonly ILoggingService loggingService;
        private readonly IHttpInterceptor httpInterceptor;
        private readonly HttpSettings httpSettings;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public HttpService(IJsonService jsonService, ILoggingService loggingService, IHttpInterceptor httpInterceptor, HttpSettings httpSettings)
        {
            this.client = new HttpClient();
            this.Configure(httpSettings);

            // Timeout can be configured only once during the lifetime
            this.client.Timeout = TimeSpan.FromMilliseconds(httpSettings.Timeout);

            this.jsonService = jsonService;
            this.loggingService = loggingService;
            this.httpInterceptor = httpInterceptor;
            this.httpSettings = httpSettings;
        }

        private HttpRequestMessage GetRequestMessage(HttpMethod httpMethod, string endpoint, HttpContent content = null)
        {
            var request = new HttpRequestMessage(httpMethod, endpoint);

            foreach (var header in this.client.DefaultRequestHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            if (content != null)
            {
                request.Content = content;
            }

            return request;
        }


        public async Task<T> GetAsync<T>(string endpoint, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var request = this.GetRequestMessage(HttpMethod.Get, endpoint);
            this.httpInterceptor.BeforeSending(request);

            var response = await this.client.SendAsync(request);

            this.ClearHeaders();

            this.httpInterceptor.AfterSending(response);
            await EnsureSuccess(response);

            var json = await response.Content.ReadAsStringAsync();
            return this.ConvertResponseToType<T>(json);
        }

        public async Task<Stream> GetStreamAsync(string endpoint, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var request = this.GetRequestMessage(HttpMethod.Get, endpoint);
            this.httpInterceptor.BeforeSending(request);

            var response = await this.client.GetAsync(endpoint);

            this.ClearHeaders();
            await EnsureSuccess(response);

            var stream = await response.Content.ReadAsStreamAsync();
            return stream;
        }

        public async Task<T> DeleteAsync<T>(string endpoint, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var request = this.GetRequestMessage(HttpMethod.Delete, endpoint);
            this.httpInterceptor.BeforeSending(request);

            var response = await this.client.SendAsync(request);

            this.ClearHeaders();
            await EnsureSuccess(response);

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

            var request = this.GetRequestMessage(HttpMethod.Delete, endpoint);
            this.httpInterceptor.BeforeSending(request);

            var response = await this.client.SendAsync(request);

            this.ClearHeaders();
            await EnsureSuccess(response);
        }

        public async Task<T> PutAsync<T>(string endpoint, object content, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {

                var request = this.GetRequestMessage(HttpMethod.Put, endpoint, jsonContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);

                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return this.ConvertResponseToType<T>(json);
                }
            }

            return null;
        }

        public async Task PutAsync(string endpoint, object content, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var request = this.GetRequestMessage(HttpMethod.Put, endpoint, jsonContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);
            }
        }

        public async Task<T> PutStreamAsync<T>(string endpoint, Stream content, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            using (var streamContent = new StreamContent(content))
            {
                var request = this.GetRequestMessage(HttpMethod.Put, endpoint, streamContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);

                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return this.ConvertResponseToType<T>(json);
                }

                return null;
            }

        }

        public async Task PutStreamAsync(string endpoint, Stream content, string token = null)
        {
            using (var streamContent = new StreamContent(content))
            {
                var request = this.GetRequestMessage(HttpMethod.Put, endpoint, streamContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);
            }
        }

        public async Task<Stream> PutRequestStreamAsync(string endpoint, object content, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var request = this.GetRequestMessage(HttpMethod.Put, endpoint, jsonContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);

                var stream = await response.Content.ReadAsStreamAsync();
                return stream;
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object content, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var request = this.GetRequestMessage(HttpMethod.Post, endpoint, jsonContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);

                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return this.ConvertResponseToType<T>(json);
                }
            }

            return null;
        }

        public async Task PostAsync(string endpoint, object content, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var request = this.GetRequestMessage(HttpMethod.Post, endpoint, jsonContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);
            }
        }

        public async Task<T> PostStreamAsync<T>(string endpoint, Stream content, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            using (var streamContent = new StreamContent(content))
            {
                var request = this.GetRequestMessage(HttpMethod.Post, endpoint, streamContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);

                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return this.ConvertResponseToType<T>(json);
                }

                return null;
            }
        }

        public async Task PostStreamAsync(string endpoint, Stream content, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            using (var streamContent = new StreamContent(content))
            {
                var request = this.GetRequestMessage(HttpMethod.Post, endpoint, streamContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);
            }
        }

        public async Task<Stream> PostRequestStreamAsync(string endpoint, object content, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var request = this.GetRequestMessage(HttpMethod.Post, endpoint, jsonContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);

                var stream = await response.Content.ReadAsStreamAsync();
                return stream;
            }
        }

        public async Task<T> PostFormAsync<T>(string endpoint, IDictionary<string, string> content, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            using (var formContent = new FormUrlEncodedContent(content))
            {
                var request = this.GetRequestMessage(HttpMethod.Post, endpoint, formContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);

                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return this.ConvertResponseToType<T>(json);
                }
            }

            return null;
        }

        public async Task<T> PatchAsync<T>(string endpoint, object content, string token = null) where T : class
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var request = this.GetRequestMessage(new HttpMethod("PATCH"), endpoint, jsonContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);

                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return this.ConvertResponseToType<T>(json);
                }
            }

            return null;
        }

        public async Task PatchAsync(string endpoint, object content, string token = null)
        {
            this.EnsureIsConnected();
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var request = this.GetRequestMessage(new HttpMethod("PATCH"), endpoint, jsonContent);
                this.httpInterceptor.BeforeSending(request);

                var response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);
            }
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

        private async Task EnsureSuccess(HttpResponseMessage response)
        {
            if (response == null) throw new Exception("Invalid Http Response");

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            // Ignore errors on logging
            try
            {
                var responseJson = await response.RequestMessage?.Content?.ReadAsStringAsync();
                await loggingService.Error($"{(int)response.StatusCode } {response.StatusCode.ToString()}: {responseJson}");
            }
            catch
            {
            }

            throw new HttpListenerException((int)response.StatusCode, response.StatusCode.ToString());
        }

        private void Configure(HttpSettings settings)
        {
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

        public IHttpService AddHeader(string name, string value)
        {
            this.SetHeader(name, value);
            return this;
        }

        private void SetHeader(string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (this.client.DefaultRequestHeaders.Contains(name))
                {
                    this.client.DefaultRequestHeaders.Remove(name);
                }

                this.client.DefaultRequestHeaders.Add(name, value);
            }
            else
            {
                this.client.DefaultRequestHeaders.Remove(name);
            }
        }

        public void ClearHeaders()
        {
            this.client.DefaultRequestHeaders.Clear();
            this.Configure(this.httpSettings);
        }

        public void Dispose()
        {
            client.Dispose();
        }

        ~HttpService()
        {
            client.Dispose();
        }
    }
}
