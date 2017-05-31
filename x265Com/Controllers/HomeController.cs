using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace x265Com.Controllers
{
    public class HomeController : Controller
    {
        private const string TempPath = @"G:\Documents\Visual\Depot";

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Monitor()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region UploadFiles
        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (HttpPostedFileBase file in files)
            {
                string filePath = Path.Combine(TempPath, file.FileName);
                System.IO.File.WriteAllBytes(filePath, ReadData(file.InputStream));
            }

            return Json("All files have been successfully stored.");
        }

        private byte[] ReadData(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
        #endregion
    }
}
/*

public class HomeController : Controller
{
    private const string TempPath = @"C:\Temp";

    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
    {
        foreach (HttpPostedFileBase file in files)
        {
            string filePath = Path.Combine(TempPath, file.FileName);
            System.IO.File.WriteAllBytes(filePath, ReadData(file.InputStream));
        }

        return Json("All files have been successfully stored.");
    }

    private byte[] ReadData(Stream stream)
    {
        byte[] buffer = new byte[16 * 1024];

        using (MemoryStream ms = new MemoryStream())
        {
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            return ms.ToArray();
        }
    }
}
*/