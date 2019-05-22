using CommonServiceLocator;
using DataAccessLibrary.Service;
using FileManager_UWP.Service;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_UWP.ViewModel {
    public class ViewModelLocator {
        public FileListViewModel FileListViewModel => ServiceLocator.Current.GetInstance<FileListViewModel>();
        public MainPageViewModel MainPageViewModel => ServiceLocator.Current.GetInstance<MainPageViewModel>();
        public PreviewViewModel PreviewViewModel => ServiceLocator.Current.GetInstance<PreviewViewModel>();
        // public LabelService LabelService => ServiceLocator.Current.GetInstance<LabelService>();

        public ViewModelLocator() {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            LabelService.InitializeDatabase();

            SimpleIoc.Default.Register<FileListViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<PreviewViewModel>();

            SimpleIoc.Default.Register<IFileService, FileService>();
        }
    }
}
