using System.IO;
using System.Configuration;
using System.Collections.Generic;
using x265Com.Models;

namespace x265Com.ScriptCS
{
    public class WatchFolder
    {
        
        public static WatchFolderModel IniWatchFolder()
        {
            string WatchFolderPath = ConfigurationManager.AppSettings["WatchFolderPath"].ToString();
            //FileSystemWatcher _FileSystemWatcher = new FileSystemWatcher(WatchFolderPath);
            DirectoryInfo _DirectoryInfo = new DirectoryInfo(WatchFolderPath);

            //FileInfo[] Files = _DirectoryInfo.GetFiles("*.txt"); //cette ligne permet de rechercher un pattern dans le directory regardé
            FileInfo[] _Files = _DirectoryInfo.GetFiles();

            List<FileInfo> FileS = new List<FileInfo>();
            foreach (FileInfo _File in _Files)
            {
                FileS.Add(_File);
            }
            WatchFolderModel _WatchFolderModel = BuildWatchFolderModel(FileS);

            return _WatchFolderModel;
        }
        public static WatchFolderModel BuildWatchFolderModel(List<System.IO.FileInfo> FileS)
        {
            WatchFolderModel _WatchFolderModel = new WatchFolderModel();
            if (FileS[0].Directory.Exists)
            {
                _WatchFolderModel.FolderName = FileS[0].Directory.Name; //Directory
            }
            if (FileS != null)
            {
                _WatchFolderModel.FileModels = new List<FileModel>();

                foreach (System.IO.FileInfo File in FileS)
                {
                    FileModel _FileModel = new FileModel();
                    _FileModel.FileName = File.Name;
                    _FileModel.FileType = File.Extension;
                    _FileModel.FileType = _FileModel.FileType.Length <= 1 ? "" : _FileModel.FileType.Substring(1);
                    _FileModel.FileLength = GetLengthStringFromLong(File.Length);
                    _WatchFolderModel.FileModels.Add(_FileModel);
                }
            }
            return _WatchFolderModel;
        }
        /// <summary>
        /// Convertit un long représentant la taille en octets en une chaîne de charactères avec l'unité qui va bien
        /// </summary>
        /// <param name="_length">taille en octet</param>
        /// <returns>string avec l'unité</returns>
        public static string GetLengthStringFromLong(long _length)
        {
            string LengthString;

            if(_length/1024.0 > 1)
            {

                if(_length/(1024.0*1024.0) > 1)
                {
                    if(_length / (1024.0 * 1024.0 * 1024.0) > 1)
                    {
                        LengthString = System.Math.Round(_length / (1024.0 * 1024.0 * 1024.0)).ToString() + " GB";
                    }
                    else
                    {
                        LengthString = System.Math.Round(_length / (1024.0 * 1024.0)).ToString() + " MB";
                    }
                }
                else
                {
                    LengthString = System.Math.Round(_length / (1024.0)).ToString() + " kB";
                }
            }
            else
            {
                LengthString = _length.ToString() + " Bytes";
            }
            return LengthString;
        }
    }
}