using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZTeachingTip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ControlDocumentationPage : Page
    {

        private readonly ZTeachingTip _documentationTeachingTip;

        public ControlDocumentationPage()
        {
            _documentationTeachingTip = new ZTeachingTip();
            _documentationTeachingTip.Closed += ControlDocumentationPage_Closed;
            this.InitializeComponent();
            _documentationTeachingTip.ActualPlacementChanged += _documentationTeachingTip_ActualPlacementChanged;
            Loaded += ControlDocumentationPage_Loaded;
        }

        private void ControlDocumentationPage_Closed(ZTeachingTip arg1, ZTeachingTipClosedEventArgs arg2)
        {
           AssignTEachingTipOpenInfo();
        }


        private void AssignTEachingTipOpenInfo()
        {
            if (_documentationTeachingTip.IsOpen)
            {
                TeachingTipOpeningToggleButton.Content = "Teaching Tip Now Opened";
                return;
            }
            TeachingTipOpeningToggleButton.Content = "Teaching Tip Now Closed";

        }

        private void ControlDocumentationPage_Loaded(object sender, RoutedEventArgs e)
        {
            _documentationTeachingTip.Target = PersonPicture;
            _documentationTeachingTip.ZTeachingTipContent = new ScrollViewer()
            {
              Content  = new PreviewControl(),
              HorizontalScrollMode = ScrollMode.Enabled
            };
            AssignTEachingTipOpenInfo();
             LightDisModeChangedDropDownButton.Content = _documentationTeachingTip.LightDismissMode.ToString();
            PlacementPreferenceDropDownButton.Content = _documentationTeachingTip.PreferredPlacement.ToString();
            TargetHorizontalAlignmentDropDownButton.Content = PersonPicture.HorizontalAlignment.ToString();
            TargetVerticalAlignementDropDownButton.Content = PersonPicture.VerticalAlignment.ToString();
        }

        private void _documentationTeachingTip_ActualPlacementChanged(ZTeachingTip arg1, ActualPlacementChangedEventArgs arg2)
        {
            if (arg2.ActualPlacement == null)
            {
                ActualPlacementNameTextBox.Text = "No Space For Pop To DisPLay PopUp maybe in ShouldBoundToXamlRootTrue";
            }
            else
            {
                ActualPlacementNameTextBox.Text = arg2.ActualPlacement.ToString();
            }
        }

        //Actual Placement
        //ForcePlace
        //IsLightDisMissEnabled
        //LightDismissMode
        //Prefferred placement
        // placement Margin
        //ShouldBound TO Xaml Root
        //Tail Visibility


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (e.AddedItems?.Count > 0)
            {
                case true when sender is ListView listView:
                    switch (listView.SelectedIndex)
                    {
                        case 0:
                            _documentationTeachingTip.LightDismissMode = LightDismissOverlayMode.On;
                            break;
                        case 1:
                            _documentationTeachingTip.LightDismissMode = LightDismissOverlayMode.Off;
                            break;

                        case 2:
                            _documentationTeachingTip.LightDismissMode = LightDismissOverlayMode.Auto;
                            break;
                    }

                    break;
            }
        }

        private void MenuFlyOutItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender == PlacementPreferenceLeftITem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.Left;
                PlacementPreferenceDropDownButton.Content = "Left";
            }
            if (sender == PlacementPrefereneceRightItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.Right;
                PlacementPreferenceDropDownButton.Content = "Right";
            }
            if (sender == PlacementPrefereneceRightTopItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.RightTop;
                PlacementPreferenceDropDownButton.Content = "Right Top";
            }
            if (sender == PlacementPrefereneceLeftTopItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.LeftTop;
                PlacementPreferenceDropDownButton.Content = "Left Top";
            }
            if (sender == PlacementPrefereneceTopItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.Top;
                PlacementPreferenceDropDownButton.Content = " Top";
            }
            if (sender == PlacementPrefereneceBottomItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.Bottom;
                PlacementPreferenceDropDownButton.Content = "Bottom";
            }
            if (sender == PlacementPrefereneceTopLeftItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.TopLeft;
                PlacementPreferenceDropDownButton.Content = "Top Left";
            }
            if (sender == PlacementPrefereneceBottomLeftItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.BottomLeft;
                PlacementPreferenceDropDownButton.Content = "Bottom Left";
            }
            if (sender == PlacementPrefereneceTopRIghtItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.TopRight;
                PlacementPreferenceDropDownButton.Content = "Top Right";
            }
            if (sender == PlacementPrefereneceBottomRightItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.BottomRight;
                PlacementPreferenceDropDownButton.Content = "Bottom Right";
            }
            if (sender == PlacementPrefereneceRightBottomItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.RightBottom;
                PlacementPreferenceDropDownButton.Content = "Right Bottom";
            }
            if (sender == PlacementPrefereneceLeftBottomItem)
            {
               _documentationTeachingTip.PreferredPlacement = ZTeachingTipPlacement.LeftBottom;
                PlacementPreferenceDropDownButton.Content = "Left Bottom";
            }
        }

        private void ChangeDimensionButtonOncLick(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ContentHEightTextBox.Text, out var contentHeight) && double.TryParse(ContentWidthTextBox.Text, out var contentWidth))
            {
                _documentationTeachingTip.ContentHeight = contentHeight;
                _documentationTeachingTip.ContentWidth = contentWidth;
            }
        }

        private void ChangeMaxDimensionButtonOncLick(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(MaxHeightTextBox.Text, out var maxHeight) && double.TryParse(MaxWidthTExtBox.Text, out var maxWidth))
            {
                _documentationTeachingTip.MaxHeight = maxHeight;
                _documentationTeachingTip.MaxWidth = maxWidth;
            }
        }

        private void TargetHoriZontalAlignemtListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PersonPicture is null)
            {
                return;
            }
            if (sender is ListView listView)
            {
                switch (listView.SelectedIndex)
                {
                    case 0:
                        PersonPicture.HorizontalAlignment = HorizontalAlignment.Center;
                        break;
                    case 1:
                        PersonPicture.HorizontalAlignment = HorizontalAlignment.Left;
                        break;
                    case 2:
                        PersonPicture.HorizontalAlignment = HorizontalAlignment.Right;
                        break;
                }
            }
        }

        private void TargetVerticalAlignementListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PersonPicture is null)
            {
                return;
            }
            if (sender is ListView listView)
            {
                switch (listView.SelectedIndex)
                {
                    case 0:
                        PersonPicture.VerticalAlignment = VerticalAlignment.Center;
                        break;
                    case 1:
                        PersonPicture.VerticalAlignment = VerticalAlignment.Top;
                        break;
                    case 2:
                        PersonPicture.VerticalAlignment = VerticalAlignment.Bottom;
                        break;
                }
            }
        }

        private void Remove_Target_OnClick(object sender, RoutedEventArgs e)
        {
           _documentationTeachingTip.Target = null;
        }

        private void ReassignTArget_Button_Click(object sender, RoutedEventArgs e)
        {
            _documentationTeachingTip.Target = PersonPicture;
        }

        private void DocTEachingTip_OPeningToggleButton(object sender, RoutedEventArgs e)
        {
            _documentationTeachingTip.IsOpen = !_documentationTeachingTip.IsOpen;
            AssignTEachingTipOpenInfo();
        }
    }
}
