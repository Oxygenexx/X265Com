using System;
using System.Diagnostics;
using System.Text;

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
        //public enum perfOption { placebo, veryslow, slower, slow, medium, fast, faster, veryfast, superfast, ultrafast }
        public enum perfOptionEnum { ultrafast, superfast, veryfast, faster, fast, medium, slow, slower, veryslow, placebo }
        public perfOptionEnum perfOption { get; set; }
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

        public bool StartConversion(out int _exitCode, out TimeSpan _Elapsed, out string _message)
        {
            _Elapsed = new TimeSpan();
            _exitCode = 0;
            _message = string.Empty;
            bool _isSuccess = false;
            string _CMDConversionStr = string.Empty;
            _CMDConversionStr = BuildConversionString();
            if (string.IsNullOrEmpty(_CMDConversionStr))
                return _isSuccess;
            DateTime _Begin = DateTime.Now;
            _exitCode = LaunchCMDCommand(_CMDConversionStr);
            _Elapsed = _Begin - DateTime.Now;            
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
            _cmdStringBuilder.Append(getResolutionCommandString());
            //if (isWpp)
            //{
            //    _cmdStringBuilder.Append("--wpp ");
            //}
            //else
            //{
            //    _cmdStringBuilder.Append("--no-wpp ");
            //}
            _cmdStringBuilder.Append(getPresetCommandString());

            _cmdStringBuilder.Append(OutFilePath + @"\" + OutFileName);

            return _cmdStringBuilder.ToString();
        }
        public static int LaunchCMDCommand(string _cmdstring)
        {
            _cmdstring = "/C " + _cmdstring;
            Process process = new Process();
            //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = _cmdstring;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.Start();
            process.BeginErrorReadLine();
            string output = process.StandardOutput.ReadToEnd();
            //Console.WriteLine(output);
            Tools.WriteErrorInXml(output);
            process.WaitForExit();
            return process.ExitCode;
        }

        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            Tools.WriteErrorInXml(outLine.Data);
            //Console.WriteLine(outLine.Data);
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
        public string getPresetCommandString()
        {
            
            string _presetCMDLine =  "-preset " + perfOption.ToString() + " ";
            return _presetCMDLine;
            //_presetCMDLine += perfOption.ToString();
            //_presetCMDLine += " ";

            //switch (PerfOption)
            //{
            //    //{ UHD_3840:2160, HD_1920:1080, HD_1440:1080, HD_1270:720, Pro:y_480p, Pro:y_360p, Pro:y_240p }
            //    case (int)perfOption.ultrafast:
            //        {
            //            _presetCMDLine += "3840:2160 ";
            //            break;
            //        }
            //    case (int)perfOption.veryfast:
            //        {
            //            _presetCMDLine += "1920:1080 ";
            //            break;
            //        }
            //    case (int)perfOption.superfast:
            //        {
            //            _presetCMDLine += "1440:1080 ";
            //            break;
            //        }
            //    case (int)perfOption.faster:
            //        {
            //            _presetCMDLine += "1270:720 ";
            //            break;
            //        }
            //    case (int)perfOption.fast:
            //        {
            //            _presetCMDLine += "854:480 ";
            //            break;
            //        }
            //    case (int)perfOption.medium:
            //        {
            //            _presetCMDLine += "640:360 ";
            //            break;
            //        }
            //    case (int)perfOption.slow:
            //        {
            //            _presetCMDLine += "426:240 ";
            //            break;
            //        }
            //    case (int)perfOption.slower:
            //        {
            //            _presetCMDLine += "426:240 ";
            //            break;
            //        }
            //    case (int)perfOption.veryslow:
            //        {
            //            _presetCMDLine += "426:240 ";
            //            break;
            //        }
            //    case (int)perfOption.placebo:
            //        {
            //            _presetCMDLine += "426:240 ";
            //            break;
            //        }
            //    default:
            //        {
            //            _presetCMDLine += "1920:1080 ";
            //            break;
            //        }
            //}

            //return _presetCMDLine;
        }
    }
}