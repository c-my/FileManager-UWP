using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace FileManager_UWP.Model
{
    public class FolderSetting {
        public List<VirtualFolder> VirtualFolders { get; set; }

        public FolderSetting() {
            VirtualFolders = new List<VirtualFolder>();
        }

        static public async Task<FolderSetting> GetInstance(StorageFolder folder) {
            var confPath = folder.Path;
            if (confPath.EndsWith('\\'))
                confPath += ".awesomefilemanager";
            else
                confPath += "\\.awesomefilemanager";
            FolderSetting folderSetting;
            try {
                var confFile = await StorageFile.GetFileFromPathAsync(confPath);
                var jsonString = await FileIO.ReadTextAsync(confFile);
                folderSetting = JsonConvert.DeserializeObject<FolderSetting>(jsonString);
            } catch (FileNotFoundException) {
                Debug.WriteLine("no configure file in " + confPath);
                folderSetting = new FolderSetting();
            }
            return folderSetting;
        }
    }
}
