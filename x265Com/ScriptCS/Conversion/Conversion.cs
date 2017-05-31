using System;
using System.Text;
using System.Text.RegularExpressions;

namespace x265Com.ScriptCS
{
    public class ConversionInfo
    {
        public string InFilePath { get; set; }
        public string InFileName { get; set; }
        public string OutFilePath { get; set; }
        public string OutFileName { get; set; }
        public int cadenceImage { get; set; }
        public bool isWpp { get; set; }
        public enum defImage { UHD_3840x2160, HD_1920x1080, HD_1440x1080, HD_1270x720, Proxy_480p, Proxy_360p, Proxy_240p }
        public int DefImage { get; set; }
        public enum perfOption { placebo, veryslow, slower, slow, medium, fast, faster, veryfast, superfast, ultrafast }
        public int PerfOption { get; set; }
        public int CTU { get; set; }
        public enum conteneur { mov, mxf, mp4 }
        public int Conteneur { get; set; }
        public enum videoCodec { HEVC, h264, Mpeg2, vp9 }
        public int VideoCodec { get; set; }
        public int debitVideo { get; set; }
        public int tailleGop { get; set; }
        public bool isQP { get; set; }
        public enum audioCodec { mp3, wmv, pcm, aac }
        public int AudioCodec { get; set; }
        public int debitAudio { get; set; }

        public bool StartConversion()
        {
            bool _isSuccess = false;
            string _CMDConversionStr = string.Empty;
            //InFilePath = @Tools.StripPathOfDoubleSlashes(InFilePath);
            //OutFilePath = @Tools.StripPathOfDoubleSlashes(OutFilePath);
            //InFilePath = Regex.Unescape(InFilePath);
            //OutFilePath = Regex.Unescape(OutFilePath);
            _CMDConversionStr = this.BuildConversionString();
            if (string.IsNullOrEmpty(_CMDConversionStr))
                return _isSuccess;
            DateTime _Begin = DateTime.Now;
            LaunchCMDCommand(_CMDConversionStr);
            TimeSpan _Elapsed = _Begin - DateTime.Now;            
            //catch (Exception e)
            //{
            //    Tools.WriteErrorInXml(e.Message);
            //    return _isSuccess;
            //}
            _isSuccess = true;
            return _isSuccess;
        }
        //G:\Documents\Visual\WatchFolder\Vikings_S01E01_VOSTFR_HDTV_XviD.avi
        //G:\Documents\Visual\WatchFolder\sortieTest.avi
        //    ffmpeg -i G:\Documents\Visual\WatchFolder\Vikings_S01E01_VOSTFR_HDTV_XviD.avi -b:v 64k -vf scale=1270:720 G:\Documents\Visual\WatchFolder\sortieTest.avi
        public string BuildConversionString()
        {
            //Example Line : ffmpeg -i input.avi -b:v 64k -bufsize 64k output.avi            
            StringBuilder _cmdStringBuilder = new StringBuilder("ffmpeg -i " + InFilePath + @"\" + InFileName + " ");
            cadenceImage = cadenceImage == 0 ? 1 : cadenceImage;
            _cmdStringBuilder.Append("-r " + cadenceImage + " ");
            this.getResolutionCommandString();
            //if (isWpp)
            //{
            //    _cmdStringBuilder.Append("--wpp ");
            //}
            //else
            //{
            //    _cmdStringBuilder.Append("--no-wpp ");
            //}
            _cmdStringBuilder.Append(OutFilePath + @"\" + OutFileName);

            return _cmdStringBuilder.ToString();
        }
        public static void LaunchCMDCommand(string _cmdstring)
        {
            //_cmdstring = "/C " + _cmdstring;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = _cmdstring;
            process.StartInfo = startInfo;
            process.Start();
        }
        public string getResolutionCommandString()
        {
            string _resolutionCMDLine = "-vf scale=";
            switch (DefImage)
            {
                //{ UHD_3840:2160, HD_1920:1080, HD_1440:1080, HD_1270:720, Pro:y_480p, Pro:y_360p, Pro:y_240p }
                case (int)defImage.UHD_3840x2160:
                    {
                        _resolutionCMDLine += "3840:2160 ";
                        break;
                    }
                case (int)defImage.HD_1920x1080:
                    {
                        _resolutionCMDLine += "1920:1080 ";
                        break;
                    }
                case (int)defImage.HD_1440x1080:
                    {
                        _resolutionCMDLine += "1440:1080 ";
                        break;
                    }
                case (int)defImage.HD_1270x720:
                    {
                        _resolutionCMDLine += "1270:720 ";
                        break;
                    }
                case (int)defImage.Proxy_480p:
                    {
                        _resolutionCMDLine += "854:480 ";
                        break;
                    }
                case (int)defImage.Proxy_360p:
                    {
                        _resolutionCMDLine += "640:360 ";
                        break;
                    }
                case (int)defImage.Proxy_240p:
                    {
                        _resolutionCMDLine += "426:240 ";
                        break;
                    }
                default:
                    {
                        _resolutionCMDLine += "1920:1080 ";
                        break;
                    }
            }
            return _resolutionCMDLine;
        }
    }
}