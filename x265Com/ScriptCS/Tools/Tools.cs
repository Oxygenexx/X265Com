using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;

namespace x265Com.ScriptCS
{
    public class Tools
    {
        public static void WriteErrorInXml(string _errorMessage)
        {
            string _filePathAndName = ConfigurationManager.AppSettings["ErrorFilePath"].ToString() + @"\" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + "_ErrorLog";
            XMLWriting(_filePathAndName, _errorMessage);
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