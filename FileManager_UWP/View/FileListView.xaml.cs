using FileManager_UWP.Server;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace FileManager_UWP.View {
    public class DisplayFileFolderItem {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsFolder { get; set; }
        public DisplayFileFolderItem(StorageFolder f) {
            Name = f.Name;
            Path = f.Path;
            IsFolder = true;
        }
        public DisplayFileFolderItem(StorageFile f) {
            Name = f.Name;
            Path = f.Path;
            IsFolder = false;
        }
        public DisplayFileFolderItem() { }
        
        public static DisplayFileFolderItem parent(string current) {
            DisplayFileFolderItem d = new DisplayFileFolderItem();
            d.Name = "..";
            d.IsFolder = true;
            d.Path = current.Substring(0, current.LastIndexOf('\\'));
            if (d.Path.Length == 2)
                d.Path += "\\";
            return d;
        }
    }
    public sealed partial class FileListView : Page {
        ObservableCollection<DisplayFileFolderItem> Files =
            new ObservableCollection<DisplayFileFolderItem>();
        public FileListView() {
            this.InitializeComponent();
        }

        private async void UpdateFileList(string path) {
            StorageFolder folder = null;
            try {
                folder = await StorageFolder.GetFolderFromPathAsync(path);
            } catch (Exception ex) {
                DebugPanel.Text = ex.Message;
            }

            if (null != folder) {
                TreeViewNode rootNode = new TreeViewNode() { Content = folder.Name };
                IReadOnlyList<StorageFolder> folders = await folder.GetFoldersAsync();
                IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
                Files.Clear();
                Files.Add(DisplayFileFolderItem.parent(path));
                foreach (StorageFolder f in folders) {
                    // DebugPanel.Children.Add(new TextBlock { Text = f.Name });
                    Files.Add(new DisplayFileFolderItem(f));
                }
                foreach (StorageFile f in files) {
                    // DebugPanel.Children.Add(new TextBlock { Text = f.Name });
                    Files.Add(new DisplayFileFolderItem(f));
                    // Files.Add(f);
                }
            }
        }

        private void GetFileList_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            //StorageFolder picturesFolder = KnownFolders.DocumentsLibrary;
            //IReadOnlyList<StorageFile> fileList = await picturesFolder.GetFilesAsync();
            //foreach (StorageFile file in fileList) {
            //    DebugPanel.Children.Add(new TextBlock { Text = file.Name });
            //    Debug.WriteLine(file.Name);
            //}

            string path = pathText.Text;
            UpdateFileList(path);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
            DisplayFileFolderItem i = e.ClickedItem as DisplayFileFolderItem;
            if (i.IsFolder) {
                pathText.Text = i.Path;
                UpdateFileList(i.Path);
            }
        }
    }
}
