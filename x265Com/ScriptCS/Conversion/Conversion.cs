using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

    }
    public class ConversionHelper
    {
        
    }
}