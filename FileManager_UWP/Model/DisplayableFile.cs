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
    public class DisplayableFile: Displayable {
        private readonly StorageFile _file;
        private readonly BitmapImage _icon;

        private DisplayableFile(StorageFile f) {
            _file = f;
            _icon = new BitmapImage();
        }

        public static async Task<DisplayableFile> GetInstanceAsync(StorageFile f) {
            DisplayableFile obj = new DisplayableFile(f);
            var thumbnail = await f.GetThumbnailAsync(ThumbnailMode.SingleItem, 32);
            await obj._icon.SetSourceAsync(thumbnail).AsTask();
            return obj;
        }

        public override string Name => _file.Name;

        public override string Path => _file.Path;

        public override Type Type => Type.File;

        public override BitmapImage Icon => _icon;
    }
}
