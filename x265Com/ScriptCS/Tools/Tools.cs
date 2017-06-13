using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;

namespace x265Com.ScriptCS
{
    public enum LogFileType { ErrorLog, ConsoleLog, ConsoleOutputHandlerLog, ConsoleOutputDataLog, ConsoleOutputErrorLog }

    public class Tools
    {
        public static string GetLogFilePathAndName(LogFileType _LogFileType)
        {
            DateTime _Date = DateTime.Now;
            return ConfigurationManager.AppSettings["LogFilePath"].ToString() + @"\" + _Date.Year.ToString() + "-" + _Date.Month.ToString() + "-" + _Date.Day.ToString() + "-" + _Date.Hour.ToString() + "-" + _Date.Minute.ToString() + "-" + _Date.Second.ToString() + "_" + _LogFileType.ToString()+".txt";
        }

        public static void WriteErrorInTextFile(string _errorMessage, string _stackTrace)
        {
            string _filePathAndName = GetLogFilePathAndName(LogFileType.ErrorLog);
            string _MessageToWrite =  _errorMessage + "\n\n" + _stackTrace;
            File.WriteAllText(_filePathAndName, _MessageToWrite);
        }

        public static void XMLWriting(string _path, string _string)
        {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create(_path + ".xml", settings);
            writer.WriteStartDocument();
            writer.WriteString(_string);
            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();


        }

        public static bool CheckIfFileExists(string _FileName, string _FilePath)
        {
            bool _exist = false;
            try
            {
                DirectoryInfo _DirectoryInfo = new DirectoryInfo(_FilePath);

                FileInfo[] _Files = _DirectoryInfo.GetFiles(_FileName);

                if ((_Files[0].Exists) && (_Files[0] != null))
                {
                    _exist = true;
                    return _exist;
                }
            }
            catch (Exception _e)
            {
                WriteErrorInTextFile(_e.Message, _e.StackTrace);
            }

            return _exist;
        }

        public static bool CheckIfDirectoryExists(string _Path)
        {
            try
            {
                DirectoryInfo _DirectoryInfo = new DirectoryInfo(_Path);
                if (_DirectoryInfo.Exists)
                {
                    return true;
                }
            }
            catch(Exception _e)
            {
                WriteErrorInTextFile(_e.Message, _e.StackTrace);
            }            
            return false;
        }

        public static string StripPathOfDoubleSlashes(string _FilePath)
        {
            List<int> foundIndexes = new List<int>();
            for (int k = _FilePath.IndexOf('\\'); k > -1; k = _FilePath.IndexOf('\\', k + 1))
            {
                // for loop end when i=-1 ('a' not found)
                foundIndexes.Add(k);
            }
            int i = 0;
            foreach (int foundIndexe in foundIndexes)
            {
                _FilePath.Remove(foundIndexe, 2);                
                i++;
            }
            return _FilePath;
        }

        public static string GetFileName(string _FilePathAndName, int SlashLastIndex)
        {
            return _FilePathAndName.Substring((SlashLastIndex + 1), (_FilePathAndName.Length - (SlashLastIndex + 1)));
        }

        public static string GetFilePath(string _FilePathAndName, int SlashLastIndex)
        {
            return _FilePathAndName.Substring(0, SlashLastIndex);
        }
    }
}