using System.Collections.Generic;

namespace x265Com.Models
{
    public class WatchFolderModel
    {
        public string FolderName { get; set; }
        public List<FileModel> FileModels { get; set; }        
    }
    public class FileModel
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileLength { get; set; }
    }
}