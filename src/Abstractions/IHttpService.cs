using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Onbox.Abstractions.V7
{
    public interface IHttpService : IDisposable
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
    }
}
