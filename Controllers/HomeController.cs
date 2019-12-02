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
            var googleSingleIndexingService = new GoogleSingleIndexingService();
            var googleBatchIndexingService = new GoogleBatchIndexingService();

            var updateResult = await googleSingleIndexingService.AddOrUpdateJob(@"http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/");
            // var deleteResult = await googleSingleIndexingService.CloseJob(@"http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/");
            var status = await googleSingleIndexingService.GetIndexingStatus(@"http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/");

            var urls = new[]
            {
                "http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/",
                 "http://hamidmosalla.com/2019/11/25/an-upcoming-series-of-blog-posts-about-xunit/"
            };

            var batchUpdateResult = await googleBatchIndexingService.AddOrUpdateBatchJobs(urls);
            // var batchCloseResult = await googleBatchIndexingService.CloseBatchJobs(urls);
            var batchStatusResult = await googleBatchIndexingService.GetBatchJobsStatus(urls);

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