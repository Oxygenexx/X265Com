using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using x265Com.ScriptCS;

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
            ViewBag.Message = "This is the Monitor Page.";

            return View();
        }
        [HttpGet]
        public ActionResult _ConversionPartial()
        {
            return PartialView("_ConversionPartial");
        }
        [HttpPost]
        public ActionResult _ConversionPartial(Models.ConversionModel _ConversionModel)
        {
            
            if (string.IsNullOrEmpty(_ConversionModel.InputFilePathAndName) || string.IsNullOrEmpty(_ConversionModel.OutputFilePathAndName))
            {
                if(string.IsNullOrEmpty(_ConversionModel.InputFilePathAndName))
                    _ConversionModel.CMDconsoleMessage = "Entrez un nom de fichier d'entré";
                if (string.IsNullOrEmpty(_ConversionModel.OutputFilePathAndName))
                    _ConversionModel.CMDconsoleMessage = "Entrez un nom de fichier de sortie";
                _ConversionModel.isOver = true;
                _ConversionModel.ExitCode = 1;
                _ConversionModel.Elapsed = System.TimeSpan.FromMinutes(0);
                return PartialView("_ConversionPartial", _ConversionModel);
            }
            string _InputFileName = "";
            string _InputFilepath = "";
            int index = _ConversionModel.InputFilePathAndName.LastIndexOf(@"\");
            _InputFileName = Tools.GetFileName(_ConversionModel.InputFilePathAndName, index);
            _InputFilepath = Tools.GetFilePath(_ConversionModel.InputFilePathAndName, index);
            string _OutputFileName = "";
            string _OutputFilepath = "";
            index = _ConversionModel.OutputFilePathAndName.LastIndexOf(@"\");
            _OutputFileName = Tools.GetFileName(_ConversionModel.OutputFilePathAndName, index);
            _OutputFilepath = Tools.GetFilePath(_ConversionModel.OutputFilePathAndName, index);

            ConversionInfo _ConversionInfo = new ConversionInfo()
            {
                InFilePath = _InputFilepath,
                InFileName = _InputFileName,
                OutFilePath = _OutputFilepath,
                OutFileName = _OutputFileName,
                cadenceImage = (cadenceImageEnum)_ConversionModel.cadenceImage,
                VideoCodec = (videoCodecEnum)_ConversionModel.VideoCodec,
                DefImage = (defImageEnum)_ConversionModel.DefImage,
                perfOption = (perfOptionEnum)_ConversionModel.perfOption,
                AudioCodec = (audioCodecEnum)_ConversionModel.AudioCodec,
                debitAudio = _ConversionModel.debitAudio,
                debitVideo = _ConversionModel.debitVideo,
                quantizerParameter = _ConversionModel.quantizerParameter,
                isQP = _ConversionModel.isQP,
                isLossless = _ConversionModel.isLossless,
                isWpp = _ConversionModel.isWpp
            };

            int _exitCode;
            System.TimeSpan _Elapsed = new System.TimeSpan();
            string _message;
            _ConversionModel.isSuccess = _ConversionInfo.StartConversion(out _exitCode, out _Elapsed, out _message);
            _ConversionModel.isOver = true;
            _ConversionModel.ExitCode = _exitCode;            
            _ConversionModel.CMDconsoleMessage = _message;
            _ConversionModel.Elapsed = _Elapsed;
            return PartialView("_ConversionPartial", _ConversionModel);
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