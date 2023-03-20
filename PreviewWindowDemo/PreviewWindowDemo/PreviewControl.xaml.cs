using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PreviewWindowDemo
{
    public sealed partial class PreviewControl : UserControl
    {
        public PreviewControl()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<string> StringSource { get; }=new ObservableCollection<string>();

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
           StringSource.Add(Guid.NewGuid().ToString());
        }
        private void RemoveItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (StringSource.Count >= 1)
            {
                StringSource.RemoveAt(0);
            }
        }
    }
}
