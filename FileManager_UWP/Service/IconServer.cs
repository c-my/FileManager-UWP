using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace FileManager_UWP.Service
{
    /// <summary>
    /// 管理文件和文件夹图标的静态类
    /// </summary>
    public class IconServer
    {
        private IconServer() {
        }

        public static async Task<BitmapImage> GetFolderIcon(ThumbnailMode mode, uint size) {
            StorageFolder f = await StorageFolder.GetFolderFromPathAsync("C:\\Windows");
            BitmapImage img = new BitmapImage();
            StorageItemThumbnail thumbnail = await f.GetThumbnailAsync(mode, size);
            await img.SetSourceAsync(thumbnail).AsTask();

            return img;
        }
    }
}
