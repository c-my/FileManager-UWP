using Windows.UI.Xaml.Controls;
using FileManager_UWP.ViewModel;
using System.Collections.ObjectModel;
using FileManager_UWP.Model;
using System.Diagnostics;
using FileManager_UWP.Controls;
using GalaSoft.MvvmLight.Ioc;

namespace FileManager_UWP.View {
    
    public sealed partial class FileListView : Page {

        //public ObservableCollection<LabelItem> Labels =
        //    new ObservableCollection<LabelItem>{
        //        new LabelItem("照片"),
        //        new LabelItem("土耳其摔跤"),
        //        new LabelItem("李若明"),
        //        new LabelItem("鸢晓曼"),
        //        new LabelItem("才明洋与吴岳江进行土耳其摔跤"),
        //        new LabelItem("真正的瑜伽大师")
        //    };

        private FileListViewModel vm;
        public FileListView() {
            this.InitializeComponent();
            vm = SimpleIoc.Default.GetInstance<FileListViewModel>();
            // vm = new FileListViewModel();
            DataContext = vm;
            vm.RefreshCommand.Execute(null);
            vm.PropertyChanged += Vm_PropertyChanged;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            Debug.WriteLine(e.PropertyName);
            if (e.PropertyName == "DisplayFileFolderItems") {
                var items = (sender as FileListViewModel).DisplayFileFolderItems;
                MicroShitList.Items.Clear();
                foreach (var displayFileItem in items) {
                    StackPanel sp = new StackPanel();
                    sp.Height = 44;
                    sp.Padding = new Windows.UI.Xaml.Thickness(12);
                    sp.Orientation = Orientation.Horizontal;
                    // TODO：滥用Tag
                    sp.SetValue(StackPanel.TagProperty, displayFileItem);
                    
                    Image Icon = new Image();
                    Icon.Source = displayFileItem.Icon;
                    Icon.Height = 16;
                    Icon.Width = 16;
                    Icon.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                    TextBlock Name = new TextBlock();
                    Name.Text = displayFileItem.Name;
                    Name.Margin = new Windows.UI.Xaml.Thickness(12, 0, 0, 0);

                    LabelListControl labels = new LabelListControl();
                    labels.ItemsSource = displayFileItem.Labels;
                    // labels.Padding = new Windows.UI.Xaml.Thickness(0, -50, 0, 0);
                    labels.SetValue(LabelListControl.TagProperty, displayFileItem.Path);
                    labels.OnRemoveLabel = vm.LabelRemoveCommand;
                    labels.OnAddLabel = vm.LabelAddCommand;


                    sp.Children.Add(Icon);
                    sp.Children.Add(Name);
                    sp.Children.Add(labels);
                    MicroShitList.Items.Add(sp);
                }
            }
        }
    }
}
