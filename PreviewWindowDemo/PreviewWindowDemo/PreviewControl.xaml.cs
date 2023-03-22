using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ZTeachingTip
{
    public sealed partial class PreviewControl : UserControl
    {
        public PreviewControl()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<string> StringSource { get; }=new ObservableCollection<string>();

        private int _count;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_count++ % 2 == 0)
            {
                StringSource.Add("Takes Any FrameWork Element As Content");
                return;
                
            }
            StringSource.Add("This is A Resizable Control Target");
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
