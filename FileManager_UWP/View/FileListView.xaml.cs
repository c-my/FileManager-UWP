using Windows.UI.Xaml.Controls;
using FileManager_UWP.ViewModel;

namespace FileManager_UWP.View {
    
    public sealed partial class FileListView : Page {
        public FileListView() {
            this.InitializeComponent();
            DataContext = new FileListViewModel();
        }
    }
}
