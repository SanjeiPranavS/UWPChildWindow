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

        public ObservableCollection<string> VerticalListViewStringSource { get; }=new ObservableCollection<string>();

        public ObservableCollection<string> HorizontalListViewStringSource { get; } = new ObservableCollection<string>();


        private int _verticalHeightItemsCount;
        private int _horizontalWidthItemsCount;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_verticalHeightItemsCount++ % 2 == 0)
            {
                VerticalListViewStringSource.Add("Takes Any FrameWork Element As Content");
                return;

            }
            VerticalListViewStringSource.Add("This is A Resizable Control");

        }

        private void RemoveItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (VerticalListViewStringSource.Count >= 1)
            {
                VerticalListViewStringSource.RemoveAt(0);
            }
        }
        private void FlyoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            TeachingTip.IsOpen = !TeachingTip.IsOpen;
        }

      
        private void HorizontalListViewAddButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (_horizontalWidthItemsCount++ % 2 == 0)
            {
                HorizontalListViewStringSource.Add("Takes Any FrameWork Element As Content");
                return;

            }
            HorizontalListViewStringSource.Add("This is A Resizable Control");
        }

        private void HorizontalListViewRemoveButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (HorizontalListViewStringSource.Count >= 1)
            {
                HorizontalListViewStringSource.RemoveAt(0);
            }
        }
    }
}
