using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Controls;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZTeachingTip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private CoreDispatcher _previewDispatcher;
        //private bool _isDraggingPreview;
       // private UserInteractionMode _currentUserInteractionMode;


        public MainPage()
        {
            var viewSettings = UIViewSettings.GetForCurrentView();

            this.InitializeComponent();

            //PreviewPopupContents.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            //PreviewPopupContents.ManipulationStarted += PreviewPopup_ManipulationStarted;
            //PreviewPopupContents.ManipulationDelta += PreviewPopup_ManipulationDelta;
            //PreviewPopupContents.ManipulationCompleted += PreviewPopup_ManipulationCompleted;

            ApplicationView.GetForCurrentView().Title = "Main View";

            this.Loaded += MainPage_Loaded;
        }

       
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private ZTeachingTip _redRectangleTeachingTip;
        private void RectangleShowButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (_redRectangleTeachingTip is null)
            {
                _redRectangleTeachingTip = new ZTeachingTip
                {
                    IsLightDismissEnabled = false,
                    ZTeachingTipContent = new Rectangle()
                    {
                        Width = 300,
                        Height = 300,
                        Fill = new SolidColorBrush(Windows.UI.Colors.White),
                    },
                    Target = sender as FrameworkElement,
                    PreferredPlacement = ZTeachingTipPlacement.Left,
                    PlacementMargin = new Thickness(10) 
                };

            }
            _redRectangleTeachingTip.IsOpen = !_redRectangleTeachingTip.IsOpen;
        }

        private ZTeachingTip _teachingZTip;
        private void PopupUpCheckButton_OnClick(object sender, RoutedEventArgs e)
        {
            var textboxes = new TextBox
            {
                Text = "Some Content By ZTeaching Tip"
            };
            if (_teachingZTip == null)
            {
                _teachingZTip = new ZTeachingTip
                {
                    ZTeachingTipContent = new PreviewControl(),
                    IsLightDismissEnabled = false,
                    Target = PersonPicture,
                    Padding = new Thickness(0),
                    PreferredPlacement = ZTeachingTipPlacement.Left,
                    PlacementMargin = new Thickness(5),
                    TailBackGround = new SolidColorBrush(Windows.UI.Colors.White),
                    Background = new SolidColorBrush(Windows.UI.Colors.White)
                };
                _teachingZTip.ActualPlacementChanged += _teachingZTip_ActualPlacementChanged;
               
                Bindings.Update();
            }
            _teachingZTip.IsOpen = !_teachingZTip.IsOpen;
        }

        private TeachingTip UiXamlTeachingTip;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //var textboxes = new TextBox
            //{
            //    Text = "Some Content By microsoft Teaching Tip"
            //};
            //popup ??= new Popup()
            //{
            //    Child = new PreviewControl(){Width = 300,Height = 150}, IsLightDismissEnabled = true,MaxWidth = 300,MaxHeight = 150
            //};
            //popup.TryShowNear(sender as FrameworkElement, default, new[]
            //{
            //    PopUpPlacement.Left
            //});
            if (UiXamlTeachingTip == null)
            {
                UiXamlTeachingTip = new TeachingTip();
                LayoutRoot.Children.Add(UiXamlTeachingTip);
            }
            UiXamlTeachingTip.Content = new PreviewControl();
            UiXamlTeachingTip.PreferredPlacement = TeachingTipPlacementMode.Bottom;
            UiXamlTeachingTip.IsLightDismissEnabled = false;
            UiXamlTeachingTip.Target = PersonPicture;
            UiXamlTeachingTip.PlacementMargin = new Thickness(0,30,0,10);
            UiXamlTeachingTip.IsOpen = !UiXamlTeachingTip.IsOpen;

        }

        private void _teachingZTip_ActualPlacementChanged(ZTeachingTip arg1, ActualPlacementChangedEventArgs arg2)
        {
           ActualPlacementTextBox.Text = arg1?.ActualPlacement?.ToString() ?? string.Empty;
        }


        private void MenuFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender == PlacementPreferenceLeftITem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.Left;
                PlacementPreferenceDropDownButton.Content = "Left";
            }
            if (sender == PlacementPrefereneceRightItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.Right;
                PlacementPreferenceDropDownButton.Content = "Right";
            }
            if (sender == PlacementPrefereneceRightTopItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.RightTop;
                PlacementPreferenceDropDownButton.Content = "Right Top";
            }
            if (sender == PlacementPrefereneceLeftTopItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.LeftTop;
                PlacementPreferenceDropDownButton.Content = "Left Top";
            }
            if (sender == PlacementPrefereneceTopItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.Top;
                PlacementPreferenceDropDownButton.Content = " Top";
            }
            if (sender == PlacementPrefereneceBottomItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.Bottom;
                PlacementPreferenceDropDownButton.Content = "Bottom";
            }
            if (sender == PlacementPrefereneceTopLeftItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.TopLeft;
                PlacementPreferenceDropDownButton.Content = "Top Left";
            }
            if (sender == PlacementPrefereneceBottomLeftItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.BottomLeft;
                PlacementPreferenceDropDownButton.Content = "Bottom Left";
            }
            if (sender == PlacementPrefereneceTopRIghtItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.TopRight;
                PlacementPreferenceDropDownButton.Content = "Top Right";
            }
            if (sender == PlacementPrefereneceBottomRightItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.BottomRight;
                PlacementPreferenceDropDownButton.Content = "Bottom Right";
            }
            if (sender == PlacementPrefereneceRightBottomItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.RightBottom;
                PlacementPreferenceDropDownButton.Content = "Right Bottom";
            }
            if (sender == PlacementPrefereneceLeftBottomItem)
            {
                _teachingZTip.PreferredPlacement = ZTeachingTipPlacement.LeftBottom;
                PlacementPreferenceDropDownButton.Content = "Left Bottom";
            }
        }
        private Popup popup;

        private TeachingTip TestTip;

        private TestControl testControl;
        private void ExtensionTestingBUtton_OnClick(object sender, RoutedEventArgs e)
        {

            //if (TestTip is null)
            //{
            //    TestTip = new TeachingTip
            //    {
            //        Content = "Show Something",
            //        PreferredPlacement = TeachingTipPlacementMode.Left,

            //    };
            //    TestTip.Target = sender as FrameworkElement;
            //    //LayoutRoot.Children.Add(TestTip);
            //    TestTip.IsOpen = true;//!TestTip.IsOpen;
            //    return;
            //}
            //TestTip.IsOpen = !TestTip.IsOpen;
            //if (popup is null)
            //{
            //    popup = new Popup();
            //    popup.Child = new PreviewControl()
            //    {
            //        Width = 300, Height = 300,
            //    };
            //    popup.Loaded += Popup_Loaded;
            //    popup.IsLightDismissEnabled = false;
            //    popup.IsOpen = true;
            //}
            //popup.IsOpen = !popup.IsOpen;

            if (testControl is null)
            {
                testControl = new TestControl
                {
                    IsOpen = true
                };
                return;
            }
            testControl.IsOpen = !testControl.IsOpen;
            //testControl.OpenSomePopUp();
        }

        private void Popup_Loaded(object sender, RoutedEventArgs e)
        {
           
        }


        private void ChangeMarginBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(LeftMarginTextBlock.Text, out var leftMargin) &&
                double.TryParse(TopMarginTextBlock.Text, out var topMargin) &&
                double.TryParse(RightMarginTextBlock.Text, out var rightMargin) &&
                double.TryParse(BottomMarginTextBlock.Text, out var bottomMargin))
            {
                var placementOffset = new Thickness(leftMargin, topMargin, rightMargin, bottomMargin);
                _teachingZTip.PlacementMargin = placementOffset;
                InfoTextBlock.Text = string.Empty;
                return;
            }
            InfoTextBlock.Text = "Invalid Margin Format";
        }

       
    }
}

