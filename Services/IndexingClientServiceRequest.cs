using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Discovery;
using Google.Apis.Requests;
using Google.Apis.Services;

namespace GoogleIndexingAPI.Services
{
    public class IndexingClientServiceRequest : IClientServiceRequest
    {
        public HttpRequestMessage CreateRequest(bool? overrideGZipEnabled = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<Stream> ExecuteAsStreamAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Stream> ExecuteAsStreamAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Stream ExecuteAsStream()
        {
            throw new System.NotImplementedException();
        }

        public string MethodName { get; }
        public string RestPath { get; }
        public string HttpMethod { get; }
        public IDictionary<string, IParameter> RequestParameters { get; }
        public IClientService Service { get; }
    }
}