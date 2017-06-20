using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace x265Com.Models
{
    public class FilesConversionModel
    {


        public bool isOver { get; set; }
        public bool isSuccess { get; set; }
        public int ExitCode { get; set; }
        public TimeSpan Elapsed { get; set; }
        public string CMDconsoleMessage { get; set; }
        public int ConversionProgress { get; set; }
    }
}