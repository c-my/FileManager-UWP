using FileManager_UWP.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileManager_UWP.Service {
    /// <summary>
    /// 负责提供本地文件及文件夹访问服务
    /// </summary>
    class FileService {
        public async Task<List<DisplayFileFolderItem>> GetDisplayFileFolderList(string path) {
            StorageFolder folder = null;
            try {
                folder = await StorageFolder.GetFolderFromPathAsync(path);
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
            IReadOnlyList<StorageFolder> folders = await folder.GetFoldersAsync();
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            List<DisplayFileFolderItem> DisplayFileFolderItems;
            DisplayFileFolderItems = folders.Select(i => new DisplayFileFolderItem(i)).ToList();
            DisplayFileFolderItems.AddRange(files.Select(i => new DisplayFileFolderItem(i)));
            return DisplayFileFolderItems;
        }
    }
}
