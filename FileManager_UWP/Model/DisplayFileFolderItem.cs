using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace FileManager_UWP.Model {
    /// <summary>
    /// 显示用的文件文件夹统一类
    /// </summary>
    public class DisplayFileFolderItem {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsFolder { get; set; }
        public BitmapImage Icon { get; set; } = new BitmapImage();

        public DisplayFileFolderItem(StorageFolder file) {
            Name = file.Name;
            Path = file.Path;
            IsFolder = true;
        }
        public DisplayFileFolderItem(StorageFile file) {
            Name = file.Name;
            Path = file.Path;
            IsFolder = false;
        }
        private DisplayFileFolderItem() { }

        public static async Task<DisplayFileFolderItem> GetInstance(StorageFile file)
        {
            DisplayFileFolderItem obj = new DisplayFileFolderItem
            {
                Name = file.Name,
                Path = file.Path,
                IsFolder = false,
            };
            var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.ListView, 32);
            await obj.Icon.SetSourceAsync(thumbnail).AsTask();

            //var imgExt = new[] { "bmp", "gif", "jpeg", "jpg", "png" }.FirstOrDefault(ext => file.Path.ToLower().EndsWith(ext));
            //if (imgExt != null) {
            //    var dummy = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("dummy." + imgExt, CreationCollisionOption.ReplaceExisting); //may overwrite existing
            //    obj.Icon = await dummy.GetThumbnailAsync(ThumbnailMode.SingleItem, 32);
            //} else {
            //    obj.Icon = await file.GetThumbnailAsync(ThumbnailMode.SingleItem, 32);
            //}

            return obj;
        }

        public static async Task<DisplayFileFolderItem> GetInstance(StorageFolder file) {
            DisplayFileFolderItem obj = new DisplayFileFolderItem {
                Name = file.Name,
                Path = file.Path,
                IsFolder = true,
            };
            var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.ListView, 32);
            await obj.Icon.SetSourceAsync(thumbnail).AsTask();

            //var imgExt = new[] { "bmp", "gif", "jpeg", "jpg", "png" }.FirstOrDefault(ext => file.Path.ToLower().EndsWith(ext));
            //if (imgExt != null) {
            //    var dummy = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("dummy." + imgExt, CreationCollisionOption.ReplaceExisting); //may overwrite existing
            //    obj.Icon = await dummy.GetThumbnailAsync(ThumbnailMode.SingleItem, 32);
            //} else {
            //    obj.Icon = await file.GetThumbnailAsync(ThumbnailMode.SingleItem, 32);
            //}

            return obj;
        }

        public async static Task<DisplayFileFolderItem> parent(string current) {
            DisplayFileFolderItem d = new DisplayFileFolderItem
            {
                Name = "..",
                IsFolder = true,
                Path = current.Substring(0, current.LastIndexOf('\\'))

            };
            if (d.Path.Length == 2)
                d.Path += "\\";
            StorageFolder file = await StorageFolder.GetFolderFromPathAsync(d.Path);
            var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.ListView, 32);
            await d.Icon.SetSourceAsync(thumbnail).AsTask();
            return d;
        }
    }
}
