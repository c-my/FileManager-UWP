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
    class FileService : IFileService {
        /// <summary>
        /// 获得普通文件夹中的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<List<Displayable>> GetRegularFilesAsync(string path) {
            StorageFolder folder = null;
            try {
                folder = await StorageFolder.GetFolderFromPathAsync(path);
            } catch (FileNotFoundException e) {
                Debug.WriteLine(e.Message);
                throw;
            } catch (AuthenticationException e) {
                Debug.WriteLine(e.Message);
                throw;
            }

            // 打开文件夹
            List<Displayable> displayFileFolderItems =
                new List<Displayable> {await DisplayableFolder.GetParentAsync(folder)};

            // 读取配置文件
            string confPath = folder.Path;
            if (confPath.EndsWith('\\'))
                confPath += ".awesomefilemanager";
            else
                confPath += "\\.awesomefilemanager";
            try
            {
                StorageFile confFile = await StorageFile.GetFileFromPathAsync(confPath);
                string jsonString = await FileIO.ReadTextAsync(confFile);
                JsonObject jsonObject = JsonObject.Parse(jsonString);
                JsonArray virtualFolders = jsonObject.GetNamedArray("virtual_folders");
                foreach (var jsonValue in virtualFolders) {
                    var virtualFolder = jsonValue.GetObject();
                    displayFileFolderItems.Add(new DisplayableSpecial(
                        virtualFolder.GetNamedString("name"), 
                        "", 
                        Type.VirtualFolder, 
                        await IconServer.GetFolderIcon(ThumbnailMode.ListView, 32)));
                }
            } catch (FileNotFoundException) {
                Debug.WriteLine("no configure file in " + confPath);
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

        /// <summary>
        /// 获得磁盘驱动器列表
        /// </summary>
        /// <returns></returns>
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
