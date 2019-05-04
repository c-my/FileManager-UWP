using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_UWP.Model
{
    public class PreviewModel
    {
        public const string PNGType = "13780";
        public const string JPGType = "255216";
        public const string GIFType = "7173";
        public const string BMPType = "6677";

        public const string PDFType = "3780";


        public enum FileType
        {
            NAT = 0,//not a type
            Picture,
            Pdf,
        }
    }
}
