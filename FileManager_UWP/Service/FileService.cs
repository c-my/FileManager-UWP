using FileManager_UWP.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Custom;
using Windows.Storage;
using Windows.Web.Http.Headers;

namespace FileManager_UWP.Service {
    internal interface IFileService {
        /// <summary>
        /// 获得可显示的目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件和文件夹列表</returns>
        Task<List<Displayable>> GetDisplayFileFolderList(string path);
    }

    /// <summary>
    /// 负责提供本地文件及文件夹访问服务
    /// </summary>
    class FileService : IFileService {
        /// <summary>
        /// 获得普通文件夹中的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<List<Displayable>> GetRegularFilesAsync(string path) {
            StorageFolder folder = null;
            try
            {
                folder = await StorageFolder.GetFolderFromPathAsync(path);
                IReadOnlyList<StorageFolder> folders = await folder.GetFoldersAsync();
                IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
                List<Displayable> displayFileFolderItems =
                    new List<Displayable> { await DisplayableFolder.GetParentAsync(folder) };
                //displayFileFolderItems.AddRange(folders
                //    .Select(async i => await DisplayFileFolderItem.GetInstance(i))
                //    .Select(i => i.Result)
                //);
                //displayFileFolderItems.AddRange(files
                //    .Select(async i => await DisplayFileFolderItem.GetInstance(i))
                //    .Select(i => i.Result)
                //);

                foreach (var file in folders) {
                    displayFileFolderItems.Add(
                        await DisplayableFolder.GetInstanceAsync(file));
                }

                foreach (var file in files) {
                    displayFileFolderItems.Add(
                        await DisplayableFile.GetInstanceAsync(file));
                }

                return displayFileFolderItems;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task<List<Displayable>> GetDiskDrivesAsync() {
            var drives = System.IO.DriveInfo.GetDrives();
            List<Displayable> ans = new List<Displayable>();
            foreach (var d in drives)
            {
                ans.Add(await DisplayableDisk.GetInstance(d));
            }

            return ans;
        }
        /// <summary>
        /// 获得可显示的目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件和文件夹列表</returns>
        public async Task<List<Displayable>> GetDisplayFileFolderList(string path) {
            if (path == "/")
                return await GetDiskDrivesAsync();
            return await GetRegularFilesAsync(path);
        }
    }
}
