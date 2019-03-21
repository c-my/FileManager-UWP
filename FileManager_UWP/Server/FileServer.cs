using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileManager_UWP.Server {
    /// <summary>
    /// 负责提供本地文件及文件夹访问服务
    /// </summary>
    class FileServer {
        public async Task<IReadOnlyList<StorageFile>> getFileList() {
            StorageFolder picturesFolder = KnownFolders.DocumentsLibrary;
            IReadOnlyList<StorageFile> fileList = await picturesFolder.GetFilesAsync();
            return fileList;
        }
    }
}
