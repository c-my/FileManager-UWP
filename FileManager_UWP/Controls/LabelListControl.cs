using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace FileManager_UWP.Controls {

    public class LabelListControl: ItemsControl {
        public LabelListControl() {
            this.DefaultStyleKey = typeof(LabelListControl);
        }

        private void ItemsSource_CollectionChanged(object sender, 
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            TextBlock textBlock = (TextBlock)GetTemplateChild("LabelTextBlock");
            textBlock.Text = "测试自定义控件\n";
            //listView.ItemsSource = ItemsSource;
            var itemsSource = ItemsSource as ObservableCollection<string>;
            textBlock.Text += itemsSource.Count().ToString();

            Canvas labelListCanvas = (Canvas)GetTemplateChild("LabelListCanvas");
            //labelListCanvas.Children.Count;
            labelListCanvas.Children.Clear();
            foreach (string label in itemsSource) {
                Button b = new Button();
                b.Content = label;
                b.SetValue(Canvas.LeftProperty, labelListCanvas.Children.Count * 10);
                labelListCanvas.Children.Add(b);
            }
        }

        private void update() {
            
        }

        private void init() {
            var itemsSource = ItemsSource as ObservableCollection<string>;
            itemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            TextBlock textBlock = (TextBlock)GetTemplateChild("LabelTextBlock");
            textBlock.Text = "测试自定义控件\nINIT";
        }

        protected override void OnApplyTemplate() {
            init();
        }
    }
}
