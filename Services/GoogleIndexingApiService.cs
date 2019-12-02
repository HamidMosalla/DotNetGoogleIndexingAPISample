using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Indexing.v3;
using Google.Apis.Indexing.v3.Data;
using Google.Apis.Requests;
using Google.Apis.Services;
using Newtonsoft.Json;

namespace GoogleIndexingAPIMVC.Services
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

        #region Sigular Request

        public async Task<HttpResponseMessage> AddOrUpdateJob(string jobUrl)
        {
            return await AddUpdateJobGoogleIndexing(jobUrl, "URL_UPDATED");
        }

        public async Task<HttpResponseMessage> CloseJob(string jobUrl)
        {
            return await AddUpdateJobGoogleIndexing(jobUrl, "URL_DELETED");
        }

        public async Task<HttpResponseMessage> GetIndexingStatus(string jobUrl)
        {
            return await GetJobIndexStatusFromGoogle(jobUrl);
        }

        private async Task<HttpResponseMessage> AddUpdateJobGoogleIndexing(string jobUrl, string action)
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

        private async Task<HttpResponseMessage> GetJobIndexStatusFromGoogle(string jobUrl)
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

        #endregion

        #region Group Request

        public async Task AddOrUpdateBatchJobs(IEnumerable<string> jobUrls)
        {
            await AddUpdateBatchJobGoogleIndexing(jobUrls, "URL_UPDATED");
        }

        public async Task CloseBatchJobs(IEnumerable<string> jobUrls)
        {
            await AddUpdateBatchJobGoogleIndexing(jobUrls, "URL_DELETED");
        }

        public async Task GetBatchJobsStatus(IEnumerable<string> jobUrls)
        {
            await GetBatchJobsIndexingStatusFromGoogle(jobUrls);
        }

        public async Task AddUpdateBatchJobGoogleIndexing(IEnumerable<string> jobUrls, string action)
        {
            var serviceAccountCredential = (ServiceAccountCredential)GetGoogleCredential().UnderlyingCredential;

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = serviceAccountCredential
            });

            var request = new BatchRequest(googleIndexingApiClientService);

            foreach (var url in jobUrls)
            {
                var urlNotification = new UrlNotification
                {
                    Url = url,
                    Type = action
                };

                request.Queue<PublishUrlNotificationResponse>(
                    new UrlNotificationsResource.PublishRequest(googleIndexingApiClientService, urlNotification),
                    async (content, error, i, message) =>
                    {
                        var con = content;

                        var errormessage = await message.Content.ReadAsStringAsync();
                    });
            }

            await request.ExecuteAsync();
        }

        public async Task GetBatchJobsIndexingStatusFromGoogle(IEnumerable<string> jobUrls)
        {
            var serviceAccountCredential = (ServiceAccountCredential)GetGoogleCredential().UnderlyingCredential;

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = serviceAccountCredential
            });

            var request = new BatchRequest(googleIndexingApiClientService);

            foreach (var url in jobUrls)
            {
                request.Queue<PublishUrlNotificationResponse>(
                    new GetMetadataRequest(googleIndexingApiClientService, url),
                    async (content, error, i, message) =>
                    {
                        var con = content;

                        var errormessage = await message.Content.ReadAsStringAsync();
                    });
            }

            await request.ExecuteAsync();
        }

        #endregion

        private GoogleCredential GetGoogleCredential()
        {
            // var path = _hostingEnvironment.MapPath(_googleServiceAccount.KeyFile);

            GoogleCredential credential;

            using (var stream = new FileStream(@"C:\Users\hmosallanejad\Downloads\hamidmosalla-28d08becf0a7.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[] { "https://www.googleapis.com/auth/indexing" });
            }

            return credential;
        }
    }
}