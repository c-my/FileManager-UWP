using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FileManager_UWP.View {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestLabelView: Page {
        public ObservableCollection<string> Labels = new ObservableCollection<string>{
            "照片", "土耳其摔跤", "李若明", "鸢晓曼", "才明洋与吴岳江进行土耳其摔跤", "真正的瑜伽大师"};

        public TestLabelView() {
            this.InitializeComponent();
            Debug.WriteLine(Labels.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            switch (Labels.Count) {
                case 0:
                    Labels.Add("照片");
                    break;
                case 1:
                    Labels.Add("土耳其摔跤");
                    break;
                case 2:
                    Labels.Add("李若明");
                    break;
                case 3:
                    Labels.Add("鸢晓曼");
                    break;
                default:
                    Labels.Add("才明洋与吴岳江摔跤");
                    break;
            }
            Debug.WriteLine(Labels.Count.ToString());
        }
    }
}
