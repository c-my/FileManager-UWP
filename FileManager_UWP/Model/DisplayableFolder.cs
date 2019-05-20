using DataAccessLibrary.Service;
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
    public class DisplayableFolder: Displayable {
        private readonly StorageFolder _folder;
        private readonly BitmapImage _icon;
        private readonly List<LabelItem> _labels;

        private DisplayableFolder(StorageFolder f) {
            _folder = f;
            _icon = new BitmapImage();
            _labels = LabelService.GetLabels(f.Path).Select((x) => new LabelItem(x)).ToList();
        }

        public static async Task<DisplayableFolder> GetInstanceAsync(StorageFolder f)
        {
            DisplayableFolder obj = new DisplayableFolder(f);
            var thumbnail = await f.GetThumbnailAsync(ThumbnailMode.ListView, 32);
            await obj._icon.SetSourceAsync(thumbnail).AsTask();
            return obj;
        }

        public static async Task<Displayable> GetParentAsync(StorageFolder f) {
            string path = System.IO.Path.GetDirectoryName(f.Path);
            BitmapImage img = new BitmapImage();
            if (path != null) {
                path = f.Path.Substring(0, f.Path.LastIndexOf('\\'));
                StorageFolder file = await StorageFolder.GetFolderFromPathAsync(path);
                var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.ListView, 32);
                await img.SetSourceAsync(thumbnail).AsTask();
                return new DisplayableSpecial("..", file.Path, Model.Type.Folder, img);
            }
            else {
                var thumbnail = await f.GetThumbnailAsync(ThumbnailMode.ListView, 32);
                await img.SetSourceAsync(thumbnail).AsTask();
                return new DisplayableSpecial("..", "/", Model.Type.Folder, img);
            }
        }

        public override string Name => _folder.Name;
        public override string Path => _folder.Path;
        public override Type Type => Type.Folder;
        public override BitmapImage Icon => _icon;
        public override List<LabelItem> Labels => _labels;
    }
}
