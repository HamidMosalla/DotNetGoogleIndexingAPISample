using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Indexing.v3;
using Google.Apis.Services;

namespace GoogleIndexingAPIMVC.Services
{
    public class GetMetadataRequest : UrlNotificationsResource.GetMetadataRequest
    {
        public GetMetadataRequest(IClientService service, string url) : base(service)
        {
            this.Url = url;
        }
    }
}