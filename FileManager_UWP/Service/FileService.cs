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

namespace FileManager_UWP.Service
{
    internal interface IFileService
    {
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
    internal class FileService : IFileService
    {
        private async Task<List<Displayable>> GetVirtualFolderAsync(string path)
        {
            var splitList = path.Split('|');
            var virtualFolderName = splitList[1];
            path = splitList[0];
            var directoryName = Path.GetDirectoryName(path);

            if (directoryName == null)
                throw new FileNotFoundException();
            var confPath = directoryName;
            if (confPath.EndsWith('\\'))
                confPath += ".awesomefilemanager";
            else
                confPath += "\\.awesomefilemanager";
            try
            {
                StorageFolder folder;
                try
                {
                    folder = await StorageFolder.GetFolderFromPathAsync(directoryName);
                }
                catch (FileNotFoundException e)
                {
                    throw;
                }
                var displayFileFolderItems = new List<Displayable>
                {
                    new DisplayableSpecial(
                        "..",
                        directoryName,
                        Type.Folder,
                        await IconServer.GetFolderIcon(ThumbnailMode.ListView, 32)
                        )
                };

                var confFile = await StorageFile.GetFileFromPathAsync(confPath);
                var jsonString = await FileIO.ReadTextAsync(confFile);

                FolderSetting folderSetting = JsonConvert.DeserializeObject<FolderSetting>(jsonString);
                foreach (var virtualFolder in
                    folderSetting.VirtualFolders.Find(x => x.Name == virtualFolderName).Includes)
                {
                    displayFileFolderItems.AddRange(
                        await GetRegularFilesAsync(virtualFolder, false)
                    );
                }
                return displayFileFolderItems;
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("no configure file in " + confPath);

                throw;
            }
        }

        /// <summary>
        /// 获得普通文件夹中的文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="withParent"></param>
        /// <returns></returns>
        private async Task<List<Displayable>> GetRegularFilesAsync(string path, bool withParent = true)
        {
            StorageFolder folder = null;
            try
            {
                folder = await StorageFolder.GetFolderFromPathAsync(path);
            }
            catch (System.UnauthorizedAccessException e)
            {
                throw;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + " " + path + " try to get Virtual Folder");
                try
                {
                    return await GetVirtualFolderAsync(path);
                }
                catch (Exception)
                {
                    Debug.WriteLine(e.Message);

                    throw;
                }
            }

            // 打开文件夹
            var displayFileFolderItems = new List<Displayable>();
            if (withParent)
                displayFileFolderItems.Add(await DisplayableFolder.GetParentAsync(folder));

            // 读取配置文件
            var confPath = folder.Path;
            if (confPath.EndsWith('\\'))
                confPath += ".awesomefilemanager";
            else
                confPath += "\\.awesomefilemanager";
            try
            {
                var confFile = await StorageFile.GetFileFromPathAsync(confPath);
                var jsonString = await FileIO.ReadTextAsync(confFile);
                FolderSetting folderSetting = JsonConvert.DeserializeObject<FolderSetting>(jsonString);
                foreach (var virtualFolder in folderSetting.VirtualFolders)
                {
                    displayFileFolderItems.Add(new DisplayableSpecial(
                        virtualFolder.Name,
                        confPath + "|" + virtualFolder.Name,
                        Type.VirtualFolder,
                        await IconServer.GetFolderIcon(ThumbnailMode.ListView, 32)));
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("no configure file in " + confPath);
            }

            var folders = await folder.GetFoldersAsync();
            var files = await folder.GetFilesAsync();
            //displayFileFolderItems.AddRange(folders
            //    .Select(async i => await DisplayFileFolderItem.GetInstance(i))
            //    .Select(i => i.Result)
            //);
            //displayFileFolderItems.AddRange(files
            //    .Select(async i => await DisplayFileFolderItem.GetInstance(i))
            //    .Select(i => i.Result)
            //);

            foreach (var file in folders)
                displayFileFolderItems.Add(
                    await DisplayableFolder.GetInstanceAsync(file));

            foreach (var file in files)
                displayFileFolderItems.Add(
                    await DisplayableFile.GetInstanceAsync(file));

            return displayFileFolderItems;
        }

        /// <summary>
        /// 获得磁盘驱动器列表
        /// </summary>
        /// <returns></returns>
        private async Task<List<Displayable>> GetDiskDrivesAsync()
        {
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
        public async Task<List<Displayable>> GetDisplayFileFolderList(string path)
        {
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
