using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileManager_UWP.Model {
    /// <summary>
    /// 显示用的文件文件夹统一类
    /// </summary>
    public class DisplayFileFolderItem {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsFolder { get; set; }

        public DisplayFileFolderItem(StorageFolder f) {
            Name = f.Name;
            Path = f.Path;
            IsFolder = true;
        }
        public DisplayFileFolderItem(StorageFile f) {
            Name = f.Name;
            Path = f.Path;
            IsFolder = false;
        }
        public DisplayFileFolderItem() { }

        public static DisplayFileFolderItem parent(string current) {
            DisplayFileFolderItem d = new DisplayFileFolderItem();
            d.Name = "..";
            d.IsFolder = true;
            d.Path = current.Substring(0, current.LastIndexOf('\\'));
            if (d.Path.Length == 2)
                d.Path += "\\";
            return d;
        }
    }
}