//{
//    if (double.TryParse(LeftMarginTextBlock.Text, out var leftMargin) &&
//            double.TryParse(TopMarginTextBlock.Text, out var topMargin) &&
//            double.TryParse(RightMarginTextBlock.Text, out var rightMargin) &&
//            double.TryParse(BottomMarginTextBlock.Text, out var bottomMargin))
//    {
//        var placementOffset = new Thickness(leftMargin, topMargin, rightMargin, bottomMargin);
//        ZTeachingTip.PlacementOffsetMargin = placementOffset;
//        MarginInfoTextBox.Text = string.Empty;
//        return;
//    }
//    MarginInfoTextBox.Text = "Invalid Margin Format";
//private async void MainPage_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
//{
//    System.Diagnostics.Debug.WriteLine("Main page consolidated: " + args.IsUserInitiated);

//    if (args.IsUserInitiated)
//    {
//        // you could so saving here

//        await HidePreviewWindowAsync(true);
//    }
//}

//private void PreviewPopup_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
//{
//    if (_isDraggingPreview)
//    {
//        e.Handled = true;
//        _isDraggingPreview = false;
//    }
//}

//private void PreviewPopup_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
//{
//    if (_isDraggingPreview)
//    {
//        //PreviewPopup.HorizontalOffset += e.Delta.Translation.X;
//        //PreviewPopup.VerticalOffset += e.Delta.Translation.Y;
//        e.Handled = true;
//    }
//}

