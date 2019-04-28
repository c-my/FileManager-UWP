using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace FileManager_UWP.Model {
    public enum Type{
        Folder = 0, VirtualFolder, Disk, File
    }
    public abstract class Displayable : IComparable {
        public abstract string Name { get; }
        public abstract string Path { get; }
        public abstract Type Type { get; }
        public abstract BitmapImage Icon { get; }
        public abstract List<String> Labels { get; }

        public int CompareTo(object obj) {

            if (this.Type != (obj as Displayable).Type)
                return this.Type - (obj as Displayable).Type;
            if (obj is Displayable another) {
                return Name.CompareTo(another.Name);
            }

            throw new NotSupportedException();
        }
    }
}
