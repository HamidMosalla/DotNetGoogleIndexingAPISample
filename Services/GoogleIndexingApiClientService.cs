using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleIndexingAPIMVC.Services
{
    public class GoogleIndexingApiClientService : BaseClientService
    {
        public GoogleIndexingApiClientService() : this(new BaseClientService.Initializer()) { }

        public GoogleIndexingApiClientService(Initializer initializer) : base(initializer)
        {
        }

        public override string Name => "Batch Client Service";

        public override string BaseUri => "https://indexing.googleapis.com/batch";

        public override string BasePath => "batch/";

        public override string BatchUri
        {
            get { return "https://www.googleapis.com/batch"; }
        }

        public override string BatchPath
        {
            get { return "batch"; }
        }

        public override IList<string> Features
        {
            get { return new[] { "publish", "update" }; }
        }
    }
}