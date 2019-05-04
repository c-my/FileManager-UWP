using FileManager_UWP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Storage.Streams;

namespace FileManager_UWP.Service
{
    class PreviewService
    {

        private static string defaultPicPath = "ms-appx:///Assets/StoreLogo.png";


        /// <summary>
        /// 判断文件的类型（图片/PDF文档）
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件类型</returns>
        public static async Task<PreviewModel.FileType> GetFileTypeAsync(string path)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(path);
            Windows.Storage.Streams.Buffer buf = new Windows.Storage.Streams.Buffer(2);
            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(file);
            string res = "";
            using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
            {
                var byt = dataReader.ReadByte();
                res = byt.ToString();
                byt = dataReader.ReadByte();
                res += byt.ToString();
            }
            switch (res)
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

        /// <summary>
        /// 获得图片文件的预览图
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns>图片的预览</returns>
        public static async Task<IRandomAccessStream> GetPicPreviewAsync(string path)
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(path);
                var res = await file.OpenAsync(FileAccessMode.Read);
                return res;
            }
            catch (System.Exception exception)
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(defaultPicPath);
                return await file.OpenAsync(FileAccessMode.Read);
            }
        }

        /// <summary>
        /// 获得PDF文件的预览图
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <returns>PDF的预览</returns>
        public static async Task<IRandomAccessStream> GetPDFPreviewAsync(string path)
        {
            StorageFile pdfFile;
            try
            {
                pdfFile = await StorageFile.GetFileFromPathAsync(path);
                var pdf = await PdfDocument.LoadFromFileAsync(pdfFile);
                var page = pdf.GetPage(0);
                InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
                await page.RenderToStreamAsync(ms);
                return ms;
            }
            catch (Exception exception)
            {
                return await GetPicPreviewAsync(defaultPicPath);
            }
        }

        public static async Task<IRandomAccessStream> ShowPreviewAsync(string path)
        {
            var t = await GetFileTypeAsync(path);
            switch (t)
            {
                case PreviewModel.FileType.Picture:
                    return await GetPicPreviewAsync(path);
                case PreviewModel.FileType.Pdf:
                    return await GetPDFPreviewAsync(path);
                default:
                    return await GetPicPreviewAsync(defaultPicPath);
            }
        }
    }
}
