using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace x265Com.ScriptCS
{
    public class WatchFolder
    {
        
        public static void IniWatchFolder()
        {
            string WatchFolderPath = ConfigurationManager.AppSettings["WatchFolderPath"].ToString();
            FileSystemWatcher _FileSystemWatcher = new FileSystemWatcher(WatchFolderPath);
            var adddress = @"data\";
            string str = "";
            DirectoryInfo d = new DirectoryInfo(adddress);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                str = str + ", " + file.Name;
            }
        }
    }
}