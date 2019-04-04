using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace FileManager_UWP.Model
{
    public class DisplayableFolder: IDisplayable {
        private readonly StorageFolder _folder;
        private readonly BitmapImage _icon;

        private DisplayableFolder(StorageFolder f) {
            _folder = f;
            _icon = new BitmapImage();
        }

        public static async Task<DisplayableFolder> GetInstanceAsync(StorageFolder f)
        {
            DisplayableFolder obj = new DisplayableFolder(f);
            var thumbnail = await f.GetThumbnailAsync(ThumbnailMode.ListView, 32);
            await obj._icon.SetSourceAsync(thumbnail).AsTask();
            return obj;
        }

        public static async Task<IDisplayable> GetParentAsync(StorageFolder f) {
            string path = f.Path;
            BitmapImage img = new BitmapImage();
            if (path.Length > 4) {
                path = f.Path.Substring(0, f.Path.LastIndexOf('\\'));
                StorageFolder file = await StorageFolder.GetFolderFromPathAsync(path);
                var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.ListView, 32);
                await img.SetSourceAsync(thumbnail).AsTask();
                return new DisplayableSpecial("..", file.Path, true, img);
            }
            else {
                var thumbnail = await f.GetThumbnailAsync(ThumbnailMode.ListView, 32);
                await img.SetSourceAsync(thumbnail).AsTask();
                return new DisplayableSpecial("..", "/", true, img);
            }
        }

        public string Name => _folder.Name;

        public string Path => _folder.Path;

        public bool IsFolder => true;

        public BitmapImage Icon => _icon;
    }
}
