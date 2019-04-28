using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace FileManager_UWP.Model
{
    public class DisplayableSpecial: Displayable {
        public DisplayableSpecial(string name, string path, Type type, BitmapImage icon)
        {
            Name = name;
            Path = path;
            Type = type;
            Icon = icon;
            Labels = new List<String>();
        }
        public override string Name { get; }
        public override string Path { get; }
        public override Type Type { get; }
        public override BitmapImage Icon { get; }
        public override List<String> Labels { get; }
    }
}
