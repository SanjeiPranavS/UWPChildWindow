using System;
using System.Collections.Generic;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZTeachingTip
{
    public sealed partial class TestControl : UserControl
    {
        public TestControl()
        {
            this.InitializeComponent();
            InnerPOpup.Loaded += InnerPOpup_Loaded;
            InnerPOpup.Closed += InnerPOpup_Closed;
        }

        private void InnerPOpup_Closed(object sender, object e)
        {
           Debug.WriteLine("InnerPopup Clsoe is Called");
        }

        private void InnerPOpup_Loaded(object sender, RoutedEventArgs e)
        {
           Debug.WriteLine($"InnerPop Up is Loaded and is open set to{InnerPOpup.IsOpen}");
        }

        public readonly static DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen), typeof(bool), typeof(TestControl), new PropertyMetadata(default(bool),(PropertyChangedCallback)));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           var cntrl = d as TestControl;
           cntrl.IsOPenChanged();
        }
        private void IsOPenChanged()
        {
            if (IsOpen)
            {
                InnerPOpup.IsOpen = true;
                return;
            }
            InnerPOpup.IsOpen = false;
        }

        public bool IsOpen
        {
            get
            {
                return (bool)GetValue(IsOpenProperty);
            }
            set
            {
                SetValue(IsOpenProperty, value);
            }
        }

        Popup _popup;
        public void OpenSomePopUp()
        {
            if (_popup == null)
            {
                _popup = new Popup();
                _popup.Child = new PreviewControl();
                _popup.IsOpen = true;
                return;
            }
            _popup.IsOpen = !_popup.IsOpen;
        }
    }
}
