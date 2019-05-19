using FileManager_UWP.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Devices.Custom;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Web.Http.Headers;
using Newtonsoft.Json;
using Type = FileManager_UWP.Model.Type;

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
    internal class FileService: IFileService {
        private async Task<List<Displayable>> GetVirtualFolderAsync(string path) {
            // var splitList = path.Split('\\');
            //var virtualFolderName = path.Substring(0, path.LastIndexOf('\\'));
            //path = path.Substring(path.LastIndexOf('\\'));
            var directoryName = Path.GetDirectoryName(path);
            var virtualFolderName = path.Substring(path.LastIndexOf('\\') + 1);

            if (directoryName == null)
                throw new FileNotFoundException();
            
            
            StorageFolder folder;
            folder = await StorageFolder.GetFolderFromPathAsync(directoryName);
            FolderSetting folderSetting = await FolderSetting.GetInstance(folder);
            VirtualFolder virtualFolder = folderSetting.VirtualFolders.Find(x => x.Name == virtualFolderName);
            if (virtualFolder == null)
                throw new FileNotFoundException();
            var displayFileFolderItems = new List<Displayable>
            {
                new DisplayableSpecial(
                    "..",
                    directoryName,
                    Type.Folder,
                    await IconServer.GetFolderIcon(ThumbnailMode.ListView, 32)
                    )
            };

            foreach (var virtualFilderItem in virtualFolder.Includes) {
                displayFileFolderItems.AddRange(
                    await GetRegularFilesAsync(virtualFilderItem, false)
                );
            }                    
            return displayFileFolderItems;
        }

        /// <summary>
        /// 获得普通文件夹中的文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="withParent"></param>
        /// <returns></returns>
        private async Task<List<Displayable>> GetRegularFilesAsync(string path, bool withParent = true) {
            StorageFolder folder = null;
            try {
                folder = await StorageFolder.GetFolderFromPathAsync(path);
            } catch (System.UnauthorizedAccessException) {
                throw;
            } catch (Exception e) {
                Debug.WriteLine(e.Message + " " + path + " try to get Virtual Folder");
                try {
                    return await GetVirtualFolderAsync(path);
                } catch (Exception) {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }

            // 打开文件夹
            var displayFileFolderItems = new List<Displayable>();
            if (withParent)
                displayFileFolderItems.Add(await DisplayableFolder.GetParentAsync(folder));

            // 读取配置文件

            FolderSetting folderSetting = await FolderSetting.GetInstance(folder);
            foreach (var virtualFolder in folderSetting.VirtualFolders) {
                displayFileFolderItems.Add(new DisplayableSpecial(
                    virtualFolder.Name,
                    folder.Path + "\\" + virtualFolder.Name,
                    Type.VirtualFolder,
                    await IconServer.GetFolderIcon(ThumbnailMode.ListView, 32)));
            }

            IReadOnlyList<StorageFolder> folders = await folder.GetFoldersAsync();
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            //displayFileFolderItems.AddRange(folders
            //    .Select(async i => await DisplayFileFolderItem.GetInstance(i))
            //    .Select(i => i.Result)
            //);
            //displayFileFolderItems.AddRange(files
            //    .Select(async i => await DisplayFileFolderItem.GetInstance(i))
            //    .Select(i => i.Result)
            //);
            try {
                foreach (var file in folders)
                    displayFileFolderItems.Add(
                        await DisplayableFolder.GetInstanceAsync(file));

                foreach (var file in files)
                    displayFileFolderItems.Add(
                        await DisplayableFile.GetInstanceAsync(file));
            } catch (Exception e) {
                Debug.WriteLine("Exception at appending dispalyFileFolder " + e.Message);
            }

            return displayFileFolderItems;
        }

        /// <summary>
        /// 获得磁盘驱动器列表
        /// </summary>
        /// <returns></returns>
        private async Task<List<Displayable>> GetDiskDrivesAsync() {
            var drives = DriveInfo.GetDrives();
            var ans = new List<Displayable>();
            foreach (var d in drives) ans.Add(await DisplayableDisk.GetInstance(d));

            return ans;
        }

        /// <summary>
        /// 获得可显示的目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件和文件夹列表</returns>
        public async Task<List<Displayable>> GetDisplayFileFolderList(string path) {
            try {
                List<Displayable> ans;
                if (path == "/")
                    ans = await GetDiskDrivesAsync();
                else
                    ans = await GetRegularFilesAsync(path);
                ans.Sort();
                return ans;

            } catch (Exception) {
                throw;
            }
        }
    }
}
