using System.Web.Mvc;
using x265Com.ScriptCS;

namespace x265Com.Controllers
{
    public class WatchFolderController : Controller
    {
        public ActionResult Index()
        {
            var model = WatchFolder.IniWatchFolder();
            return View(model);
        }        
        /*
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
       */
    }
}