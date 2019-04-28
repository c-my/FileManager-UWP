using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace FileManager_UWP.Controls {
    public sealed class LabelListControl: Control {
        public LabelListControl() {
            this.DefaultStyleKey = typeof(LabelListControl);
        }

        protected override void OnApplyTemplate() {
            ((TextBlock)GetTemplateChild("LabelTextBlock")).Text = "测试自定义控件";
        }
    }
}
