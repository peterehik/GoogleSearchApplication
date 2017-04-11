using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using GoogleSearch.Business;

namespace GoogleSearch.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetIndexes(string url, string keywords)
        {
            var testCollection = new List<string>();
            var results = new GoogleSearchService(url, keywords).GetSearchIndexes(testCollection);
            return Json(new
            {results = string.Join(", ", results), all = string.Join(Environment.NewLine, testCollection)});
        }
        
    }
}