using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZTeachingTip
{
    public sealed partial class ZTeachingTip : UserControl
    {

        #region Dependendcy PRoperty
        private ZTeachingTipPlacement? _actualPlacement;
        private double _targetWidth;
        private double _targetHeight;
        private double _popUpWidth;
        private double _popUpHeight;

        public readonly static DependencyProperty IsLightDismissEnabledProperty = DependencyProperty.Register(
            nameof(IsLightDismissEnabled), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(true, LightDismissPropertyChanged));


        public readonly static DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(false, TeachingTipClosingChanged));



        public readonly static DependencyProperty ShouldBoundToXamlRootProperty = DependencyProperty.Register(
            nameof(ShouldBoundToXamlRoot), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(true, ShouldBoundToXamlRootChangedCallBack));


        public readonly static DependencyProperty TeachingTipContentProperty = DependencyProperty.Register(
            nameof(TeachingTipContent), typeof(FrameworkElement), typeof(ZTeachingTip), new PropertyMetadata(default, TeachingTipContentChanged));


        public readonly static DependencyProperty TargetProperty = DependencyProperty.Register(
            nameof(Target), typeof(FrameworkElement), typeof(ZTeachingTip), new PropertyMetadata(default(FrameworkElement), (TargetPropertyChangedCallBack)));

        public readonly static DependencyProperty PlacementOffsetMarginProperty = DependencyProperty.Register(
            nameof(PlacementOffsetMargin), typeof(Thickness), typeof(ZTeachingTip), new PropertyMetadata(new Thickness(0), PlacementOffsetMarginPropertyChangedCallBack));


        public readonly static DependencyProperty PreferredPlacementProperty = DependencyProperty.Register(
            nameof(PreferredPlacement), typeof(ZTeachingTipPlacement), typeof(ZTeachingTip), new PropertyMetadata(ZTeachingTipPlacement.LeftTop, (PlacementPReferenceOnPropertyChanged)));


        public readonly static DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register(
            nameof(CloseButtonStyle), typeof(Style), typeof(ZTeachingTip), new PropertyMetadata(default(Style)));

        public readonly static DependencyProperty LightDismissModeProperty = DependencyProperty.Register(
            nameof(LightDismissMode), typeof(LightDismissOverlayMode), typeof(ZTeachingTip), new PropertyMetadata(LightDismissOverlayMode.Auto, OnLightDismissModePropertyChanged));

        public ZTeachingTipPlacement PreferredPlacement
        {
            get => (ZTeachingTipPlacement)GetValue(PreferredPlacementProperty);
            set => SetValue(PreferredPlacementProperty, value);
        }

        public LightDismissOverlayMode LightDismissMode
        {
            get => (LightDismissOverlayMode)GetValue(LightDismissModeProperty);
            set => SetValue(LightDismissModeProperty, value);
        }

        public Thickness PlacementOffsetMargin
        {
            get => (Thickness)GetValue(PlacementOffsetMarginProperty);
            set => SetValue(PlacementOffsetMarginProperty, value);
        }

        public bool ShouldBoundToXamlRoot
        {
            get => (bool)GetValue(ShouldBoundToXamlRootProperty);
            set => SetValue(ShouldBoundToXamlRootProperty, value);
        }

        public Style CloseButtonStyle
        {
            get => (Style)GetValue(CloseButtonStyleProperty);
            set => SetValue(CloseButtonStyleProperty, value);
        }

        public FrameworkElement Target
        {
            get => (FrameworkElement)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public FrameworkElement TeachingTipContent
        {
            get => (FrameworkElement)GetValue(TeachingTipContentProperty);
            set => SetValue(TeachingTipContentProperty, value);
        }

        public bool IsLightDismissEnabled
        {
            get => (bool)GetValue(IsLightDismissEnabledProperty);
            set => SetValue(IsLightDismissEnabledProperty, value);
        }


        public ZTeachingTipPlacement? ActualPlacement
        {
            get => _actualPlacement;
            private set
            {
                _actualPlacement = value;
                ActualPlacementChanged?.Invoke(this, new ActualPlacementChangedEventArgs(_actualPlacement));
            }
        }

        public event Action<ZTeachingTip, ActualPlacementChangedEventArgs> ActualPlacementChanged;

        public Action<object, RoutedEventArgs> CloseButtonClicked;

        #endregion

        #region PropertyChangedCallBack


        private static void PlacementPReferenceOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip && e.OldValue is ZTeachingTipPlacement placementOLdValue && e.NewValue is ZTeachingTipPlacement placementNewValue)
            {
                if (placementNewValue != placementOLdValue)
                {
                    teachingTip.PlacePreferenceChanged();
                }
            }
        }
        private static void TargetPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip tip && e.NewValue is FrameworkElement newTarget)
            {
                newTarget.SizeChanged += tip.ContentElement_SizeChanged;
            }
            if (d is ZTeachingTip teachingTip && e.OldValue is FrameworkElement oldTarget)
            {
                oldTarget.SizeChanged -= teachingTip.ContentElement_SizeChanged;
            }
        }

        private static void TeachingTipClosingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip && e.OldValue is bool isOpenOLdValue && e.NewValue is bool isOpenNewValue)
            {
                if (isOpenNewValue != isOpenOLdValue)
                {
                    teachingTip.IsOpenPropertyChanged();
                }
            }
        }

        private static void ShouldBoundToXamlRootChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip && e.NewValue is bool oldValue && e.NewValue is bool newValue)
            {
                if (oldValue != newValue)
                {
                    teachingTip.ShouldBoundToXamlRootChanged();
                }
            }
        }

        private static void OnLightDismissModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip && e.NewValue is LightDismissOverlayMode newValue && e.OldValue is LightDismissOverlayMode oldValue)
            {
                if (oldValue != newValue)
                {
                    teachingTip.OnLightDismissModeChanged(newValue);
                }
            }
        }

        private static void TeachingTipContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip && e.NewValue is FrameworkElement newElement)
            {
                if (!newElement.Equals(e.OldValue as FrameworkElement))
                {
                    teachingTip.ContentChanged(newElement);
                    newElement.SizeChanged += teachingTip.ContentElement_SizeChanged;
                }
                if (e.OldValue is FrameworkElement oldContentElement)
                {
                    oldContentElement.SizeChanged -= teachingTip.ContentElement_SizeChanged;
                }
            }
        }
        private static void PlacementOffsetMarginPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip)
            {
                teachingTip.OnPlacementMarginOffsetChanged();
            }
        }

        private static void LightDismissPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip && e.NewValue is bool newValue && e.OldValue is bool oldValue)
            {
                if (oldValue != newValue)
                {
                    teachingTip.OnLightDismissChanged();
                }
            }
        }
        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            PositionPopUp();
        }
        private void CoreWindowResizeCompleted(CoreWindow sender, object args)
        {
            PositionPopUp();
        }


        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            CloseButtonClicked?.Invoke(this, e);
            IsOpen = false;
        }

        #endregion


        public ZTeachingTip()
        {
            this.InitializeComponent();
            ZTeachingTipPopUp.Closed += ZTeachingTipPopUp_Closed;
            PlacementOffsets = PopulateOffsets();

            RootGrid.Loaded += RootGrid_Loaded;
            RootGrid.Translation += new Vector3(0, 0, 35);
            RootGrid.SizeChanged += ContentElement_SizeChanged;
        }

        #region ArrangementAndInitialPositioningCalaculation

        private void ContentChanged(FrameworkElement newContentElement)
        {
            RootContentPresenter.Content = newContentElement;
            RootGrid.Measure(new Size(double.MaxValue, double.MaxValue));
            if (newContentElement.IsLoaded)
            {
                _popUpHeight = RootGrid.ActualHeight;
                _popUpWidth = RootGrid.ActualWidth;
                return;
            }
            _popUpHeight = RootGrid.DesiredSize.Height;
            _popUpWidth = RootGrid.DesiredSize.Width;
        }

        //Each And Every Time POp up is opened
        private void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            _popUpHeight = RootGrid.ActualHeight;
            _popUpWidth = RootGrid.ActualWidth;
            AssignTargetDimentionIfExist();
            PositionPopUp();
        }

        #endregion


        private void IsOpenPropertyChanged()
        {
            if (IsOpen)
            {
                SubscribeToSizeChangeNotification();
                PositionPopUp();
                return;
            }
            UnSubscribeToSizeChangeNotification();
        }
        private void UnSubscribeToSizeChangeNotification()
        {
            Window.Current.SizeChanged -= WindowSizeChanged;
            CoreWindow.GetForCurrentThread().ResizeCompleted -= CoreWindowResizeCompleted;
            CoreWindow.GetForCurrentThread().SizeChanged -= CoreWindowResizeCompleted;

        }
        private void SubscribeToSizeChangeNotification()
        {
            UnSubscribeToSizeChangeNotification();
            Window.Current.SizeChanged += WindowSizeChanged;
            CoreWindow.GetForCurrentThread().ResizeCompleted += CoreWindowResizeCompleted;
            CoreWindow.GetForCurrentThread().SizeChanged += CoreWindowResizeCompleted;

        }
        private void OnLightDismissModeChanged(LightDismissOverlayMode dissDismissMode)
        {
            ZTeachingTipPopUp.LightDismissOverlayMode = dissDismissMode;
        }
        private void ContentElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement contentElement)
            {
                _popUpHeight = RootGrid.ActualHeight;
                _popUpWidth = RootGrid.ActualWidth;
                AssignTargetDimentionIfExist();
                PositionPopUp();
            }
        }
        private void AssignTargetDimentionIfExist()
        {
            if (Target == null)
            {
                return;
            }
            _targetHeight = Target.ActualHeight;
            _targetWidth = Target.ActualWidth;
        }

        private void ShouldBoundToXamlRootChanged()
        {
            PositionPopUp();
        }


        private void PlacePreferenceChanged()
        {
            PositionPopUp();
        }

        /// <summary>
        /// Change The Visual State From LightDismiss To NormalDismiss or ViseVersa Changes
        /// </summary>
        private void OnLightDismissChanged()
        {
            //LightDismiss Behaviour cannot be changed when Teaching Tip is Opened Else It will Stuck On Screen 
            //So Change is Applied once The Control is Closed
            if (IsOpen)
            {
                ZTeachingTipPopUp.Closed += LightDismissOnPopClosedEventHandler;
                return;
            }
            ChangeUiBehaviourBasedOnLightDismiss(IsLightDismissEnabled);

            void LightDismissOnPopClosedEventHandler(object sender, object e)
            {
                ZTeachingTipPopUp.Closed -= LightDismissOnPopClosedEventHandler;
                ChangeUiBehaviourBasedOnLightDismiss(IsLightDismissEnabled);

            }

            void ChangeUiBehaviourBasedOnLightDismiss(bool isLightDismissEnabled)
            {
                ZTeachingTipPopUp.IsLightDismissEnabled = isLightDismissEnabled;
                if (isLightDismissEnabled)
                {
                    VisualStateManager.GoToState(this, nameof(LightDismissEnabled), false);
                    return;
                }
                VisualStateManager.GoToState(this, nameof(LightDismissDisabled), false);
            }
        }


        private void OnPlacementMarginOffsetChanged()
        {
            PositionPopUp();
        }

        private void ZTeachingTipPopUp_Closed(object sender, object e)
        {

        }


        #region PopUpPlacementLogicRegion

        class ZTeachingTipOffset
        {
            public double HorizontalOffSet { get; set; }

            public double VerticalOffSet { get; set; }

            public bool IsFittingWithinBounds { get; set; }
        }
        enum SidePreference
        {
            Left,
            Right,
            Top,
            Bottom,
        }
        enum Alignment
        {
            Center,
            Right,
            Left,
            Top,
            Bottom,
        }
        private List<ZTeachingTipPlacement> PopulateOffsets()
        {
            return new List<ZTeachingTipPlacement>()
            {
                { ZTeachingTipPlacement.TopLeft },
                { ZTeachingTipPlacement.Top },
                { ZTeachingTipPlacement.TopRight },
                { ZTeachingTipPlacement.RightTop },
                { ZTeachingTipPlacement.Right },
                { ZTeachingTipPlacement.RightBottom },
                { ZTeachingTipPlacement.BottomRight } ,
                { ZTeachingTipPlacement.Bottom },
                { ZTeachingTipPlacement.BottomLeft },
                { ZTeachingTipPlacement.LeftBottom },
                { ZTeachingTipPlacement.Left },
                { ZTeachingTipPlacement.LeftTop }

            };
        }


        private List<ZTeachingTipPlacement> PlacementOffsets { get; }


        private void PositionPopUp()
        {
            var isTeachingTipFit = TeachingTipContent is null ? PositionPopUpUnTargeted() : PositionPopUpBasedOnTarget(Target);
            if (!ShouldBoundToXamlRoot || isTeachingTipFit)
            {
                return;
            }//if Control should be contained within xaml root but size not enough to show so  Hiding the PopUp to Avoid Clipping
            ActualPlacement = null;
            ZTeachingTipPopUp.IsOpen = false;
        }

        private bool PositionPopUpUnTargeted()
        {
            return true;
        }
        private bool PositionPopUpBasedOnTarget(FrameworkElement targetElement)
        {

            var calculatedOffset = DetermineSuitablePlacementPreference();

            ZTeachingTipPopUp.HorizontalOffset = calculatedOffset.HorizontalOffSet + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;
            ZTeachingTipPopUp.VerticalOffset = calculatedOffset.VerticalOffSet + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom;

            if (!calculatedOffset.IsFittingWithinBounds)
            {
                return false;
            }

            return calculatedOffset.IsFittingWithinBounds;

        }
        // PrintOffsetDetails(calculatedOffset);
        //ZTeachingTipPopUp.TryShowNear(targetElement, default, PlacementPreferenceOrders.Top, VerticalAlignmentPreferenceOrders.TopCenterBottom,HorizontalAlignmentPreferenceOrders.Center, 0, true);

        private ZTeachingTipOffset DetermineSuitablePlacementPreference()
        {

            var requestedOffset = CalculatePlacementOffsetForPopUp(PreferredPlacement);


            if (!ShouldBoundToXamlRoot || requestedOffset.IsFittingWithinBounds)
            {
                ActualPlacement = PreferredPlacement;
                return requestedOffset;
            }

            var startIndex = PlacementOffsets.IndexOf(PreferredPlacement);

            for (int i = 0; i < PlacementOffsets.Count - 1; i++)
            {
                if (startIndex >= PlacementOffsets.Count - 1)
                {
                    startIndex = -1;
                }
                var offsetForPlacement = CalculatePlacementOffsetForPopUp(PlacementOffsets[++startIndex]);

                if (offsetForPlacement.IsFittingWithinBounds)
                {
                    ActualPlacement = PlacementOffsets[startIndex];
                    return offsetForPlacement;
                }
            }
            return requestedOffset;
        }

