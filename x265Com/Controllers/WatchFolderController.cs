using System.Configuration;
using System.Web.Mvc;
using x265Com.Models;
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
        [HttpGet]
        public ActionResult _BundleConversion()
        {
            return PartialView("_BundleConversion");
        }
        [HttpPost]
        public ActionResult _BundleConversion(ConversionModel _ConversionModel)
        {
            int _exitCode=1;
            System.TimeSpan _Elapsed = new System.TimeSpan();
            string _message=string.Empty;

            WatchFolderModel _WatchFolderModel = WatchFolder.IniWatchFolder();
            if (_WatchFolderModel.FileModels==null || _WatchFolderModel.FileModels.Count==0)
            {
                _ConversionModel.isOver = true;
                _ConversionModel.ExitCode = 1;
                _ConversionModel.Elapsed = System.TimeSpan.FromMinutes(0);
                return PartialView("_BundleConversion", _ConversionModel);
            }
            foreach (FileModel _FM in _WatchFolderModel.FileModels)
            {
                string _WatchFolderPath = ConfigurationManager.AppSettings["WatchFolderPath"].ToString();
                string _WatchFolderOutpurPath = ConfigurationManager.AppSettings["WatchFolderOutputPath"].ToString();
                ConversionInfo _ConversionInfo = new ConversionInfo()
                {
                    InFileName = _FM.FileName,
                    OutFileName = _FM.FileName,
                    InFilePath = _WatchFolderPath,
                    OutFilePath = _WatchFolderOutpurPath,
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
                bool _isSuccess = _ConversionInfo.StartConversion(out _exitCode, out _Elapsed, out _message);
                if(!_isSuccess)
                {
                    _ConversionModel.isOver = true;
                    _ConversionModel.ExitCode = _exitCode;
                    _ConversionModel.CMDconsoleMessage = "Fichier : "+_FM.FileName + " erreur. " +_message;
                    _ConversionModel.Elapsed = _Elapsed;
                    return PartialView("_BundleConversion", _ConversionModel);
                }
            }
            _ConversionModel.isOver = true;
            _ConversionModel.ExitCode = _exitCode;
            _ConversionModel.CMDconsoleMessage = _message;
            _ConversionModel.Elapsed = _Elapsed;
            return PartialView("_BundleConversion", _ConversionModel);
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