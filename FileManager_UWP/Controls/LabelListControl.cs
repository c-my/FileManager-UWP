using FileManager_UWP.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
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
    public class LabelListChangeEvent {
        public enum EventType { Add, Remove };

        public LabelListControl LabelListControl;
        public EventType eventType;
        public LabelItem label;
    }

    /// <summary>
    /// 一个可以自动折叠的标签菜单
    /// </summary>
    public class LabelListControl: ItemsControl {

        // public delegate void OnLabelChanged(LabelItem item);
        public RelayCommand<LabelListChangeEvent> OnAddLabel = null, OnRemoveLabel = null;

        //private int _collpse_distance = 22;
        //private List<int> _expanded_position = new List<int>();
        private Storyboard _collpse_story_board = new Storyboard();
        private Storyboard _expand_story_board = new Storyboard();
        private Duration _animate_during = new Duration(TimeSpan.FromMilliseconds(200));
        private Canvas labelListCanvas;
        private bool _adding_new_label = false;
        private bool _hovering = false, _draging = false;
        private int shit_cnt = 0;

        private ResourceDictionary genericResourceDictionary;
        public LabelListControl() {
            this.DefaultStyleKey = typeof(LabelListControl);
            var uri = new Uri("ms-appx:///Controls/LabelListControl.xaml");
            genericResourceDictionary = new ResourceDictionary();
            Application.LoadComponent(genericResourceDictionary, uri);
            Loaded += LabelListControl_Loaded;
            //AllowDrop = true;
            //RegisterPropertyChangedCallback(ItemsControl.ItemsSourceProperty, this.tbTagChangedCallback);
        }

        private void LabelListControl_Loaded(object sender, RoutedEventArgs e) {
            init();
            update();
        }

        private void tbTagChangedCallback(DependencyObject sender, DependencyProperty dp) {
            if (dp == ItemsControl.ItemsSourceProperty) {
                // These lines produce the same result.
                Debug.WriteLine("The tag has been set to " + sender.ToString());
                Debug.WriteLine("The tag has been set to " + sender.GetValue(dp));
                ItemsSource = sender.GetValue(dp);
                shit_cnt++;
                if (shit_cnt >= 2) {
                    init();
                    update();
                }
            }
        }

        /// <summary>
        /// 监听ItemSource改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemsSource_CollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            update();
        }

        private void SetElasticAnimate(Button b, int expanded_position) {
            DoubleAnimation expand_animate = new DoubleAnimation();
            expand_animate.From = null; // labelListCanvas.Children.Count * 22;
            expand_animate.To = expanded_position;
            expand_animate.Duration = _animate_during;
            //expand_animate.Completed += Animate_Completed;
            _expand_story_board.Children.Add(expand_animate);
            Storyboard.SetTarget(expand_animate, b);
            Storyboard.SetTargetProperty(expand_animate, "(Canvas.Left)");

            DoubleAnimation collpse_animate = new DoubleAnimation();
            collpse_animate.From = null; // expanded_position;
            collpse_animate.To = labelListCanvas.Children.Count * 20;
            collpse_animate.Duration = _animate_during;
            //collpse_animate.Completed += Animate_Completed;
            _collpse_story_board.Children.Add(collpse_animate);
            Storyboard.SetTarget(collpse_animate, b);
            Storyboard.SetTargetProperty(collpse_animate, "(Canvas.Left)");
        }

        private DependencyProperty LabelProperty = DependencyProperty.Register(
            "LabelItem", typeof(LabelItem), typeof(Button), null);

        /// <summary>
        /// 当ItemsSource改变时，绘制标签，注册动画
        /// </summary>
        private void update() {
            Debug.WriteLine("ObservableCollection update");
            var itemsSource = ItemsSource as ObservableCollection<LabelItem>;

            labelListCanvas.Children.Clear();
            _collpse_story_board.Stop();
            _collpse_story_board.Children.Clear();
            _expand_story_board.Stop();
            _expand_story_board.Children.Clear();
            int expanded_position = 0;
            foreach (LabelItem label in itemsSource) {
                Button b = new Button();
                b.SetValue(LabelProperty, label);
                b.Content = label.tag;
                b.Background = new SolidColorBrush(label.color);
                b.SetValue(Canvas.LeftProperty, expanded_position);
                b.Style = (Style)genericResourceDictionary["LabelButtonStyle"];
                b.Click += B_Click;
                //b.AddHandler(PointerPressedEvent, new PointerEventHandler(B_PointerPressed), true);
                //b.AddHandler(PointerReleasedEvent, new PointerEventHandler(B_PointerReleased), true);
                InitManipulationTransforms(b);
                SetElasticAnimate(b, expanded_position);
                expanded_position += label.tag.Count() * 14 + 15;
                labelListCanvas.Children.Add(b);
            }
            Button add_button = new Button();
            SetElasticAnimate(add_button, expanded_position);
            add_button.Content = "+";
            add_button.SetValue(Canvas.LeftProperty, expanded_position);
            add_button.Style = (Style)genericResourceDictionary["LabelButtonStyle"];
            add_button.Click += Add_Click;
            DoubleAnimation appear_animate = new DoubleAnimation {
                To = 100,
                Duration = _animate_during
            };
            appear_animate.Completed += Animate_Completed;
            _expand_story_board.Children.Add(appear_animate);
            Storyboard.SetTarget(appear_animate, add_button);
            Storyboard.SetTargetProperty(appear_animate, "Opacity");
            DoubleAnimation disappear_animate = new DoubleAnimation {
                Duration = _animate_during
            };
            // 如果没有标签，则+不自动隐藏
            if (itemsSource.Count > 0)
                disappear_animate.To = 0;
            else
                disappear_animate.To = 100;
            disappear_animate.Completed += Animate_Completed;
            _collpse_story_board.Children.Add(disappear_animate);
            Storyboard.SetTarget(disappear_animate, add_button);
            Storyboard.SetTargetProperty(disappear_animate, "Opacity");

            labelListCanvas.Children.Add(add_button);
            measure();
            LabelListCanvas_PointerExited(null, null);
        }

        //private bool _pressing_label = false;
        //private void B_PointerPressed(object sender, PointerRoutedEventArgs e) {
        //    Debug.WriteLine("label pressed");
        //    _pressing_label = true;
        //}

        //private void B_PointerReleased(object sender, PointerRoutedEventArgs e) {
        //    Debug.WriteLine("label release");
        //    _pressing_label = false;
        //}

        private void B_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) {
            TransformGroup transforms = (sender as Button).RenderTransform as TransformGroup;
            MatrixTransform previousTransform = transforms.Children[0] as MatrixTransform;
            CompositeTransform deltaTransform = transforms.Children[1] as CompositeTransform;
            previousTransform.Matrix = transforms.Value;

            // Get center point for rotation
            Point center = previousTransform.TransformPoint(new Point(e.Position.X, e.Position.Y));
            deltaTransform.CenterX = center.X;
            deltaTransform.CenterY = center.Y;

            // Look at the Delta property of the ManipulationDeltaRoutedEventArgs to retrieve
            // the rotation, scale, X, and Y changes
            deltaTransform.Rotation = e.Delta.Rotation;
            deltaTransform.TranslateX = e.Delta.Translation.X;
            deltaTransform.TranslateY = e.Delta.Translation.Y;
        }

        private void InitManipulationTransforms(Button b) {
            var transforms = new TransformGroup();
            var previousTransform = new MatrixTransform();
            previousTransform.Matrix = Matrix.Identity;
            var deltaTransform = new CompositeTransform();
            transforms.Children.Add(previousTransform);
            transforms.Children.Add(deltaTransform);

            b.RenderTransform = transforms;
            b.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            b.ManipulationDelta += B_ManipulationDelta;
            b.ManipulationStarted += B_ManipulationStarted;
            b.ManipulationCompleted += B_ManipulationCompleted;
        }

        private Point _origin;
        private void B_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) {
            Debug.WriteLine("Manipulation finish");
            _draging = false;
            ObservableCollection<LabelItem> itemsSource = ItemsSource as ObservableCollection<LabelItem>;
            var toRemove = (sender as Button).GetValue(LabelProperty) as LabelItem;
            itemsSource.Remove(toRemove);
            update();
            OnRemoveLabel?.Execute(new LabelListChangeEvent {
                label = toRemove,
                LabelListControl = this,
                eventType = LabelListChangeEvent.EventType.Remove
            });
        }

        private void B_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) {
            Debug.WriteLine("Manipulation start");
            _origin = e.Position;
            _draging = true;
        }

        private void B_DragLeave(object sender, DragEventArgs e) {
            var itemsSource = ItemsSource as ObservableCollection<LabelItem>;
            Button b = sender as Button;
            var tag = b.Content as string;
            Debug.WriteLine("Drag Leave " + tag);
            int index = itemsSource.ToList().FindIndex(x => x.tag == tag);
            itemsSource.RemoveAt(index);
        }

        /// <summary>
        /// 点击标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            Debug.WriteLine(b.Content);
        }
        /// <summary>
        /// 点击新建标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e) {
            Debug.WriteLine("Click Add Label");
            Button b = sender as Button;
            TextBox textBox = new TextBox();
            textBox.Height = b.ActualHeight;
            textBox.FontSize = b.FontSize;
            textBox.BorderThickness = new Thickness(0);
            textBox.Padding = new Thickness(4, 4, 4, 4);
            textBox.Margin = new Thickness(0, 0, 0, 0);
            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;
            b.Content = textBox;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
            _adding_new_label = false;
            TextBox tb = sender as TextBox;
            var NewLabel = new LabelItem(tb.Text);
            if (tb.Text.Count() != 0)
                (ItemsSource as ObservableCollection<LabelItem>).Add(NewLabel);
            if (!_hovering)
                LabelListCanvas_PointerExited(null, null);
            // 调用回调函数
            OnAddLabel?.Execute(new LabelListChangeEvent {
                label = NewLabel,
                LabelListControl = this,
                eventType = LabelListChangeEvent.EventType.Add
            });
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
            _adding_new_label = true;
        }

        /// <summary>
        /// 动画完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Animate_Completed(object sender, object e) {
            measure();
        }
        /// <summary>
        /// 计算控件尺寸
        /// </summary>
        private void measure() {
            Button lb = labelListCanvas.Children.Last() as Button;
            lb.Content = "+";
            labelListCanvas.Height = lb.ActualHeight;
            double left = (double)lb.GetValue(Canvas.LeftProperty);
            labelListCanvas.Width = left + lb.ActualWidth;
        }
        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelListCanvas_PointerExited(object sender, PointerRoutedEventArgs e) {
            _hovering = false;
            // 正在输入新标签时不折叠
            if (!_adding_new_label && !_draging) {
                _collpse_story_board.Begin();
                for (int i = 0; i < labelListCanvas.Children.Count - 2; i++) {
                    Button b = labelListCanvas.Children[i] as Button;
                    b.Content = string.Format("{0}...", (b.Content as string)[0]);
                }
            }
        }
        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelListCanvas_PointerEntered(object sender, PointerRoutedEventArgs e) {
            _hovering = true;
            if (!_adding_new_label && !_draging) {
                _expand_story_board.Begin();
                var labels = ItemsSource as ObservableCollection<LabelItem>;
                for (int i = 0; i < labels.Count - 1; i++) {
                    Button b = labelListCanvas.Children[i] as Button;
                    b.Content = labels[i].tag;
                }
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void init() {
            Debug.WriteLine("INIT LABEL LIST CONTROL");
            var itemsSource = ItemsSource as ObservableCollection<LabelItem>;
            if (ItemsSource != null && itemsSource == null) {
                ItemsSource = new ObservableCollection<LabelItem>(ItemsSource as IEnumerable<LabelItem>);
                itemsSource = ItemsSource as ObservableCollection<LabelItem>;
            }
            //var itemsSource = GetValue(ItemsControl.ItemsSourceProperty) as ObservableCollection<LabelItem>;
            itemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            Button labelListButton = (Button)GetTemplateChild("LabelListPresenter");
            labelListButton.PointerEntered += LabelListCanvas_PointerEntered;
            labelListButton.PointerExited += LabelListCanvas_PointerExited;
            labelListCanvas = (Canvas)GetTemplateChild("LabelListCanvas");
        }

        /// <summary>
        /// 该Control被加载时调用
        /// </summary>
        protected override void OnApplyTemplate() {
            //shit_cnt++;
            //if (shit_cnt >= 2) {

            //}
        }

        //public new static readonly DependencyProperty ItemsSourceProperty = //Notice this is static. It's bound to an internal static hash table of some sort; I use to know exactly but forgot.
        //DependencyProperty.Register(
        //    "ItemsSources", 
        //    /*The name of the property to register against. 
        //     * The static version is always the name of the property ended with Property
        //     * i.e. SomeValue property is SomeValueProperty dependency property */
        //    typeof(ObservableCollection<LabelItem>), //This is the type used to describe the property.
        //    typeof(LabelListControl), //This is the type the dependency property relates to. 

        //    /* Below this is the magic. It's where we supply the property meta data and can be delivered different ways.
        //    * For this example I will supply only the default value and the event we want to use when the value is changed.
        //    * Note: 
        //    * The event we supply is fired ONLY if the value is changed. This event is what we need to use to handle changes in the setter to cover binding operations as well. */
        //    new PropertyMetadata(default(ObservableCollection<LabelItem>),  
        //        /* This is the default value the dependency property will have. 
        //         * It can be whatever you decide but make sure it works with the same type or you'll most likely get an error. */
        //        /* This is the event fired when the value changes.
        //         * Note: Dependency properties binding and events always operate on the UI thread. Cross threading will throw exceptions. */
        //        new PropertyChangedCallback((s, e) => {
        //            var ctr = s as LabelListControl; //The sender of the callback will always be of the type it's from.

        //            /* The values given from the e argument for OldValue and NewValue should be of type int in this example...
        //             * but since we can't gaurantee the property is setup properly before here I always add a check. */
        //             if (e.OldValue != e.NewValue) {
        //                ctr.ItemsSource = e.NewValue;
        //            }
        //            //if (e.OldValue is int oldValue) { }
        //            //if (e.NewValue is int newValue) {
        //            //    /* Now do what you want with the information.  This is where you need to do custom work instead of using the setter.
        //            //     * Note: If you need to work on something in the MainPage remember this is a static event and you'll need to refer to the sender or s value in this case.
        //            //     * I've converted s to the variable mainPage for easy referencing here. */
        //            //     ctr.ItemsSource = newValue;  //Note: The custom control should bind to this as well via XAML making this pointless. I set the value here just for educational purposes.
        //            //}
        //        })));

    }
}
