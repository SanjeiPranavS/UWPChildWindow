using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZTeachingTip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DummyPage : Page
    {
        public DummyPage()
        {
            this.InitializeComponent();
        }
        private void DummyPage_OnLoaded(object sender, RoutedEventArgs e)
        {
           
            TeachingTip.Content = new PreviewControl();
            TeachingTip.PreferredPlacement = TeachingTipPlacementMode.Left;
            TeachingTip.IsLightDismissEnabled = false;
            TeachingTip.Closing += TeachingTip_Closing;
        }

        private void TeachingTip_Closing(TeachingTip sender, TeachingTipClosingEventArgs args)
        {
            throw new System.NotImplementedException();
        }

        private void TargetBUtotn_OnClick(object sender, RoutedEventArgs e)
        {
            TeachingTip.IsOpen = !TeachingTip.IsOpen;
        }
    }
}
