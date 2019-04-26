using Windows.UI.Xaml.Controls;
using FileManager_UWP.ViewModel;

namespace FileManager_UWP.View {
    
    public sealed partial class FileListView : Page {
        private FileListViewModel vm;
        public FileListView() {
            this.InitializeComponent();
            vm = new FileListViewModel();
            DataContext = vm;
            vm.RefreshCommand.Execute(null);
        }
    }
}
