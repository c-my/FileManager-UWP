using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace FileManager_UWP.Model
{
    public class DisplayableSpecial: IDisplayable {
        public DisplayableSpecial(string name, string path, bool isFolder, BitmapImage icon)
        {
            Name = name;
            Path = path;
            IsFolder = isFolder;
            Icon = icon;
        }
        public string Name { get; }
        public string Path { get; }
        public bool IsFolder { get; }
        public BitmapImage Icon { get; }
    }
}
