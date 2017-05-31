using System;

namespace x265Com.ScriptCS
{
    public class ConversionInfo
    {
        public bool isWpp { get; set; }
        public enum ConversionSpeed { Slow, Regular, Fast }
        public int Length { get; set; }
        public int Width { get; set; }
        public int EndTime { get; set; }
        public string InFormat { get; set; }
        public string OutFormat { get; set; }

        public bool StartConversion()
        {
            bool _isSuccess=false;
            string _CMDConversionStr = string.Empty;
            _CMDConversionStr = this.BuildConversionString();
            if (string.IsNullOrEmpty(_CMDConversionStr))
                return _isSuccess;
            try
            {
                LaunchCMDCommand(_CMDConversionStr);
            }
            catch (Exception e)
            {

                return _isSuccess;
            }
            _isSuccess = true;
            return _isSuccess;
        }
        public string BuildConversionString()
        {            
            string _cmdstring = "";
            if(isWpp)
            {
                _cmdstring += " --wpp";
            }
            else
            {
                _cmdstring += " --no-wpp";
            }
            return string.Empty;
        }
        public static void LaunchCMDCommand(string _cmdstring)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + _cmdstring;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}