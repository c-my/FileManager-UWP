using FileManager_UWP.Service;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using FileManager_UWP.ViewModel;

namespace FileManager_UWP.View {
    
    public sealed partial class FileListView : Page {
        //ObservableCollection<DisplayFileFolderItem> Files =
        //    new ObservableCollection<DisplayFileFolderItem>();
        public FileListView() {
            this.InitializeComponent();
            DataContext = new FileListViewModel();

            //pathText.DataContext = DataContext;
            //pathText.SetBinding()
        }

        //private async void UpdateFileList(string path) {
        //    StorageFolder folder = null;
        //    try {
        //        folder = await StorageFolder.GetFolderFromPathAsync(path);
        //    } catch (Exception ex) {
        //        DebugPanel.Text = ex.Message;
        //    }

        //    if (null != folder) {
        //        TreeViewNode rootNode = new TreeViewNode() { Content = folder.Name };
        //        IReadOnlyList<StorageFolder> folders = await folder.GetFoldersAsync();
        //        IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
        //        Files.Clear();
        //        Files.Add(DisplayFileFolderItem.parent(path));
        //        foreach (StorageFolder f in folders) {
        //            // DebugPanel.Children.Add(new TextBlock { Text = f.Name });
        //            Files.Add(new DisplayFileFolderItem(f));
        //        }
        //        foreach (StorageFile f in files) {
        //            // DebugPanel.Children.Add(new TextBlock { Text = f.Name });
        //            Files.Add(new DisplayFileFolderItem(f));
        //            // Files.Add(f);
        //        }
        //    }
        //}

        private void GetFileList_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            //StorageFolder picturesFolder = KnownFolders.DocumentsLibrary;
            //IReadOnlyList<StorageFile> fileList = await picturesFolder.GetFilesAsync();
            //foreach (StorageFile file in fileList) {
            //    DebugPanel.Children.Add(new TextBlock { Text = file.Name });
            //    Debug.WriteLine(file.Name);
            //}

            //string path = pathText.Text;
            //UpdateFileList(path);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
            //DisplayFileFolderItem i = e.ClickedItem as DisplayFileFolderItem;
            //if (i.IsFolder) {
            //    pathText.Text = i.Path;
            //    UpdateFileList(i.Path);
            //}
        }

        private void ListView_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e) {
            //DisplayFileFolderItem i = FileList.SelectedItem as DisplayFileFolderItem;
            //if (i.IsFolder) {
            //    pathText.Text = i.Path;
            //    UpdateFileList(i.Path);
            //}
        }
    }
}
