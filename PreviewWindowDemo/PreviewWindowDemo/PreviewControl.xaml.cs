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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
           StringSource.Add("Takes Any FrameWork Element As Content");
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
