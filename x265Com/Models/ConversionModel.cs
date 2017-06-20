using System.ComponentModel.DataAnnotations;

namespace x265Com.Models
{
    public class ConversionModel
    {
        [Required]
        public string InputFilePathAndName { get; set; }
        [Required]
        public string OutputFilePathAndName { get; set; }
        //public string InFilePath { get; set; }
        //public string InFileName { get; set; }
        //public string OutFilePath { get; set; }
        //public string OutFileName { get; set; }
        public int cadenceImage { get; set; }
        public bool isWpp { get; set; }
        //public enum defImage { UHD_3840x2160, HD_1920x1080, HD_1440x1080, HD_1270x720, Proxy_480p, Proxy_360p, Proxy_240p }
        public int DefImage { get; set; }
        //public enum perfOptionEnum { ultrafast, superfast, veryfast, faster, fast, medium, slow, slower, veryslow, placebo }
        public int perfOption { get; set; }
        public int CTU { get; set; }
        //public enum conteneur { mov, mxf, mp4 }
        //public int Conteneur { get; set; }
        //public enum videoCodec { HEVC, h264, vp9 }
        public int VideoCodec { get; set; }
        public int debitVideo { get; set; }
        public int tailleGop { get; set; }
        public int quantizerParameter { get; set; }
        public bool isQP { get; set; }
        public bool isLossless { get; set; }
        //public enum audioCodec { mp3, wmv, pcm, aac }
        public int AudioCodec { get; set; }
        public int debitAudio { get; set; }
        //public string logErrorPath { get; set; }
        //public string logDataPath { get; set; }

        public bool isOver { get; set; }
        public bool isSuccess { get; set; }
        public int ExitCode { get; set; }
        public System.TimeSpan Elapsed { get; set; }
        public string CMDconsoleMessage { get; set; }
        public int ConversionProgress { get; set; }
    }
}