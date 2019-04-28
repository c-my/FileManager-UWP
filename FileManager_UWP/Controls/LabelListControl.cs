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
            //var itemSource = new ObservableCollection<string>();
            //itemSource.CollectionChanged += ItemsSource_CollectionChanged;
            //ItemsSource = itemSource;
            //Debug.WriteLine(itemSource.ToString());
        }

        private void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            Debug.WriteLine("Add ELEMENT");
            OnApplyTemplate();
        }

        public new ObservableCollection<string> ItemsSource {
            get { return (ObservableCollection<string>)GetValue(ItemsControl.ItemsSourceProperty); }
            set { SetValue(ItemsControl.ItemsSourceProperty, value); }
        }

        public new static readonly DependencyProperty ItemsSourceProperty =
             DependencyProperty.Register(
                 "ItemsSource", typeof(ObservableCollection<string>), typeof(LabelListControl),
                 new PropertyMetadata(null, OnItemsChanged));

        private static void OnItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            Debug.WriteLine("Add Element.");
            LabelListControl target = obj as LabelListControl;
            ObservableCollection<string> oldValue = (ObservableCollection<string>)args.OldValue;
            ObservableCollection<string> newValue = (ObservableCollection<string>)args.NewValue;
            if (oldValue != newValue) {
                target.ItemsSource = newValue;
                target.OnApplyTemplate();
            }
        }

        protected override void OnApplyTemplate() {
            TextBlock textBlock = (TextBlock)GetTemplateChild("LabelTextBlock");
            textBlock.Text = "测试自定义控件\n";
            ListView listView = (ListView)GetTemplateChild("LabelListView");
            //listView.ItemsSource = ItemsSource;
            ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            Debug.WriteLine(ItemsSource.ToString());
            if (ItemsSource != null) {
                textBlock.Text += (ItemsSource as ICollection<string>).Count().ToString();
            } else {
                textBlock.Text += "0";
            }
        }
    }
}
