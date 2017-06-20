using System;
using System.Diagnostics;
using System.Text;

namespace x265Com.ScriptCS
{
    #region Enums
    public enum defImageEnum { UHD_3840x2160, HD_1920x1080, HD_1440x1080, HD_1270x720, Proxy_480p, Proxy_360p, Proxy_240p }
    public enum perfOptionEnum { ultrafast, superfast, veryfast, faster, fast, medium, slow, slower, veryslow, placebo }
    public enum CTUEnum { c64, c32, c16, c8 }
    //public enum conteneur { mov, mxf, mp4 }
    public enum videoCodecEnum { HEVC, h264, Mpeg2, vp9 }
    public enum audioCodecEnum { aac, mp3, flac, pcm }
    public enum cadenceImageEnum { fps25, fps50, fps60, fps100, fps300 }
    #endregion
    public class ConversionInfo
    {
        #region Propriétés
        public string InFilePath { get; set; }
        public string InFileName { get; set; }
        public string OutFilePath { get; set; }
        public string OutFileName { get; set; }
        public cadenceImageEnum cadenceImage { get; set; }
        public defImageEnum DefImage { get; set; }
        public perfOptionEnum perfOption { get; set; }
        public CTUEnum CTU { get; set; }
        //public conteneur Conteneur { get; set; }
        public videoCodecEnum VideoCodec { get; set; }
        public audioCodecEnum AudioCodec { get; set; }
        public int debitAudio { get; set; }
        public int debitVideo { get; set; }
        public int tailleGop { get; set; }
        public int quantizerParameter { get; set; }
        public bool isQP { get; set; }
        public bool isLossless { get; set; }
        public bool isWpp { get; set; }

        #region Exe
        public bool FirstFPS { get; set; }
        public string logErrorPath { get; set; }
        public string logDataPath { get; set; }
        #endregion
        #endregion

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
                _message = "Fichier d'entrée inexistant : " + InFilePath + InFileName;
                return _isSuccess;
            }
            #endregion

