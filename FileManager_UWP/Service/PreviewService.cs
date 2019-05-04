using FileManager_UWP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace FileManager_UWP.Service
{
    class PreviewService
    {
        public static PreviewModel.FileType GetFileType(string path)
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

        public static async Task<IRandomAccessStream> GetPicPreviewAsync(string path)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(path);
            return await file.OpenAsync(FileAccessMode.Read);
        }

        //public static async Task<IRandomAccessStream> GetPDFPreviewAsyc(string path)
        //{
            
        //    return new IRandomAccessStream();
        //}

        public static async Task<IRandomAccessStream> ShowPreviewAsync(string path)
        {
            return await GetPicPreviewAsync(path);
        }
    }
}
