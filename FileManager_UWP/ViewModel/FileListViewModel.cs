using FileManager_UWP.Model;
using FileManager_UWP.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileManager_UWP.ViewModel {
    public class FileListViewModel : ViewModelBase {

        /// <summary>
        /// 当前路径
        /// </summary>
        private string _path = "C:\\";
        public string Path {
            get => _path;
            set => Set(nameof(Path), ref _path, value);
        }

        /// <summary>
        /// 文件和文件夹列表
        /// </summary>
        private IEnumerable<DisplayFileFolderItem> _displayFileFolderItems;
        public IEnumerable<DisplayFileFolderItem> DisplayFileFolderItems {
            get => _displayFileFolderItems;
            set => Set(nameof(DisplayFileFolderItem), ref _displayFileFolderItems, value);
        }

        /// <summary>
        /// 刷新命令
        /// </summary>
        public RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => {
                var todoItemService = new FileService();
                var todoItemList = await todoItemService.GetDisplayFileFolderList(Path);
                DisplayFileFolderItems = todoItemList;
                foreach (var i in DisplayFileFolderItems)
                    Debug.WriteLine(i.Name);
                Debug.WriteLine(Path + todoItemList.Count);
            }));
    }
}
