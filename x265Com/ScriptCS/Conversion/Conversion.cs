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
        public enum perfOptionEnum { ultrafast, superfast, veryfast, faster, fast, medium, slow, slower, veryslow, placebo }
        public perfOptionEnum perfOption { get; set; }
        public int CTU { get; set; }
        public enum conteneur { mov, mxf, mp4 }
        public int Conteneur { get; set; }
        public enum videoCodecEnum { HEVC, h264, Mpeg2, vp9 }
        public videoCodecEnum VideoCodec { get; set; }
        public int debitVideo { get; set; }
        public int tailleGop { get; set; }
        public bool isQP { get; set; }
        public enum audioCodecEnum { mp3, wmv, pcm, aac }
        public audioCodecEnum AudioCodec { get; set; }
        public int debitAudio { get; set; }
        public string logErrorPath { get; set; }
        public string logDataPath { get; set; }

        public bool StartConversion(out int _exitCode, out TimeSpan _Elapsed, out string _message)
        {
            _Elapsed = new TimeSpan();
            _exitCode = 0;
            _message = string.Empty;
            bool _isSuccess = false;
            string _CMDConversionStr = string.Empty;

            #region CheckIfInFileExists
            bool _inFileExists = Tools.CheckIfFileExists(InFileName, InFilePath);
            if (!_inFileExists)
            {
                _message = "Fichier d'entrée inexistant : "+ InFilePath+ InFileName;
                return _isSuccess;
            }
            #endregion

            #region CheckIfOutDirectoryExists
            bool _outDirectoryExists = Tools.CheckIfDirectoryExists(OutFilePath);
            if (!_outDirectoryExists)
            {
                _message = "Dossier de sortie inexistant : "+ OutFilePath;
                return _isSuccess;
            }
            #endregion

            _CMDConversionStr = BuildConversionString();
            if (string.IsNullOrEmpty(_CMDConversionStr))
                return _isSuccess;
            DateTime _Begin = DateTime.Now;
            _exitCode = LaunchCMDCommand(_CMDConversionStr);            
            _Elapsed = DateTime.Now - _Begin;
            if (_exitCode == 0)
                _isSuccess = true;
            return _isSuccess;
        }
        
        public string BuildConversionString()
        {
            //Example Line : ffmpeg -i input.avi -b:v 64k -bufsize 64k output.avi            
            StringBuilder _cmdStringBuilder = new StringBuilder("ffmpeg -thread_queue_size 32 -vsync passthrough -frame_drop_threshold 4 -i " +  InFilePath + @"\" + InFileName + " ");
            cadenceImage = cadenceImage == 0 ? 1 : cadenceImage;
            _cmdStringBuilder.Append("-r " + cadenceImage + " ");
            _cmdStringBuilder.Append(getResolutionCommandString());
            _cmdStringBuilder.Append(getVideoCodec());
            //if (isWpp)
            //{
            //    _cmdStringBuilder.Append("-wpp:1 ");
            //}
            //else
            //{
            //    _cmdStringBuilder.Append("-no-wpp:0 ");
            //}
            _cmdStringBuilder.Append(getPresetCommandString());

            _cmdStringBuilder.Append(" -y "+OutFilePath + @"\" + OutFileName);

            return _cmdStringBuilder.ToString();
        }
        /// <summary>
        /// Fonction qui lance une commande CMD à partir d'une string et log les erreurs reçus
        /// </summary>
        /// <param name="_cmdstring"></param>
        /// <returns></returns>
        public int LaunchCMDCommand(string _cmdstring)
        {
            
            _cmdstring = "/C " + _cmdstring;
            Process _process = new Process();
            //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            _process.StartInfo.FileName = "cmd.exe";
            _process.StartInfo.Arguments = _cmdstring;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.Verb = "runas";
            logErrorPath = Tools.GetLogFilePathAndName(LogFileType.ConsoleOutputErrorLog);
            System.IO.File.AppendAllText(logErrorPath, _cmdstring + "\n");
            //logDataPath = Tools.GetLogFilePathAndName(LogFileType.ConsoleOutputDataLog);
            _process.ErrorDataReceived += new DataReceivedEventHandler(OutputErrorHandler);
            //process.OutputDataReceived += new DataReceivedEventHandler(OutputDataHandler);
            _process.Start();
            _process.BeginErrorReadLine();
            //process.BeginOutputReadLine();
            string output = _process.StandardOutput.ReadToEnd();
            //Console.WriteLine(output);
            string _LogPath = Tools.GetLogFilePathAndName(LogFileType.ConsoleLog);
            //File.AppendAllText(_LogPath, appendText);
            if(!string.IsNullOrEmpty(output))
                System.IO.File.WriteAllText(_LogPath, output);
            //Tools.WriteErrorInXml(output);
            _process.WaitForExit();
            return _process.ExitCode;
        }

        void OutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            //Tools.WriteErrorInXml(outLine.Data);
            //string _LogPath = Tools.GetLogFilePathAndName(LogFileType.ConsoleOutputDataLog);
            //System.IO.File.WriteAllText(_LogPath, outLine.Data);
            System.IO.File.AppendAllText(logDataPath, outLine.Data+"\n");
            //if(outLine.Data.Contains(""))
            //Console.WriteLine(outLine.Data);
        }

        void OutputErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            //Tools.WriteErrorInXml(outLine.Data);
            //string _LogPath = Tools.GetLogFilePathAndName(LogFileType.ConsoleOutputErrorLog);
           // System.IO.File.WriteAllText(_LogPath, outLine.Data);
            System.IO.File.AppendAllText(logErrorPath, outLine.Data+"\n");
            //Console.WriteLine(outLine.Data);
        }

        public string getResolutionCommandString()
        {
            string _resolutionCMDLine = "-vf scale=";
            switch (DefImage)
            {
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
        public string getVideoCodec()
        {
            string _VideoCodecLine = "";
            switch (VideoCodec)
            {
                case videoCodecEnum.HEVC:
                    {
                        _VideoCodecLine = "-c:v libx265 ";
                        break;
                    }
                case videoCodecEnum.h264:
                    {
                        _VideoCodecLine = "";
                        break;
                    }
                case videoCodecEnum.Mpeg2:
                    {
                        _VideoCodecLine = "-c:v mpeg2video ";
                        break;
                    }
                case videoCodecEnum.vp9:
                    {
                        _VideoCodecLine = "-c:v libvpx-vp9 ";
                        break;
                    }
            }
            return _VideoCodecLine;
        }
        public string getPresetCommandString()
        {
            string _presetCMDLine =  "-preset " + perfOption.ToString() + " ";
            return _presetCMDLine;
        }
    }
}