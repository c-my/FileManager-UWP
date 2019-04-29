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
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace FileManager_UWP.Controls {

    public class LabelListControl: ItemsControl {

        //private int _collpse_distance = 22;
        //private List<int> _expanded_position = new List<int>();
        private Storyboard _collpse_story_board = new Storyboard();
        private Storyboard _expand_story_board = new Storyboard();
        private Duration _animate_during = new Duration(TimeSpan.FromMilliseconds(500));
        private int _status = 0; // 0 展开， 1 动画， 2 折叠
        private double _width, _height;
        private Canvas labelListCanvas;


        private ResourceDictionary genericResourceDictionary;
        public LabelListControl() {
            this.DefaultStyleKey = typeof(LabelListControl);
            var uri = new Uri("ms-appx:///Controls/LabelListControl.xaml");
            genericResourceDictionary = new ResourceDictionary();
            Application.LoadComponent(genericResourceDictionary, uri);
        }

        private void ItemsSource_CollectionChanged(object sender, 
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            update();
        }

        private void update() {
            //TextBlock textBlock = (TextBlock)GetTemplateChild("LabelTextBlock");
            //textBlock.Text = "测试自定义控件\n";
            //listView.ItemsSource = ItemsSource;
            var itemsSource = ItemsSource as ObservableCollection<string>;
            //textBlock.Text += itemsSource.Count().ToString();

            //labelListCanvas.Children.Count;
            labelListCanvas.Children.Clear();
            _collpse_story_board.Stop();
            _collpse_story_board.Children.Clear();
            _expand_story_board.Stop();
            _expand_story_board.Children.Clear();
            int expanded_position = 0;
            foreach (string label in itemsSource) {
                Button b = new Button();
                b.Content = label;
                //b.SetValue(Canvas.LeftProperty, labelListCanvas.Children.Count * _collpse_distance);
                b.SetValue(Canvas.LeftProperty, expanded_position);
                b.Style = (Style)genericResourceDictionary["LabelButtonStyle"];

                DoubleAnimation collpse_animate = new DoubleAnimation();
                collpse_animate.From = labelListCanvas.Children.Count * 22;
                collpse_animate.To = expanded_position;
                collpse_animate.Duration = _animate_during;
                collpse_animate.Completed += Animate_Completed;
                _collpse_story_board.Children.Add(collpse_animate);
                Storyboard.SetTarget(collpse_animate, b);
                Storyboard.SetTargetProperty(collpse_animate, "(Canvas.Left)");

                DoubleAnimation expand_animate = new DoubleAnimation();
                expand_animate.From = collpse_animate.To;
                expand_animate.To = collpse_animate.From;
                expand_animate.Duration = _animate_during;
                expand_animate.Completed += Animate_Completed;
                _expand_story_board.Children.Add(expand_animate);
                Storyboard.SetTarget(expand_animate, b);
                Storyboard.SetTargetProperty(expand_animate, "(Canvas.Left)");

                expanded_position += label.Count() * 14 + 20;
                labelListCanvas.Children.Add(b);
            }
        }

        private void Animate_Completed(object sender, object e) {
            measure();
        }

        private void measure() {
            Button lb = labelListCanvas.Children.Last() as Button;
            labelListCanvas.Height = lb.ActualHeight;
            double left = (double)lb.GetValue(Canvas.LeftProperty);
            labelListCanvas.Width = left + lb.ActualHeight;
        }

        private void LabelListCanvas_PointerExited(object sender, PointerRoutedEventArgs e) {
            _expand_story_board.Begin();
            Debug.WriteLine(labelListCanvas.Width.ToString() + "," + labelListCanvas.Height.ToString());
        }

        private void LabelListCanvas_PointerEntered(object sender, PointerRoutedEventArgs e) {
            _collpse_story_board.Begin();
            //labelListCanvas.Width = labelListCanvas.ActualWidth;
            //labelListCanvas.Height = 20;
            Debug.WriteLine(labelListCanvas.Width.ToString() + "," + labelListCanvas.Height.ToString());
        }

        private void init() {
            var itemsSource = ItemsSource as ObservableCollection<string>;
            itemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            Button labelListButton = (Button)GetTemplateChild("LabelListPresenter");
            labelListButton.PointerEntered += LabelListCanvas_PointerEntered;
            labelListButton.PointerExited += LabelListCanvas_PointerExited;
            labelListCanvas = (Canvas)GetTemplateChild("LabelListCanvas");

            //TextBlock textBlock = (TextBlock)GetTemplateChild("LabelTextBlock");
            //textBlock.Text = "测试自定义控件\nINIT";
        }

        protected override void OnApplyTemplate() {
            init();
            update();
        }
    }
}
