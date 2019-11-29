using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Requests;
using Google.Apis.Services;
using GoogleIndexingAPIMVC.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace GoogleIndexingAPI.Services
{
    public class GoogleIndexingApiService
    {
        //public GoogleServiceAccount _googleServiceAccount;
        //private IConfigHelper _configHelper;
        //private GoogleCredential _googleCredential;
        //private IHostingEnvironment _hostingEnvironment;

        //public Indexing(IConfigHelper configHelper, IHostingEnvironment hostingEnvironment)
        //{
        //    _configHelper = configHelper;
        //    _hostingEnvironment = hostingEnvironment;
        //    _googleServiceAccount = _configHelper.Settings.GoogleServiceAccounts.SingleOrDefault(a => a.Name == "Indexing");
        //    _googleCredential = GetGoogleCredential();
        //}

        public async Task<HttpResponseMessage> AddOrUpdateJob(string jobUrl)
        {
            return await PostJobToGoogle(jobUrl, "URL_UPDATED");
        }

        public async Task<HttpResponseMessage> CloseJob(string jobUrl)
        {
            return await PostJobToGoogle(jobUrl, "URL_DELETED");
        }

        public async Task<HttpResponseMessage> GetIndexingStatus(string jobUrl)
        {
            return await GetJobStatusFromGoogle(jobUrl);
        }

        private async Task<HttpResponseMessage> PostJobToGoogle(string jobUrl, string action)
        {
            var serviceAccountCredential = (ServiceAccountCredential)GetGoogleCredential().UnderlyingCredential;

            string googleApiUrl = "https://indexing.googleapis.com/v3/urlNotifications:publish";

            var requestBody = new
            {
                url = jobUrl,
                type = action
            };

            var httpClientHandler = new HttpClientHandler();

            var configurableMessageHandler = new ConfigurableMessageHandler(httpClientHandler);

            var configurableHttpClient = new ConfigurableHttpClient(configurableMessageHandler);

            serviceAccountCredential.Initialize(configurableHttpClient);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await configurableHttpClient.PostAsync(new Uri(googleApiUrl), content);

            var responseBody = await response.Content.ReadAsStringAsync();

            return response;
        }

        private async Task<HttpResponseMessage> GetJobStatusFromGoogle(string jobUrl)
        {
            var serviceAccountCredential = (ServiceAccountCredential)GetGoogleCredential().UnderlyingCredential;

            string googleApiUrl = $"https://indexing.googleapis.com/v3/urlNotifications/metadata?url={HttpUtility.UrlEncode(jobUrl)}";

            var httpClientHandler = new HttpClientHandler();

            var configurableMessageHandler = new ConfigurableMessageHandler(httpClientHandler);

            var configurableHttpClient = new ConfigurableHttpClient(configurableMessageHandler);

            serviceAccountCredential.Initialize(configurableHttpClient);

            var response = await configurableHttpClient.GetAsync(new Uri(googleApiUrl));

            var responseBody = await response.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<HttpResponseMessage> PostBatchJobsToGoogle(IEnumerable<string> jobUrls, string action)
        {
            var serviceAccountCredential = (ServiceAccountCredential)GetGoogleCredential().UnderlyingCredential;

            string googleApiUrl = "https://indexing.googleapis.com/batch";

            var googleIndexingApiClientService = new GoogleIndexingApiClientService(new BaseClientService.Initializer
            {
                HttpClientInitializer = serviceAccountCredential
            });

            var request = new BatchRequest(googleIndexingApiClientService, googleApiUrl);

            //foreach (var item in jobUrls) { }

            request.Queue<string>(
                new IndexingApiBaseServiceRequest<string>(googleIndexingApiClientService,
                new
                {
                    url = "http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/",
                    type = "URL_UPDATED"
                }), async (content, error, i, message) =>
                 {
                     var con = content;

                     var errormessage = await message.Content.ReadAsStringAsync();

                 });


            await request.ExecuteAsync();


            return null;
        }

        private GoogleCredential GetGoogleCredential()
        {
            // var path = _hostingEnvironment.MapPath(_googleServiceAccount.KeyFile);

            GoogleCredential credential;

            using (var stream = new FileStream(@"C:\Users\Hamid\Desktop\hamidmosalla-258eabd6f142.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[] { "https://www.googleapis.com/auth/indexing" });
            }

            return credential;
        }
    }
}