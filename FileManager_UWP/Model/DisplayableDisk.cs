using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace FileManager_UWP.Model
{
    public class DisplayableDisk: IDisplayable {
        private readonly DriveInfo _drive;
        private BitmapImage _icon = new BitmapImage();

        private DisplayableDisk(DriveInfo d) {
            _drive = d;
        }

        public static async Task<DisplayableDisk> GetInstance(DriveInfo d) {
            var obj = new DisplayableDisk(d);
            StorageFolder f = await StorageFolder.GetFolderFromPathAsync(d.Name);
            var thumbnail = await f.GetThumbnailAsync(ThumbnailMode.ListView, 32);
            await obj._icon.SetSourceAsync(thumbnail).AsTask();

            return obj;
        }


        public string Name => _drive.Name;
        public string Path => _drive.Name;
        public BitmapImage Icon => _icon;
        public bool IsFolder => true;
    }
}
