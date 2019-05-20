using DataAccessLibrary.Service;
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
    public class DisplayableDisk: Displayable {
        private readonly DriveInfo _drive;
        private readonly BitmapImage _icon = new BitmapImage();
        private readonly List<LabelItem> _labels;

        private DisplayableDisk(DriveInfo d) {
            _drive = d;
            _labels = LabelService.GetLabels(d.Name).Select((x) => new LabelItem(x)).ToList();

        }

        public static async Task<DisplayableDisk> GetInstance(DriveInfo d) {
            var obj = new DisplayableDisk(d);
            StorageFolder f = await StorageFolder.GetFolderFromPathAsync(d.Name);
            var thumbnail = await f.GetThumbnailAsync(ThumbnailMode.ListView, 32);
            await obj._icon.SetSourceAsync(thumbnail).AsTask();

            return obj;
        }

        public override string Name => _drive.Name;
        public override string Path => _drive.Name;
        public override BitmapImage Icon => _icon;
        public override Type Type => Type.Disk;
        public override List<LabelItem> Labels => _labels;
    }
}
