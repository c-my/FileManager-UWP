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
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;

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
                case PreviewModel.OfficeXType:
                case PreviewModel.OfficeOldType:
                    return PreviewModel.FileType.Office;
                default:
                    return PreviewModel.FileType.NAT;
            }
        }

        public static async Task<IRandomAccessStream> GetWordPreviewAsync(string path)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(path);
            var res = await file.OpenAsync(FileAccessMode.Read);
            WordDocument wordDocument = new WordDocument();
            wordDocument.Open(res.AsStream(), Syncfusion.DocIO.FormatType.Automatic);
            DocIORenderer render = new DocIORenderer();
            Syncfusion.Pdf.PdfDocument pdfDocument = render.ConvertToPDF(wordDocument);
            render.Dispose();
            wordDocument.Dispose();
            InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
            pdfDocument.Save(ms.AsStream());
            return await GetPDFPreviewFromStreamAsync(ms);
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
            catch (System.Exception)
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
                var pdf = await Windows.Data.Pdf.PdfDocument.LoadFromFileAsync(pdfFile);
                var page = pdf.GetPage(0);
                InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
                await page.RenderToStreamAsync(ms);
                return ms;
            }
            catch (Exception)
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
                case PreviewModel.FileType.Office:
                    return await GetWordPreviewAsync(path);
                default:
                    return await GetPicPreviewAsync(defaultPicPath);
            }
        }

        private static async Task<IRandomAccessStream> GetPDFPreviewFromStreamAsync(IRandomAccessStream stream)
        {
            try
            {
                var pdf = await Windows.Data.Pdf.PdfDocument.LoadFromStreamAsync(stream);
                var page = pdf.GetPage(0);
                InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
                await page.RenderToStreamAsync(ms);
                return ms;
            }
            catch (Exception)
            {
                return await GetPicPreviewAsync(defaultPicPath);
            }
        }
    }
}
