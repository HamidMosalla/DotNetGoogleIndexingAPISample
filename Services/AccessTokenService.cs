using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace GoogleIndexingAPI.Services
{
    public class GoogleIndexingApiService
    {
        public async Task<string> GetAccessTokenWithJsonPrivateKey()
        {
            var privateKeyStream = File.OpenRead(@"C:\Users\hmosallanejad\Desktop\New folder\hamidmosalla-83e0e56ac909.json");

            var serviceAccountCredential = ServiceAccountCredential.FromServiceAccountData(privateKeyStream);

            var googleCredetial = GoogleCredential.FromServiceAccountCredential(serviceAccountCredential);

            var result = await googleCredetial.UnderlyingCredential.GetAccessTokenForRequestAsync("https://www.googleapis.com/auth/indexing");

            var request = new BatchRequest(new GoogleIndexingService());

            return result;
        }

        public async Task<string> GetAccessTokenWithP12PrivateKey()
        {
            var serviceAccountEmail = "hamidmosalla3@hamidmosalla.iam.gserviceaccount.com";

            var certificate = new X509Certificate2(@"C:\Users\hmosallanejad\Desktop\New folder\hamidmosalla-8ba5b4d3d59a.p12",
                "notasecret", X509KeyStorageFlags.Exportable);

            var serviceAccountCredential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    .FromCertificate(certificate));

            var googleCredetial = GoogleCredential.FromServiceAccountCredential(serviceAccountCredential);

            var result = await googleCredetial.UnderlyingCredential.GetAccessTokenForRequestAsync("https://www.googleapis.com/auth/indexing");

            return result;
        }

        public async Task<HttpStatusCode> SendIndexUpdateRequest(string url)
        {
            // 


            return await Task.FromResult(HttpStatusCode.OK);
        }

        public async Task<HttpStatusCode> GetNotificationStatus()
        {
            var client = new RestClient("https://indexing.googleapis.com/v3/urlNotifications/metadata");
            var request = new RestRequest(Method.GET);
            var accessToken = GetAccessTokenWithJsonPrivateKey();
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("key", "AIzaSyAtVs5Fje6k8reFgAqkswERrYKGuhBFsQw");
            IRestResponse response = client.Execute(request);

            return await Task.FromResult(response.StatusCode);
        }
    }
}