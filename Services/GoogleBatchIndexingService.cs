namespace GoogleIndexingAPIMVC.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Indexing.v3;
    using Google.Apis.Indexing.v3.Data;
    using Google.Apis.Requests;
    using Google.Apis.Services;

    public class GoogleBatchIndexingService
    {
        private GoogleCredentialService _googleCredentialService;
        private GoogleCredential _googleCredential;

        public GoogleBatchIndexingService()
        {
            _googleCredentialService = new GoogleCredentialService();
            _googleCredential = _googleCredentialService.GetGoogleCredential();
        }

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
            var credential = _googleCredential.UnderlyingCredential;

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
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
            var credential = _googleCredential.UnderlyingCredential;

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
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
    }
}
