using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
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
        private HostingEnvironment HostingEnvironment => new HostingEnvironment();

        #region Sigular Requests

        public async Task<PublishUrlNotificationResponse> AddOrUpdateJob(string jobUrl)
        {
            return await AddUpdateJobGoogleIndexing(jobUrl, "URL_UPDATED");
        }

        public async Task<PublishUrlNotificationResponse> CloseJob(string jobUrl)
        {
            return await AddUpdateJobGoogleIndexing(jobUrl, "URL_DELETED");
        }

        public async Task<UrlNotificationMetadata> GetIndexingStatus(string jobUrl)
        {
            return await GetJobIndexStatusFromGoogle(jobUrl);
        }

        private Task<PublishUrlNotificationResponse> AddUpdateJobGoogleIndexing(string jobUrl, string action)
        {
            var serviceAccountCredential = (ServiceAccountCredential)GetGoogleCredential().UnderlyingCredential;

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = serviceAccountCredential
            });

            var requestBody = new UrlNotification
            {
                Url = jobUrl,
                Type = action
            };

            var publishRequest = new UrlNotificationsResource.PublishRequest(googleIndexingApiClientService, requestBody);

            return publishRequest.ExecuteAsync();
        }

        private Task<UrlNotificationMetadata> GetJobIndexStatusFromGoogle(string jobUrl)
        {
            var serviceAccountCredential = (ServiceAccountCredential)GetGoogleCredential().UnderlyingCredential;

            string googleApiUrl = $"https://indexing.googleapis.com/v3/urlNotifications/metadata?url={HttpUtility.UrlEncode(jobUrl)}";

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = serviceAccountCredential
            });

            var metaDataRequest = new GetMetadataRequest(googleIndexingApiClientService, jobUrl);

            return metaDataRequest.ExecuteAsync();
        }

        #endregion

        #region Group Requests

        public async Task<List<PublishUrlNotificationResponse>> AddOrUpdateBatchJobs(IEnumerable<string> jobUrls)
        {
            return await AddUpdateBatchJobGoogleIndexing(jobUrls, "URL_UPDATED");
        }

        public async Task<List<PublishUrlNotificationResponse>> CloseBatchJobs(IEnumerable<string> jobUrls)
        {
            return await AddUpdateBatchJobGoogleIndexing(jobUrls, "URL_DELETED");
        }

        public async Task<List<UrlNotificationMetadata>> GetBatchJobsStatus(IEnumerable<string> jobUrls)
        {
            return await GetBatchJobsIndexingStatusFromGoogle(jobUrls);
        }

        private async Task<List<PublishUrlNotificationResponse>> AddUpdateBatchJobGoogleIndexing(IEnumerable<string> jobUrls, string action)
        {
            var serviceAccountCredential = (ServiceAccountCredential)GetGoogleCredential().UnderlyingCredential;

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = serviceAccountCredential
            });

            var request = new BatchRequest(googleIndexingApiClientService);

            var notificationResponses = new List<PublishUrlNotificationResponse>();

            foreach (var url in jobUrls)
            {
                var urlNotification = new UrlNotification
                {
                    Url = url,
                    Type = action
                };

                request.Queue<PublishUrlNotificationResponse>(
                    new UrlNotificationsResource.PublishRequest(googleIndexingApiClientService, urlNotification), (response, error, i, message) =>
                    {
                        notificationResponses.Add(response);
                    });
            }

            await request.ExecuteAsync();

            return await Task.FromResult(notificationResponses);
        }

        private async Task<List<UrlNotificationMetadata>> GetBatchJobsIndexingStatusFromGoogle(IEnumerable<string> jobUrls)
        {
            var serviceAccountCredential = (ServiceAccountCredential)GetGoogleCredential().UnderlyingCredential;

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = serviceAccountCredential
            });

            var request = new BatchRequest(googleIndexingApiClientService);

            var metaDataResponses = new List<UrlNotificationMetadata>();

            foreach (var url in jobUrls)
            {
                request.Queue<UrlNotificationMetadata>(
                    new GetMetadataRequest(googleIndexingApiClientService, url), (response, error, i, message) =>
                    {
                        metaDataResponses.Add(response);
                    });
            }

            await request.ExecuteAsync();

            return await Task.FromResult(metaDataResponses);
        }

        #endregion

        private GoogleCredential GetGoogleCredential()
        {
            var path = HostingEnvironment.MapPath("/PrivateKey/hamidmosalla-28d08becf0a7.json");

            GoogleCredential credential;

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[] { "https://www.googleapis.com/auth/indexing" });
            }

            return credential;
        }
    }
}