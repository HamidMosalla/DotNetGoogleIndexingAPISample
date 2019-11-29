using Google.Apis.Discovery;
using Google.Apis.Requests;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleIndexingAPIMVC.Services
{
    public class IndexingApiBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
    {
        public IndexingApiBaseServiceRequest(IClientService service, object body) : base(service)
        {
            Body = body;
            InitParameters();
        }

        public object Body { get; private set; }

        public override string RestPath => "batch";

        public override string MethodName => "create";

        public override string HttpMethod => "POST";

        protected override object GetBody() { return Body; }

        protected override void InitParameters()
        {
            base.InitParameters();
        }
    }

}