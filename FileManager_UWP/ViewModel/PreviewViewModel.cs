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
            Path = "C:\\Users\\CaiMY\\Downloads\\tt.doc";
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

        private RelayCommand<String> _showPreviewCommand;
        private RelayCommand _nextPageCommand;
        private RelayCommand _prevPageCommand;


        public RelayCommand<String> ShowPreviewCommand =>
            _showPreviewCommand ?? (_showPreviewCommand = new RelayCommand<String>(async (String path) =>
              {
                  if (path == null)
                      path = "C:\\Users\\CaiMY\\Downloads\\tt.pdf";
                  var preview = await PreviewService.ShowPreviewAsync(path);
                  ImgSource.SetSource(preview);
              }));

        public RelayCommand NextPageCommand =>
            _nextPageCommand ?? (_nextPageCommand = new RelayCommand(async () =>
            {
                if (PreviewService.IsCurrentPDF())
                {
                    var nextPage = await PreviewService.GetNextPageAsync();
                    ImgSource.SetSource(nextPage);
                }
            }));

        public RelayCommand PrevPageCommand =>
            _prevPageCommand ?? (_prevPageCommand = new RelayCommand(async () =>
            {
                if (PreviewService.IsCurrentPDF())
                {
                    var prevPage = await PreviewService.GetPrevPageAsync();
                    ImgSource.SetSource(prevPage);
                }
            }));
    }
}