#if DEBUG
        private void PrintOffsetDetails(ZTeachingTipOffset preferredOffset)
        {
            Debug.WriteLine("================================================================================================================");
            Debug.WriteLine($"Offset Calculation By ZTeaching Tip X = {preferredOffset.HorizontalOffSet} prefered offset Y = {preferredOffset.VerticalOffSet}");
            Debug.WriteLine($"Pop UP Dimentions  Height = {PopUpCoordinatesInCoreWindowSpace.Height} Width ='{PopUpCoordinatesInCoreWindowSpace.Width}");
            Debug.WriteLine($"pop up Cordinates in ZTeachingTip X= {PopUpCoordinatesInCoreWindowSpace.X} Y = {PopUpCoordinatesInCoreWindowSpace.Y}");
            Debug.WriteLine($"Pop Vetical And Horizontal OFfset Vertical={ZTeachingTipPopUp.VerticalOffset} Horizontal = {ZTeachingTipPopUp.HorizontalOffset}");
            Debug.WriteLine($"Target Cordinates X= {TargetCoordinatesInCoreWindowSpace.X}  Y = {TargetCoordinatesInCoreWindowSpace.Y}");
        }
#endif



        private ZTeachingTipOffset CalculatePlacementOffsetForPopUp(ZTeachingTipPlacement RequestedPlacement)
        {

            var distanceX = TargetCoordinatesInCoreWindowSpace.X - PopUpCoordinatesInCoreWindowSpace.X;
            var distanceY = TargetCoordinatesInCoreWindowSpace.Y - PopUpCoordinatesInCoreWindowSpace.Y;
            var placementOffset = new ZTeachingTipOffset();

            switch (RequestedPlacement)
            {

                case ZTeachingTipPlacement.Top:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Top);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Center, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case ZTeachingTipPlacement.TopLeft:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Top);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Left, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case ZTeachingTipPlacement.TopRight:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Top);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Right, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;

                case ZTeachingTipPlacement.Bottom:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Bottom);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Center, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case ZTeachingTipPlacement.BottomRight:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Bottom);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Right, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case ZTeachingTipPlacement.BottomLeft:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Bottom);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Left, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case ZTeachingTipPlacement.Left:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Left);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Center, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);

                    break;
                case ZTeachingTipPlacement.LeftTop:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Left);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Top, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);
                    break;
                case ZTeachingTipPlacement.LeftBottom:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Left);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Bottom, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);

                    break;
                case ZTeachingTipPlacement.Right:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Right);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Center, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);

                    break;
                case ZTeachingTipPlacement.RightTop:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Right);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Top, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);
                    break;
                case ZTeachingTipPlacement.RightBottom:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Right);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Bottom, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);
                    break;

            }
            CheckIfSpaceForPopupPositioningAvailableAvailable(placementOffset, RequestedPlacement, distanceX, distanceY);

            return placementOffset;
        }
        private void CheckIfSpaceForPopupPositioningAvailableAvailable(ZTeachingTipOffset placementOffset, ZTeachingTipPlacement preferredPlacement, double distanceX, double distanceY)
        {
            bool hasVerticalSpace = default;
            bool hasHorizontalSpace = default;
            var verticalOffset = placementOffset.VerticalOffSet - VerticalMarginDeviation();
            var horizontalOffset = placementOffset.HorizontalOffSet - HorizontalMarginDeviation();

            switch (preferredPlacement)
            {

                case ZTeachingTipPlacement.Top:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset >= 0;
                    hasHorizontalSpace = (PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0 && PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width);
                    break;
                case ZTeachingTipPlacement.TopLeft:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset >= 0;
                    hasHorizontalSpace = distanceX + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;
                    break;
                case ZTeachingTipPlacement.TopRight:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset >= 0;
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0;
                    break;
                case ZTeachingTipPlacement.Bottom:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;
                    hasHorizontalSpace = (PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0 && PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width);
                    break;

                case ZTeachingTipPlacement.BottomRight:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0;

                    break;
                case ZTeachingTipPlacement.BottomLeft:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;
                    hasHorizontalSpace = distanceX + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;

                    break;
                case ZTeachingTipPlacement.Left:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset >= 0 && PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;
                    break;
                case ZTeachingTipPlacement.LeftTop:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + distanceY + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;

                    break;
                case ZTeachingTipPlacement.LeftBottom:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + distanceY - PopUpCoordinatesInCoreWindowSpace.Height >= 0;

                    break;
                case ZTeachingTipPlacement.Right:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset >= 0 && PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;

                    break;
                case ZTeachingTipPlacement.RightTop:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + distanceY + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;

                    break;
                case ZTeachingTipPlacement.RightBottom:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + distanceY - PopUpCoordinatesInCoreWindowSpace.Height >= 0;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(preferredPlacement), preferredPlacement, null);
            }

            placementOffset.IsFittingWithinBounds = hasHorizontalSpace && hasVerticalSpace;
            Debug.WriteLine($"=============================IS Space Available to display ==================================");
            Debug.WriteLine($"{preferredPlacement} Has VerticalSpace To Display = {hasVerticalSpace} Has Horizontal Space to Display = {hasHorizontalSpace} ");

            double VerticalMarginDeviation()
            {
                return PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom;
            }

            double HorizontalMarginDeviation()
            {
                return PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;
            }
        }



        #region OffsetLOgic Region


        private Rect WindowBounds
        {
            get => Window.Current.Bounds;
        }

        private Rect PopUpCoordinatesInCoreWindowSpace
        {
            get => ZTeachingTipPopUp.TransformToVisual(Window.Current.Content).TransformBounds(new Rect(0.0, 0.0, _popUpWidth, _popUpHeight));
        }

        private Rect TargetCoordinatesInCoreWindowSpace
        {
            get => Target.TransformToVisual(Window.Current.Content).TransformBounds(new Rect(0.0, 0.0, _targetWidth, _targetHeight));
        }

        private Thickness SpaceAroundTarget
        {
            get
            {
                var availableSpace = new Thickness
                {
                    Left = TargetCoordinatesInCoreWindowSpace.X,
                    Top = TargetCoordinatesInCoreWindowSpace.Y,
                    Bottom = WindowBounds.Height - (Target.ActualHeight + TargetCoordinatesInCoreWindowSpace.Y),
                    Right = WindowBounds.Width - (Target.ActualWidth + TargetCoordinatesInCoreWindowSpace.X)
                };
                return availableSpace;
            }
        }


        private double CalculateOffsetForSidePlacement(double distanceX, double distanceY, SidePreference side)
        {
            double offset;
            switch (side)
            {
                case SidePreference.Left:
                    //New Method
                    // offset.HorizontalOffSet = distanceX - PopUpCoordinatesInCoreWindowSpace.Width;
                    offset = distanceX - PopUpCoordinatesInCoreWindowSpace.Width;
                    break;
                case SidePreference.Right:
                    offset = distanceX + TargetCoordinatesInCoreWindowSpace.Width;
                    break;
                case SidePreference.Top:
                    offset = distanceY - PopUpCoordinatesInCoreWindowSpace.Height;
                    break;
                case SidePreference.Bottom:
                    offset = distanceY + TargetCoordinatesInCoreWindowSpace.Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
            return offset;
        }
        private double CalculateOffsetForAlignment(double distance, Alignment alignment, double targetElementDimention, double popUpElementDimentions)
        {
            double offset = default;
            switch (alignment)
            {

                case Alignment.Center:
                    offset = distance - (popUpElementDimentions - targetElementDimention) / 2;
                    break;
                case Alignment.Right:
                    offset = distance - popUpElementDimentions + targetElementDimention;
                    break;
                case Alignment.Left:
                    offset = distance;
                    break;
                case Alignment.Top:
                    offset = distance;
                    break;
                case Alignment.Bottom:
                    offset = distance - popUpElementDimentions + targetElementDimention;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
            }
            return offset;
        }

        #endregion

        #endregion


    }
    public class ActualPlacementChangedEventArgs : EventArgs
    {
        public ZTeachingTipPlacement? ActualPlacement { get; }
        public ActualPlacementChangedEventArgs(ZTeachingTipPlacement? actualPlacement)
        {
            ActualPlacement = actualPlacement;
        }

    }


    public enum ZTeachingTipPlacement
    {
        Top,
        TopLeft,
        TopRight,
        Bottom,
        BottomRight,
        BottomLeft,
        Left,
        LeftTop,
        LeftBottom,
        Right,
        RightTop,
        RightBottom,
    }

}