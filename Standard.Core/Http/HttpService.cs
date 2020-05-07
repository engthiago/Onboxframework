using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Standard.Core.Http
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string endpoint, string token = null) where T : class;
        Task<Stream> GetStreamAsync(string endpoint, string token = null);
        Task<T> DeleteAsync<T>(string endpoint, string token = null) where T : class;
        Task DeleteAsync(string endpoint, string token = null);
        Task<T> PutAsync<T>(string endpoint, object content, string token = null) where T : class;
        Task PutAsync(string endpoint, object content, string token = null);
        Task<T> PutStreamAsync<T>(string endpoint, Stream content, string token = null) where T : class;
        Task PutStreamAsync(string enpoint, Stream content, string token = null);
        Task<Stream> PutRequestStreamAsync(string endpoint, object content, string token = null);
        Task<T> PostAsync<T>(string endpoint, object content, string token = null) where T : class;
        Task<T> PostFormAsync<T>(string endpoint, IDictionary<string, string> content, string token = null) where T : class;
        Task PostAsync(string endpoint, object content, string token = null);
        Task<T> PostStreamAsync<T>(string endpoint, Stream content, string token = null) where T : class;
        Task PostStreamAsync(string endpoint, Stream content, string token = null);
        Task<Stream> PostRequestStreamAsync(string endpoint, object content, string token = null);
        Task<T> PatchAsync<T>(string endpoint, object content, string token = null) where T : class;
        Task PatchAsync(string endpoint, object content, string token = null);

        IHttpService AddHeader(string name, string value);
        IHttpService RemoveHeader(string name);
        IHttpService SetTokenHeaders(string token);
        void ClearHeaders();
    }

    public class HttpService : IHttpService
    {
        private readonly HttpClient client;
        private readonly IJsonService jsonService;
        private readonly HttpSettings httpSettings;

        public HttpService(IJsonService jsonService, HttpSettings httpSettings)
        {
            this.client = new HttpClient();
            this.Configure(httpSettings);

            this.jsonService = jsonService;
            this.httpSettings = httpSettings;
        }

        ~HttpService()
        {
            client.Dispose();
        }

        private void Configure(HttpSettings settings)
        {
            this.client.Timeout = TimeSpan.FromMilliseconds(settings.Timeout);
            this.SetCacheHeaders(settings.AllowCache ? null : "no-cache");
        }

        public IHttpService AddHeader(string name, string value)
        {
            this.SetHeader(name, value);
            return this;
        }

        public IHttpService RemoveHeader(string name)
        {
            this.SetHeader(name, null);
            return this;
        }

        public IHttpService SetTokenHeaders(string token)
        {
            string headerValue = null;
            if (!string.IsNullOrWhiteSpace(token) && !token.ToLower().StartsWith("bearer"))
            {
                headerValue = $"Bearer {token}";
            }
            this.SetHeader("Authorization", headerValue);
            return this;
        }


        public async Task<T> GetAsync<T>(string endpoint, string token = null) where T : class
        {
            this.SetTokenHeaders(token);

            var response = await this.client.GetAsync(endpoint);

            this.ClearHeaders();
            await EnsureSuccess(response);

            var json = await response.Content.ReadAsStringAsync();
            return this.ConvertResponseToType<T>(json);
        }

        public async Task<Stream> GetStreamAsync(string endpoint, string token = null)
        {
            this.SetTokenHeaders(token);

            var response = await this.client.GetAsync(endpoint);

            this.ClearHeaders();
            await EnsureSuccess(response);

            var stream = await response.Content.ReadAsStreamAsync();
            return stream;
        }

        public async Task<T> DeleteAsync<T>(string endpoint, string token = null) where T : class
        {
            this.SetTokenHeaders(token);

            var response = await this.client.DeleteAsync(endpoint);

            this.ClearHeaders();
            await EnsureSuccess(response);

            var json = await response.Content.ReadAsStringAsync();
            return this.ConvertResponseToType<T>(json);
        }

        public async Task DeleteAsync(string endpoint, string token = null)
        {
            this.SetTokenHeaders(token);

            var response = await this.client.DeleteAsync(endpoint);

            this.ClearHeaders();
            await EnsureSuccess(response);
        }

        public async Task<T> PutAsync<T>(string endpoint, object content, string token = null) where T : class
        {
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {

                var response = await this.client.PutAsync(endpoint, jsonContent);

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
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {

                var response = await this.client.PutAsync(endpoint, jsonContent);

                this.ClearHeaders();
                await EnsureSuccess(response);
            }
        }

        public async Task<T> PutStreamAsync<T>(string endpoint, Stream content, string token = null) where T : class
        {
            this.SetTokenHeaders(token);

            using (var streamContent = new StreamContent(content))
            {
                var response = await this.client.PutAsync(endpoint, streamContent);

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
            this.SetTokenHeaders(token);

            using (var streamContent = new StreamContent(content))
            {
                var response = await this.client.PutAsync(endpoint, streamContent);

                this.ClearHeaders();
                await EnsureSuccess(response);
            }
        }

        public async Task<Stream> PutRequestStreamAsync(string endpoint, object content, string token = null)
        {
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var response = await this.client.PutAsync(endpoint, jsonContent);

                this.ClearHeaders();
                await EnsureSuccess(response);

                var stream = await response.Content.ReadAsStreamAsync();
                return stream;
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object content, string token = null) where T : class
        {
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var response = await this.client.PostAsync(endpoint, jsonContent);

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
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var response = await this.client.PostAsync(endpoint, jsonContent);

                this.ClearHeaders();
                await EnsureSuccess(response);
            }
        }

        public async Task<T> PostStreamAsync<T>(string endpoint, Stream content, string token = null) where T : class
        {
            this.SetTokenHeaders(token);

            using (var streamContent = new StreamContent(content))
            {
                var response = await this.client.PostAsync(endpoint, streamContent);
                await EnsureSuccess(response);

                var json = await response.Content.ReadAsStringAsync();

                this.ClearHeaders();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return this.ConvertResponseToType<T>(json);
                }

                return null;
            }
        }

        public async Task PostStreamAsync(string endpoint, Stream content, string token = null)
        {
            this.SetTokenHeaders(token);

            using (var streamContent = new StreamContent(content))
            {
                var response = await this.client.PostAsync(endpoint, streamContent);

                this.ClearHeaders();
                await EnsureSuccess(response);
            }
        }

        public async Task<Stream> PostRequestStreamAsync(string endpoint, object content, string token = null)
        {
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var response = await this.client.PostAsync(endpoint, jsonContent);

                this.ClearHeaders();
                await EnsureSuccess(response);

                var stream = await response.Content.ReadAsStreamAsync();
                return stream;
            }
        }

        public async Task<T> PostFormAsync<T>(string endpoint, IDictionary<string, string> content, string token = null) where T : class
        {
            this.SetTokenHeaders(token);

            using (var formContent = new FormUrlEncodedContent(content))
            {
                var response = await this.client.PostAsync(endpoint, formContent);

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
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {

                var method = new HttpMethod("PATCH");
                var request = new HttpRequestMessage(method, endpoint)
                {
                    Content = jsonContent
                };

                HttpResponseMessage response = new HttpResponseMessage();
                response = await this.client.SendAsync(request);

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
            this.SetTokenHeaders(token);

            var payload = this.jsonService.Serialize(content);
            using (var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var method = new HttpMethod("PATCH");
                var request = new HttpRequestMessage(method, endpoint)
                {
                    Content = jsonContent
                };

                HttpResponseMessage response = await this.client.SendAsync(request);

                this.ClearHeaders();
                await EnsureSuccess(response);
            }
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

        private T ConvertResponseToType<T>(string json)
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(json))
                //{
                //    throw new Exception("Response json is empty");
                //}
                var data = this.jsonService.Deserialize<T>(json);
                return data;
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Could not convert response to {typeof(T).Name}");
            }
        }

        private async Task EnsureSuccess(HttpResponseMessage response)
        {
            if (response == null) throw new Exception("Invalid Http Response");

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new HttpListenerException((int)response.StatusCode, await response.RequestMessage.Content.ReadAsStringAsync());
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
