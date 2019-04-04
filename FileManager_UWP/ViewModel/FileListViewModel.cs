using FileManager_UWP.Model;
using FileManager_UWP.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Type = FileManager_UWP.Model.Type;

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
        private IEnumerable<Displayable> _displayFileFolderItems;
        public IEnumerable<Displayable> DisplayFileFolderItems {
            get => _displayFileFolderItems;
            set => Set(nameof(DisplayFileFolderItems), ref _displayFileFolderItems, value);
        }

        private string _debugText;
        public string DebugText {
            get => _debugText;
            set => Set(nameof(DebugText), ref _debugText, value);
        }

        /// <summary>
        /// 刷新命令
        /// </summary>
        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => {
                var fileService = new FileService();
                try {
                    var fileList = await fileService.GetDisplayFileFolderList(Path);
                    DisplayFileFolderItems = fileList;
                } catch (Exception e) {
                    Debug.WriteLine(e.Message);
                }
            }));

        private object _listSelectedItem;
        public object ListSelectedItem {
            get => _listSelectedItem;
            set => Set(nameof(ListSelectedItem), ref _listSelectedItem, value);
        }

        private List<object> _listSelectedItems;
        public List<object> ListSelectedItems {
            get => _listSelectedItems;
            set => Set(nameof(ListSelectedItems), ref _listSelectedItems, value);
        }

        /// <summary>
        /// 刷新命令
        /// </summary>
        private RelayCommand _doubleTappedCommand;
        public RelayCommand DoubleTappedCommand =>
            _doubleTappedCommand ?? (_doubleTappedCommand = new RelayCommand(
                () =>
                {
                    Displayable i = ListSelectedItem as Displayable;
                    Debug.WriteLine("Double tapped");
                    if (i != null && (i.Type == Type.Folder || i.Type == Type.Disk))
                    {
                        Debug.WriteLine("double tapped: " + i.Name);
                        Path = i.Path;
                        RefreshCommand.Execute(null);
                    }
            }));

    }
}
