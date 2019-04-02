using Windows.UI.Xaml.Media.Imaging;

namespace FileManager_UWP.Model
{
    public interface IDisplayable {
        string Name { get; }
        string Path { get; }
        bool IsFolder { get; }
        BitmapImage Icon { get; }
        //string Name();
        //string Path();
        //bool IsFolder();
        //BitmapImage Icon();
    }
}
