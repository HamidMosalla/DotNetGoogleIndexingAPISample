using GoogleIndexingAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GoogleIndexingAPIMVC.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var googleService = new GoogleIndexingApiService();

            // var accessToken = await googleService.GetAccessTokenWithJsonPrivateKey();
            // var accessToken2 = await googleService.GetAccessTokenWithP12PrivateKey();
            // var notificationStatus = await googleService.GetNotificationStatus();

            // var updateResult = await googleService.AddOrUpdateJob(@"http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/");
            // var status = await googleService.GetIndexingStatus(@"http://hamidmosalla.com/2016/01/26/supercharge-your-text-editing-with-viasfora/");

            var batchResult = await googleService.PostBatchJobsToGoogle(null, null);

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