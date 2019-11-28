using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis;
using Google.Apis.Http;
using Google.Apis.Requests;
using Google.Apis.Services;

namespace GoogleIndexingAPI.Services
{
    public class GoogleIndexingService : IClientService
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void SetRequestSerailizedContent(HttpRequestMessage request, object body)
        {
            throw new System.NotImplementedException();
        }

        public string SerializeObject(object data)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            throw new System.NotImplementedException();
        }

        public Task<RequestError> DeserializeError(HttpResponseMessage response)
        {
            throw new System.NotImplementedException();
        }

        public ConfigurableHttpClient HttpClient { get; }
        public IConfigurableHttpClientInitializer HttpClientInitializer { get; }
        public string Name { get; }
        public string BaseUri { get; }
        public string BasePath { get; }
        public IList<string> Features { get; }
        public bool GZipEnabled { get; }
        public string ApiKey { get; }
        public string ApplicationName { get; }
        public ISerializer Serializer { get; }
    }
}