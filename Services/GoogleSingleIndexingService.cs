namespace GoogleIndexingAPIMVC.Services
{
    using System.Threading.Tasks;
    using System.Web;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Indexing.v3;
    using Google.Apis.Indexing.v3.Data;
    using Google.Apis.Services;

    public class GoogleSingleIndexingService
    {
        private GoogleCredentialService _googleCredentialService;
        private GoogleCredential _googleCredential;

        public GoogleSingleIndexingService()
        {
            _googleCredentialService = new GoogleCredentialService();
            _googleCredential = _googleCredentialService.GetGoogleCredential();
        }

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
            var credential = _googleCredential.UnderlyingCredential;

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
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
            var credential = _googleCredential.UnderlyingCredential;

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
            });

            var metaDataRequest = new GetMetadataRequest(googleIndexingApiClientService, jobUrl);

            return metaDataRequest.ExecuteAsync();
        }
    }
}