            #region CheckIfOutDirectoryExists
            bool _outDirectoryExists = Tools.CheckIfDirectoryExists(OutFilePath);
            if (!_outDirectoryExists)
            {
                _message = "Dossier de sortie inexistant : " + OutFilePath;
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

        /// <summary>
        /// Fonction qui construit la commande FFMPEG à executer
        /// </summary>
        /// <returns></returns>
        public string BuildConversionString()
        {
            //Example Line : ffmpeg -i input.avi -b:v 64k -bufsize 64k output.avi            
            StringBuilder _cmdStringBuilder = new StringBuilder("ffmpeg -thread_queue_size 32 -vsync passthrough -frame_drop_threshold 4 -i " + InFilePath + @"\" + InFileName + " ");
            _cmdStringBuilder.Append(getCadenceImage());
            _cmdStringBuilder.Append(getResolutionCommandString());
            _cmdStringBuilder.Append(getVideoCodec());
            _cmdStringBuilder.Append(getQP());
            _cmdStringBuilder.Append(getDebitVideo());
            _cmdStringBuilder.Append(getAudioCodec());
            _cmdStringBuilder.Append(getDebitAudio());
            //_cmdStringBuilder.Append(getCTU());
            //if (isWpp)
            //{
            //    _cmdStringBuilder.Append("-wpp:1 ");
            //}
            //else
            //{
            //    _cmdStringBuilder.Append("-no-wpp:0 ");
            //}
            _cmdStringBuilder.Append(getPresetCommandString());

            _cmdStringBuilder.Append(" -y " + OutFilePath + @"\" + OutFileName);

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
            if (!string.IsNullOrEmpty(output))
                System.IO.File.WriteAllText(_LogPath, output);
            //Tools.WriteErrorInXml(output);
            _process.WaitForExit();
            return _process.ExitCode;
        }

        void OutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //Tools.WriteErrorInXml(outLine.Data);
            //string _LogPath = Tools.GetLogFilePathAndName(LogFileType.ConsoleOutputDataLog);
            //System.IO.File.WriteAllText(_LogPath, outLine.Data);
            System.IO.File.AppendAllText(logDataPath, outLine.Data + "\n");
            //if(outLine.Data.Contains(""))
            //Console.WriteLine(outLine.Data);
        }

        void OutputErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //Tools.WriteErrorInXml(outLine.Data);
            //string _LogPath = Tools.GetLogFilePathAndName(LogFileType.ConsoleOutputErrorLog);
            // System.IO.File.WriteAllText(_LogPath, outLine.Data);
            System.IO.File.AppendAllText(logErrorPath, outLine.Data + "\n");
            //Console.WriteLine(outLine.Data);
        }

        public string getResolutionCommandString()
        {
            string _resolutionCMDLine = "";

            _resolutionCMDLine = "-vf scale=";
            switch (DefImage)
            {
                case defImageEnum.UHD_3840x2160:
                    {
                        _resolutionCMDLine += "3840:2160 ";
                        break;
                    }
                case defImageEnum.HD_1920x1080:
                    {
                        _resolutionCMDLine += "1920:1080 ";
                        break;
                    }
                case defImageEnum.HD_1440x1080:
                    {
                        _resolutionCMDLine += "1440:1080 ";
                        break;
                    }
                case defImageEnum.HD_1270x720:
                    {
                        _resolutionCMDLine += "1270:720 ";
                        break;
                    }
                case defImageEnum.Proxy_480p:
                    {
                        _resolutionCMDLine += "854:480 ";
                        break;
                    }
                case defImageEnum.Proxy_360p:
                    {
                        _resolutionCMDLine += "640:360 ";
                        break;
                    }
                case defImageEnum.Proxy_240p:
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
            if (!isLossless)
            {
                switch (VideoCodec)
                {
                    case videoCodecEnum.HEVC:
                        {
                            _VideoCodecLine = "-c:v libx265 ";
                            break;
                        }
                    case videoCodecEnum.h264:
                        {
                            _VideoCodecLine = "-c:v libx264 ";
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
            }
            else
            {
                _VideoCodecLine = "-c:v rawvideo";
            }

            return _VideoCodecLine;
        }

        public string getAudioCodec()
        {
            string _AudioCodecLine = "";
            switch (AudioCodec)
            {
                case audioCodecEnum.aac:
                    {
                        _AudioCodecLine = "-acodec aac ";
                        break;
                    }
                case audioCodecEnum.mp3:
                    {
                        _AudioCodecLine = "-acodec libmp3lame ";
                        break;
                    }
                case audioCodecEnum.pcm:
                    {
                        //_AudioCodecLine = "-c:v mpeg2video ";
                        break;
                    }
                case audioCodecEnum.flac:
                    {
                        _AudioCodecLine = "-acodec flac ";
                        break;
                    }
            }
            return _AudioCodecLine;
        }

        public string getCTU()
        {
            string _CTU = "";
            switch (CTU)
            {
                case CTUEnum.c64:
                    {
                        _CTU = "-CTU=64 ";
                        break;
                    }
                case CTUEnum.c32:
                    {
                        _CTU = "-CTU=32 ";
                        break;
                    }
                case CTUEnum.c16:
                    {
                        _CTU = "-CTU=16 ";
                        break;
                    }
                case CTUEnum.c8:
                    {
                        _CTU = "-CTU=8 ";
                        break;
                    }
            }
            return _CTU;
        }

        public string getQP()
        {
            string _QP = "";
            if (isQP)
            {
                if (quantizerParameter > 51)
                {
                    quantizerParameter = 51;
                }
                else
                {
                    if (quantizerParameter < 0)
                    {
                        quantizerParameter = 0;
                    }
                }
                if (VideoCodec == videoCodecEnum.h264)
                {
                    _QP = "-x264-params qp=" + quantizerParameter + " ";
                }
                else
                {
                    if (VideoCodec == videoCodecEnum.HEVC)
                    {
                        _QP = "-x265-params qp=" + quantizerParameter + " ";
                    }
                }
            }

            return _QP;
        }

        public string getPresetCommandString()
        {
            string _presetCMDLine = "-preset " + perfOption.ToString() + " ";
            return _presetCMDLine;
        }

        public string getDebitAudio()
        {
            string _presetDebitAudio = "";
            if (debitAudio != 0)
            {
                _presetDebitAudio = "-b:a " + (debitAudio * 1000).ToString() + " ";
            }
            return _presetDebitAudio;
        }

        public string getDebitVideo()
        {
            string _presetDebitVideo = "";
            if (debitVideo != 0)
            {
                _presetDebitVideo = "-b:v " + (debitVideo * 1000).ToString() + " ";
            }
            return _presetDebitVideo;
        }
        public string getCadenceImage()
        {
            string _cadenceImage = "-r ";
            switch (cadenceImage)
            {
                case cadenceImageEnum.fps25:
                    {
                        _cadenceImage = _cadenceImage + "25 ";
                        break;
                    }
                case cadenceImageEnum.fps50:
                    {
                        _cadenceImage = _cadenceImage + "50 ";
                        break;
                    }
                case cadenceImageEnum.fps60:
                    {
                        _cadenceImage = _cadenceImage + "60 ";
                        break;
                    }
                case cadenceImageEnum.fps100:
                    {
                        _cadenceImage = _cadenceImage + "100 ";
                        break;
                    }
                case cadenceImageEnum.fps300:
                    {
                        _cadenceImage = _cadenceImage + "300 ";
                        break;
                    }
            }
            return _cadenceImage;
        }
    }
}