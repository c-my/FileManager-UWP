using FileManager_UWP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_UWP.Service
{
    class PreviewService
    {
        public static PreviewModel.FileType getFileType(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fs);
            string bx = "";
            byte buffer;
            try
            {
                buffer = reader.ReadByte();
                bx = buffer.ToString();
                buffer = reader.ReadByte();
                bx += buffer.ToString();
            }
            catch (System.IO.IOException)
            {

            }
            switch (bx)
            {
                case PreviewModel.PNGType:
                case PreviewModel.GIFType:
                case PreviewModel.JPGType:
                case PreviewModel.BMPType:
                    return PreviewModel.FileType.Picture;
                case PreviewModel.PDFType:
                    return PreviewModel.FileType.Pdf;
                default:
                    return PreviewModel.FileType.NAT;
            }
        } 
    }
}
