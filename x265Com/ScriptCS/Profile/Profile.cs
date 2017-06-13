using System.Configuration;

namespace x265Com.ScriptCS.Profile
{
    public class ProfileInfo
    {
        public bool isWpp { get; set; }
        public enum ConversionSpeed { Slow, Regular, Fast }
        public int Length { get; set; }
        public int Width { get; set; }
        public int EndTime { get; set; }
        public string InFormat { get; set; }
        public string OutFormat { get; set; }
    }
    public class ProfileHelper
    {
        public static void SaveProfileInXML(ProfileInfo _ProfileToSave)
        {
            string WatchFolderPath = ConfigurationManager.AppSettings["WatchFolderPath"].ToString();

        }
    }
}

