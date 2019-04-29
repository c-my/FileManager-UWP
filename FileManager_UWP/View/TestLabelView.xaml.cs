using FileManager_UWP.Model;
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
        public ObservableCollection<LabelItem> Labels =
            new ObservableCollection<LabelItem>{
                new LabelItem("照片"),
                new LabelItem("土耳其摔跤"),
                new LabelItem("李若明"),
                new LabelItem("鸢晓曼"),
                new LabelItem("才明洋与吴岳江进行土耳其摔跤"),
                new LabelItem("真正的瑜伽大师")
            };

        public TestLabelView() {
            this.InitializeComponent();
            Debug.WriteLine(Labels.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            switch (Labels.Count) {
                case 0:
                    Labels.Add(new LabelItem("照片"));
                    break;
                case 1:
                    Labels.Add(new LabelItem("土耳其摔跤"));
                    break;
                case 2:
                    Labels.Add(new LabelItem("李若明"));
                    break;
                case 3:
                    Labels.Add(new LabelItem("鸢晓曼"));
                    break;
                default:
                    Labels.Add(new LabelItem("才明洋与吴岳江摔跤"));
                    break;
            }
            Debug.WriteLine(Labels.Count.ToString());
        }
    }
}
