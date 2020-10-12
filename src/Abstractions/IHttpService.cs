using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Onbox.Abstractions.VDev
{
    /// <summary>
    /// Onbox's contract for Http requests.
    /// </summary>
    public interface IHttpService : IDisposable
    {
        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        Task<T> GetAsync<T>(string endpoint, string token = null) where T : class;
        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a stream in an asynchronous operation.
        /// </summary>
        Task<Stream> GetStreamAsync(string endpoint, string token = null);
        /// <summary>
        /// Send a DELETE request to the specified Uri and return the response body as an asynchronous operation.
        /// </summary>
        Task<T> DeleteAsync<T>(string endpoint, string token = null) where T : class;
        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        Task DeleteAsync(string endpoint, string token = null);
        /// <summary>
        /// Send a PUT request to the specified Uri and return the response body as an asynchronous operation.
        /// </summary>
        Task<T> PutAsync<T>(string endpoint, object content, string token = null) where T : class;
        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        Task PutAsync(string endpoint, object content, string token = null);
        /// <summary>
        /// Send a PUT stream request to the specified Uri and return the response body as an asynchronous operation.
        /// </summary>
        Task<T> PutStreamAsync<T>(string endpoint, Stream content, string token = null) where T : class;
        /// <summary>
        /// Send a PUT stream request to the specified Uri as an asynchronous operation.
        /// </summary>
        Task PutStreamAsync(string enpoint, Stream content, string token = null);
        /// <summary>
        /// Send a PUT request to the specified Uri and return the response body as a stream in an asynchronous operation.
        /// </summary>
        Task<Stream> PutRequestStreamAsync(string endpoint, object content, string token = null);
        /// <summary>
        /// Send a POST request to the specified Uri and return the response body as an asynchronous operation.
        /// </summary>
        Task<T> PostAsync<T>(string endpoint, object content, string token = null) where T : class;
        /// <summary>
        /// Send a POST form request to the specified Uri and return the response body as an asynchronous operation.
        /// </summary>
        Task<T> PostFormAsync<T>(string endpoint, IDictionary<string, string> content, string token = null) where T : class;
        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        Task PostAsync(string endpoint, object content, string token = null);
        /// <summary>
        /// Send a POST stream request to the specified Uri and return the response body as an asynchronous operation.
        /// </summary>
        Task<T> PostStreamAsync<T>(string endpoint, Stream content, string token = null) where T : class;
        /// <summary>
        /// Send a POST stream request to the specified Uri as an asynchronous operation.
        /// </summary>
        Task PostStreamAsync(string endpoint, Stream content, string token = null);
        /// <summary>
        /// Send a POST request to the specified Uri and return the response body as a stream in an asynchronous operation.
        /// </summary>
        Task<Stream> PostRequestStreamAsync(string endpoint, object content, string token = null);
        /// <summary>
        /// Send a PATCH request to the specified Uri and return the response body as an asynchronous operation.
        /// </summary>
        Task<T> PatchAsync<T>(string endpoint, object content, string token = null) where T : class;
        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        Task PatchAsync(string endpoint, object content, string token = null);
        /// <summary>
        /// Add a header for all subsequent requests
        /// </summary>
        IHttpService AddHeader(string name, string value);
        /// <summary>
        /// Clears all headers for all subsequent requests
        /// </summary>
        IHttpService ClearHeaders();
    }
}