//private void PreviewPopup_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
//{
//    _isDraggingPreview = true;

//    e.Handled = true;
//}

// this is called when switching to tablet mode
//private async void MainPage_SizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
//{
//    var viewSettings = UIViewSettings.GetForCurrentView();

//    if (viewSettings.UserInteractionMode == UserInteractionMode.Touch && _currentUserInteractionMode == UserInteractionMode.Mouse)
//    {
//        await HidePreviewWindowAsync(false);

//        PreviewPopup.IsOpen = true;
//    }

//    _currentUserInteractionMode = viewSettings.UserInteractionMode;
//}

//private async System.Threading.Tasks.Task HidePreviewWindowAsync(bool shutdown)
//{
//    if (_previewDispatcher == null)
//    {
//        return;
//    }

//    await _previewDispatcher.RunAsync(CoreDispatcherPriority.High, delegate
//    {
//        _previewDispatcher = null;

//        _previewDispatcher = null;

//        // close the preview window
//        CoreWindow.GetForCurrentThread().Close();

//        if (shutdown)
//        {
//            // do any saving here

//            CoreApplication.Exit();
//        }
//    });
//}

//private async void OnFloat(object sender, RoutedEventArgs e)
//{
//    PreviewPopup.IsOpen = false;

//    var view = CoreApplication.CreateNewView();

//    _previewDispatcher = view.Dispatcher;

//    SplitView.IsPaneOpen = false;

//    var anchorViewId = ApplicationView.GetForCurrentView().Id;

//    await view.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async delegate
//    {
//        var viewId = ApplicationView.GetApplicationViewIdForWindow(CoreWindow.GetForCurrentThread());

//        var frame = new Frame();

//        frame.Navigate(typeof(PreviewPage));

//        Window.Current.Content = frame;

//        Window.Current.Activate();

//        var applicationView = ApplicationView.GetForCurrentView();

//        applicationView.Consolidated += PreviewWindow_Consolidated;

//        applicationView.Title = "Preview";

//        var shown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId, ViewSizePreference.UseMinimum, anchorViewId, ViewSizePreference.Default);

//        applicationView.SetPreferredMinSize(new Windows.Foundation.Size(320, 500));

//        bool resized = applicationView.TryResizeView(new Windows.Foundation.Size(320, 500));

//        System.Diagnostics.Debug.WriteLine($"Shown: {shown}, Resized: {resized}");
//    });
//}

//private async void PreviewWindow_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
//{
//    System.Diagnostics.Debug.WriteLine($"Consolidated: user initiated: {args.IsUserInitiated}");

//    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
//    {
//        OpenPaneButton.IsChecked = false;
//    });
//}

//private void ShowPreview(object sender, RoutedEventArgs e)
//{
//    this.PreviewPopup.IsOpen = true;
//    SplitView.IsPaneOpen = false;
//    //var flyout = new Flyout();
//    //flyout.LightDismissOverlayMode = LightDismissOverlayMode.Off;
//    //flyout.Content = new PreviewControl()
//    //{
//    //    MinHeight = 400,
//    //    MinWidth = 400
//    //};
//    //flyout.ShowAt(sender as FrameworkElement);
//}

//private async void HidePreview(object sender, RoutedEventArgs e)
//{
//    this.PreviewPopup.IsOpen = false;
//    SplitView.IsPaneOpen = false;
//    await HidePreviewWindowAsync(false);
//}

//private void ClosePreviewPopup(object sender, RoutedEventArgs e)
//{TeachingTip
//    PreviewPopup.IsOpen = false;
//}

//private void OpenPane(object sender, RoutedEventArgs e)
//{
//    PreviewPopup.IsOpen = false;
//    SplitView.IsPaneOpen = true;
//}
