using System;
using Windows.UI.Xaml.Media.Imaging;

namespace FileManager_UWP.Model {
    public enum Type{
        File, Folder, Disk, VirtualFolder
    }
    public abstract class Displayable : IComparable {
        public abstract string Name { get; }
        public abstract string Path { get; }
        public abstract Type Type { get; }
        public abstract BitmapImage Icon { get; }

        public int CompareTo(object obj) {
            throw new NotImplementedException();
        }
    }
}
