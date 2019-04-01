using FileManager_UWP.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http.Headers;

namespace FileManager_UWP.Service {
    /// <summary>
    /// 负责提供本地文件及文件夹访问服务
    /// </summary>
    class FileService {
        /// <summary>
        /// 获得可显示的目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件和文件夹列表</returns>
        public async Task<List<DisplayFileFolderItem>> GetDisplayFileFolderList(string path) {
            StorageFolder folder = null;
            try {
                folder = await StorageFolder.GetFolderFromPathAsync(path);
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw;
            }
            IReadOnlyList<StorageFolder> folders = await folder.GetFoldersAsync();
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            //List<DisplayFileFolderItem> displayFileFolderItems = 
            //    folders.Select(async i =>
            //        await DisplayFileFolderItem.GetInstance(i));
            //displayFileFolderItems.AddRange(files.Select(
            //    i => new DisplayFileFolderItem(i)));
            // displayFileFolderItems.AddRange(files.Select(DisplayFileFolderItem.getInstance) as IEnumerable<DisplayFileFolderItem>);

            List<DisplayFileFolderItem> displayFileFolderItems = 
                new List<DisplayFileFolderItem> {await DisplayFileFolderItem.parent(path)};
            //displayFileFolderItems.AddRange(folders
            //    .Select(async i => await DisplayFileFolderItem.GetInstance(i))
            //    .Select(i => i.Result)
            //);
            //displayFileFolderItems.AddRange(files
            //    .Select(async i => await DisplayFileFolderItem.GetInstance(i))
            //    .Select(i => i.Result)
            //);
            foreach (var file in folders)
            {
                displayFileFolderItems.Add(
                    await DisplayFileFolderItem.GetInstance(file));
            }
            foreach (var file in files)
            {
                displayFileFolderItems.Add(
                    await DisplayFileFolderItem.GetInstance(file));
            }

            return displayFileFolderItems;
        }
    }
}
