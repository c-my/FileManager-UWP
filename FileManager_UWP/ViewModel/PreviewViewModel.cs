using FileManager_UWP.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace FileManager_UWP.ViewModel
{
    class PreviewViewModel :ViewModelBase//, INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public PreviewViewModel()
        {
            _source = new BitmapImage();
            Path = "C:\\Users\\CaiMY\\Downloads\\Square.png";
        }

        private BitmapImage _source;
        public BitmapImage ImgSource
        {
            get => _source;
            set
            {
                Set(nameof(ImgSource), ref _source, value);
                //this.OnPropertyChanged();
            }
        }

        private string _path;
        public string Path
        {
            get => _path;
            set => Set(nameof(Path), ref _path, value);
        }

        private RelayCommand _showPreviewCommand;

        public RelayCommand ShowPreviewCommand =>
            _showPreviewCommand ?? (_showPreviewCommand = new RelayCommand(async () =>
              {
                  var preview = await PreviewService.ShowPreviewAsync(Path);
                  ImgSource.SetSource(preview);
              }));

        //public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    // Raise the PropertyChanged event, passing the name of the property whose value has changed.
        //    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
