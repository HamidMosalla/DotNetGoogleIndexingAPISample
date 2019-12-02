using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Indexing.v3.Data;
using GoogleIndexingAPIMVC.Services;

namespace GoogleIndexingAPIMVC.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var googleService = new GoogleIndexingApiService();

            // var updateResult = await googleService.AddOrUpdateJob(@"http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/");
            // var deleteResult = await googleService.CloseJob(@"http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/");
            // var status = await googleService.GetIndexingStatus(@"http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/");

            var urls = new[]
            {
                "http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/",
                 "http://hamidmosalla.com/2019/11/25/an-upcoming-series-of-blog-posts-about-xunit/"
            };

            var batchUpdateResult = await googleService.AddOrUpdateBatchJobs(urls);
            // var batchCloseResult = await googleService.CloseBatchJobs(urls);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}