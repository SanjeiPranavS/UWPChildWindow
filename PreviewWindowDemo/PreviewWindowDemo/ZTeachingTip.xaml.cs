using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZTeachingTip.Zoho.UWP.Common.Extensions;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZTeachingTip
{
    public sealed partial class ZTeachingTip : UserControl
    {

        #region Dependendcy PRoperty

        public readonly static DependencyProperty IsLightDismissEnabledProperty = DependencyProperty.Register(
            nameof(IsLightDismissEnabled), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(true, LightDismissPropertyChanged));


        public readonly static DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(false, TeachingTipClosingChanged));



        public readonly static DependencyProperty ShouldBoundToXamlRootProperty = DependencyProperty.Register(
            nameof(ShouldBoundToXamlRoot), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(default(bool), ShouldBoundToXamlRootChangedCallBack));


        public readonly static DependencyProperty TeachingTipContentProperty = DependencyProperty.Register(
            nameof(TeachingTipContent), typeof(FrameworkElement), typeof(ZTeachingTip), new PropertyMetadata(default, TeachingTipContentChanged));


        public readonly static DependencyProperty TargetProperty = DependencyProperty.Register(
            nameof(Target), typeof(FrameworkElement), typeof(ZTeachingTip), new PropertyMetadata(default(FrameworkElement),(TargetPropertyChangedCallBack)));

        public readonly static DependencyProperty PlacementOffsetMarginProperty = DependencyProperty.Register(
            nameof(PlacementOffsetMargin), typeof(Thickness), typeof(ZTeachingTip), new PropertyMetadata(new Thickness(0), PlacementOffsetMarginPropertyChangedCallBack));


        public readonly static DependencyProperty PreferredPlacementProperty = DependencyProperty.Register(
            nameof(PreferredPlacement), typeof(ZTeachingTipPlacement), typeof(ZTeachingTip), new PropertyMetadata(ZTeachingTipPlacement.LeftTop, (PlacementPReferenceOnPropertyChanged)));


        public ZTeachingTipPlacement PreferredPlacement
        {
            get => (ZTeachingTipPlacement)GetValue(PreferredPlacementProperty);
            set => SetValue(PreferredPlacementProperty, value);
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
            if (d is ZTeachingTip teachingTip  && e.OldValue is FrameworkElement oldTarget)
            {
                oldTarget.SizeChanged += teachingTip.ContentElement_SizeChanged;
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
        private static void TeachingTipContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip && e.NewValue is FrameworkElement newElement)
            {
                if (!newElement.Equals(e.OldValue as FrameworkElement))
                {
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

        #endregion


        public ZTeachingTip()
        {
            this.InitializeComponent();
            ZTeachingTipPopUp.Closed += ZTeachingTipPopUp_Closed;
            PlacementOffsets = PopulateOffsets();
            Loaded += ZTeachingTip_Loaded;
            RootContentPresenter.Translation += new Vector3(0, 0, 70);
            RootContentPresenter.SizeChanged += ContentElement_SizeChanged;
        }

       

        private void ZTeachingTip_Loaded(object sender, RoutedEventArgs e)
        {
            ZTeachingTipPopUp.MaxHeight = RootContentPresenter.ActualHeight;
            ZTeachingTipPopUp.MaxWidth = RootContentPresenter.ActualWidth;
        }

        private void IsOpenPropertyChanged()
        {
            if (IsOpen)
            {
                PositionPopUp();
                SubscribeToSizeChangeNotification();
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
            Window.Current.SizeChanged += WindowSizeChanged;
            CoreWindow.GetForCurrentThread().ResizeCompleted += CoreWindowResizeCompleted;
            CoreWindow.GetForCurrentThread().SizeChanged += CoreWindowResizeCompleted;

        }


        private void ContentElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement contentElement)
            {
                ZTeachingTipPopUp.MaxHeight = RootContentPresenter.ActualHeight;
                ZTeachingTipPopUp.MaxWidth = RootContentPresenter.ActualWidth;
                //IsOpen = false;
                PositionPopUp();
                //IsOpen = true;
            }
        }
        private void ShouldBoundToXamlRootChanged()
        {
            if (!IsOpen)
            {
                ZTeachingTipPopUp.ShouldConstrainToRootBounds = ShouldBoundToXamlRoot;
                return;
            }
            ShouldBoundToXamlRoot = false;
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
        private Dictionary<ZTeachingTipPlacement, ZTeachingTipOffset> PopulateOffsets()
        {
            return new Dictionary<ZTeachingTipPlacement, ZTeachingTipOffset>
            {
                {
                    ZTeachingTipPlacement.Top, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.TopLeft, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.TopRight, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.Right, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.RightTop, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.RightBottom, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.Bottom, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.BottomLeft, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.BottomRight, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.Left, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.LeftTop, new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.LeftBottom, new ZTeachingTipOffset()
                }

            };
        }


        private Dictionary<ZTeachingTipPlacement, ZTeachingTipOffset> PlacementOffsets { get; }


        private void PositionPopUp()
        {
            var isTeachingTipFit = TeachingTipContent is null ? PositionPopUpUnTargeted() : PositionPopUpBasedOnTarget(Target);
        }


        private bool PositionPopUpUnTargeted()
        {
            return true;
        }
        private bool PositionPopUpBasedOnTarget(FrameworkElement targetElement)
        {
            
            var preferredOffset = CalculatePlacementOffsetForPopUp(PreferredPlacement); ;

            ZTeachingTipPopUp.HorizontalOffset = preferredOffset.HorizontalOffSet  + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;
            ZTeachingTipPopUp.VerticalOffset = preferredOffset.VerticalOffSet  + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom;

            PrintOffsetDetails(preferredOffset);
            //ZTeachingTipPopUp.TryShowNear(targetElement, default, PlacementPreferenceOrders.Top, VerticalAlignmentPreferenceOrders.TopCenterBottom,HorizontalAlignmentPreferenceOrders.Center, 0, true);
            return true;

        }
        private void PrintOffsetDetails(ZTeachingTipOffset preferredOffset)
        {
            Debug.WriteLine("================================================================================================================");
            Debug.WriteLine($"Offset Calculation By ZTeaching Tip X = {preferredOffset.HorizontalOffSet} prefered offset Y = {preferredOffset.VerticalOffSet}");
            Debug.WriteLine($"Pop UP Dimentions  Height = {PopUpCoordinatesInCoreWindowSpace.Height} Width ='{PopUpCoordinatesInCoreWindowSpace.Width}");
            Debug.WriteLine($"pop up Cordinates in ZTeachingTip X= {PopUpCoordinatesInCoreWindowSpace.X} Y = {PopUpCoordinatesInCoreWindowSpace.Y}");
            Debug.WriteLine($"Pop Vetical And Horizontal OFfset Vertical={ZTeachingTipPopUp.VerticalOffset} Horizontal = {ZTeachingTipPopUp.HorizontalOffset}");
            Debug.WriteLine($"Target Cordinates X= {TargetCoordinatesInCoreWindowSpace.X}  Y = {TargetCoordinatesInCoreWindowSpace.Y}");
        }


        private ZTeachingTipOffset CalculatePlacementOffsetForPopUp(ZTeachingTipPlacement RequestedPlacement)
        {

            var distanceX =  TargetCoordinatesInCoreWindowSpace.X - PopUpCoordinatesInCoreWindowSpace.X ;
            var distanceY =  TargetCoordinatesInCoreWindowSpace.Y - PopUpCoordinatesInCoreWindowSpace.Y ;
            var placementOffset = new ZTeachingTipOffset();

            switch (RequestedPlacement)
            {

                case ZTeachingTipPlacement.Top:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX,distanceY,SidePreference.Top);
                   placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX,Alignment.Center,TargetCoordinatesInCoreWindowSpace.Width,PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case ZTeachingTipPlacement.TopLeft:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX,distanceY,SidePreference.Top);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Left,TargetCoordinatesInCoreWindowSpace.Width,PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case ZTeachingTipPlacement.TopRight:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Top);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment( distanceX, Alignment.Right,TargetCoordinatesInCoreWindowSpace.Width,PopUpCoordinatesInCoreWindowSpace.Width);
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
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX,distanceY,SidePreference.Left);
                    placementOffset.VerticalOffSet= CalculateOffsetForAlignment(distanceY,Alignment.Top,TargetCoordinatesInCoreWindowSpace.Height,PopUpCoordinatesInCoreWindowSpace.Height);
                     break;
                case ZTeachingTipPlacement.LeftBottom:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Left);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Bottom, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);

                    break;
                case ZTeachingTipPlacement.Right:
                   placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX,distanceY,SidePreference.Right);
                   placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY,Alignment.Center,TargetCoordinatesInCoreWindowSpace.Height,PopUpCoordinatesInCoreWindowSpace.Height);

                    break;
                case ZTeachingTipPlacement.RightTop:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY,SidePreference.Right);
                    placementOffset.VerticalOffSet= CalculateOffsetForAlignment(distanceY,Alignment.Top,TargetCoordinatesInCoreWindowSpace.Height,PopUpCoordinatesInCoreWindowSpace.Height);
                    break;
                case ZTeachingTipPlacement.RightBottom:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, SidePreference.Right);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Bottom, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);
                    break;

            }
            return placementOffset;
        }
    


    #region OffsetLOgic Region


    private Rect WindowBounds
    {
        get => Window.Current.Bounds;
    }
    
    private Rect PopUpCoordinatesInCoreWindowSpace
    {
       get => ZTeachingTipPopUp.TransformToVisual(Window.Current.Content).TransformBounds(new Rect(0.0, 0.0,ZTeachingTipPopUp.MaxWidth,ZTeachingTipPopUp.MaxHeight));
    }

    private Rect TargetCoordinatesInCoreWindowSpace
    {
        get => Target.TransformToVisual(Window.Current.Content).TransformBounds(new Rect(0.0, 0.0,Target.ActualWidth, Target.ActualHeight));
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


    private double CalculateOffsetForSidePlacement(double distanceX,double distanceY,SidePreference side)
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
    private double CalculateOffsetForAlignment( double distance, Alignment alignment,double targetElementDimention,double popUpElementDimentions)
    {
        double offset = default;
        switch (alignment)
        {

            case Alignment.Center:
                offset = distance - (popUpElementDimentions - targetElementDimention) /2;
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
    private double GetOffsetForAlignment(double popupCord, double targetElementCord, double distance, double popupMaxDimension, double targetElementDimension, double windowDimension, Alignment alignmentPreference)
    {
        switch (alignmentPreference)
        {
            case (Alignment.Left):
            case (Alignment.Top):
            {
                // Check if aligning left/top causes overflow. Assumes left/top of targetElement is within window bounds.
                var isOverFlowing = popupCord + distance + popupMaxDimension <= windowDimension;
                return distance;
            }

            case (Alignment.Center):
            {
                // Calculates the offset needed to align the center of the popup with the center of the target element.
                return distance - (popupMaxDimension - targetElementDimension) / 2;
                //var isOVerFlowing = popupCoord + offsetForCenterAlignment >= 0 && popupCoord + offsetForCenterAlignment + popupMaxDimension <= windowDimension)
                // If the calculated offset positions the popup within the window boundaries, align the center of the popup with the center of the target element.
            }
            case (Alignment.Right):
            case (Alignment.Bottom):
            {
                var isOverFlowing = popupCord + distance - popupMaxDimension >= 0;
                return distance - popupMaxDimension + targetElementDimension;

            }
        }
        return default;

    }

        #endregion
#endregion
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

/*
    private readonly double _deviation = 0;
    /// <summary>
    /// ZTeachingTipPLaceMent.Right
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="distanceX"></param>
    /// <param name="distanceY"></param>
    private void AssignOffsetForRightBottomPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    {
        offset.HorizontalOffSet = CalculateHorizontalOffsetForRightBasedPreference();
        offset.VerticalOffSet = CalculateVerticalOffsetForLeftBottomAndRightBottomBasedPreference(distanceY);
    }
    /// <summary>
    /// ZTeachingTipPlacement.Left
    /// </summary>
    private void AssignOffsetForLeftPlacementPreference(ZTeachingTipOffset offset,double distanceX,double distanceY)
    {
        offset.HorizontalOffSet = CalculateHorizontalOffsetForLeftBasedPreferences(distanceX) + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;  
        offset.VerticalOffSet = CalculateVerticalOffsetCenterForRightAndLeftPreference(distanceY) + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom; 
        offset.IsFittingWithinBounds = SpaceAroundTarget.Left >= TeachingTipContent.ActualWidth && (offset.VerticalOffSet + TeachingTipContent.ActualHeight)<= WindowBounds.Height;
    }
    
    /// <summary>
    /// ZTeachingTipPlacement.LeftTop
    /// </summary>
    private void AssignOffsetForLeftTopPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    { 
        offset.HorizontalOffSet = CalculateHorizontalOffsetForLeftBasedPreferences(distanceX) + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;
        offset.VerticalOffSet = CalculateVerticalOffsetTopForRightAndLeftPreference(distanceY) + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom; 
        offset.IsFittingWithinBounds = SpaceAroundTarget.Left >= TeachingTipContent.ActualWidth && (offset.VerticalOffSet + TeachingTipContent.ActualHeight) <= WindowBounds.Height;
    }

    /// <summary>
    /// ZTeachingTipPlacement.LeftBottom
    /// </summary>
    private void AssignOffsetForLeftBottomPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    {
        offset.HorizontalOffSet = CalculateHorizontalOffsetForLeftBasedPreferences(distanceX) + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;  
        offset.VerticalOffSet = CalculateVerticalOffsetForLeftBottomAndRightBottomBasedPreference(distanceY) + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom; 
    }
    
    /// <summary>
    /// ZTeachingTipPlacement.Right
    /// </summary>
    private void AssignOffsetForRightPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    {
        offset.HorizontalOffSet = CalculateHorizontalOffsetForRightBasedPreference() + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;  
        offset.VerticalOffSet = CalculateVerticalOffsetCenterForRightAndLeftPreference(distanceY) + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom; 
        //offset validation can done using spaceAround target property and teaching tip size
        offset.IsFittingWithinBounds = SpaceAroundTarget.Right>=TeachingTipContent.ActualWidth && (offset.VerticalOffSet + TeachingTipContent.ActualHeight) <= WindowBounds.Height;
    }
    
    /// <summary>
    /// ZTeachingTipPlacement.RightTop
    /// </summary>
    private void AssignOffsetForRightTopPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    {
        offset.HorizontalOffSet = CalculateHorizontalOffsetForRightBasedPreference() + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;  
        offset.VerticalOffSet = CalculateVerticalOffsetTopForRightAndLeftPreference(distanceY) + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom; 
        offset.IsFittingWithinBounds = SpaceAroundTarget.Right >= TeachingTipContent.ActualWidth && (offset.VerticalOffSet + TeachingTipContent.ActualHeight) <= WindowBounds.Height;
    }


    /// <summary>
    /// ZTeachingTipPlacement.Top
    /// </summary>
    private void AssignOffsetForTopPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    {
        //Sanjei Anna Logic Also Did't work
        //var distanceY2 = TargetCoordinatesInCoreWindowSpace.Y - PopUpCoordinatesInCoreWindowSpace.Y;
        //offset.VerticalOffSet = distanceY2 - PopUpCoordinatesInCoreWindowSpace.Height;

        offset.VerticalOffSet = CalculateVerticalOffsetForTopPlacementPreferences() + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom;
        offset.HorizontalOffSet = CalculateHorizontalOffsetCenterForTopAndBottomPlacement(distanceX)  -PlacementOffsetMargin.Left + PlacementOffsetMargin.Right;
    }
    
    /// <summary>
    /// ZTeachingTipPlacement.TopLeft
    /// </summary>
    private void AssignOffsetForTopLeftPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    {
        offset.VerticalOffSet = CalculateVerticalOffsetForTopPlacementPreferences() + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom; 
        offset.HorizontalOffSet = CalculateHorizontalOffsetForTopLeftAndBottomLeftPreference() + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;  
    }
    /// <summary>
    /// ZTeachingTipPlacement.TopRight
    /// </summary>
    private void AssignOffsetForTopRightPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    {
        offset.VerticalOffSet = CalculateVerticalOffsetForTopPlacementPreferences() + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom;
        offset.HorizontalOffSet = CalculateHorizontalOffsetForTopRightAndBottomRightPlacementPreference() + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;  
    }

    /// <summary>
    /// ZTeachingTipPlacement.Bottom
    /// </summary>
    private void AssignOffsetForBottomPlacementReference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    {
        offset.HorizontalOffSet = CalculateHorizontalOffsetCenterForTopAndBottomPlacement(distanceX) + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;  
        offset.VerticalOffSet = CalCulateVerticalOffsetForBottomPlacementPreference() + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom; 
    }
   
    /// <summary>
    /// ZTeachingTipPlacement.BottomLeft
    /// </summary>
    private void AssignOffsetForBottomLeftPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    {
        offset.VerticalOffSet = CalCulateVerticalOffsetForBottomPlacementPreference() + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom; 
        offset.HorizontalOffSet = CalculateHorizontalOffsetForTopLeftAndBottomLeftPreference() + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;  
    }

    /// <summary>
    /// ZTeachingTipPlacement.BottomRight
    /// </summary>
    private void AssignOffsetForBottomRightPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
    {
        offset.HorizontalOffSet = CalculateHorizontalOffsetForTopRightAndBottomRightPlacementPreference() + PlacementOffsetMargin.Left - PlacementOffsetMargin.Right;  
        offset.VerticalOffSet = CalCulateVerticalOffsetForBottomPlacementPreference() + PlacementOffsetMargin.Top - PlacementOffsetMargin.Bottom;
    }
    private double CalculateVerticalOffsetForLeftBottomAndRightBottomBasedPreference(double distanceY)
    {
        double verticalOffset = default;
        var sizeDifference = Math.Abs(TeachingTipContent.ActualHeight - Target.ActualHeight);
        if (Target.ActualHeight < TeachingTipContent.ActualHeight)
        {
           verticalOffset=  TargetCoordinatesInCoreWindowSpace.Y - sizeDifference;
        }
        if (Target.ActualHeight > TeachingTipContent.ActualHeight)
        {
            verticalOffset= TargetCoordinatesInCoreWindowSpace.Y + sizeDifference;
        }
        if (Target.ActualHeight.Equals(TeachingTipContent.ActualHeight))
        {
            verticalOffset = TargetCoordinatesInCoreWindowSpace.Y;
        }
        return verticalOffset - _deviation;
    }

    private double CalculateHorizontalOffsetForTopLeftAndBottomLeftPreference()
    {
        return TargetCoordinatesInCoreWindowSpace.X;
    }

    private double CalculateHorizontalOffsetForRightBasedPreference()
    {
        return TargetCoordinatesInCoreWindowSpace.X + TargetCoordinatesInCoreWindowSpace.Width;
    }

    private double CalculateHorizontalOffsetForLeftBasedPreferences(double distanceX)
    {
       return PopUpCoordinatesInCoreWindowSpace.X - distanceX - PopUpCoordinatesInCoreWindowSpace.Width;
    }

    private double CalculateVerticalOffsetCenterForRightAndLeftPreference(double distanceY)
    {
        var sizeDifference = Math.Abs(TargetCoordinatesInCoreWindowSpace.Height - PopUpCoordinatesInCoreWindowSpace.Height) / 2;
        double verticalOffset = default;

        if (TargetCoordinatesInCoreWindowSpace.Height.Equals(PopUpCoordinatesInCoreWindowSpace.Height))
        {
            verticalOffset = TargetCoordinatesInCoreWindowSpace.Y;
        }
        if (TargetCoordinatesInCoreWindowSpace.Height < PopUpCoordinatesInCoreWindowSpace.Height)
        {
            verticalOffset = TargetCoordinatesInCoreWindowSpace.Y - sizeDifference;
        }
        if (TargetCoordinatesInCoreWindowSpace.Height > PopUpCoordinatesInCoreWindowSpace.Height)
        {
            verticalOffset = TargetCoordinatesInCoreWindowSpace.Y + sizeDifference;
        }

        return verticalOffset - _deviation;
    }

    private double CalculateHorizontalOffsetCenterForTopAndBottomPlacement(double distanceX)
    {
        var sizeDifference =Math.Abs( TargetCoordinatesInCoreWindowSpace.Width - TeachingTipContent.ActualWidth)/2;

        if (TargetCoordinatesInCoreWindowSpace.Width < PopUpCoordinatesInCoreWindowSpace.Width)
        {
            return TargetCoordinatesInCoreWindowSpace.X - sizeDifference;
        }
        if (TargetCoordinatesInCoreWindowSpace.Width > PopUpCoordinatesInCoreWindowSpace.Width)
        {
            return TargetCoordinatesInCoreWindowSpace.X + sizeDifference;
        }
        return TargetCoordinatesInCoreWindowSpace.X;

    }

    private double CalculateVerticalOffsetTopForRightAndLeftPreference(double distanceY)
    {
        return TargetCoordinatesInCoreWindowSpace.Y - _deviation;
    }
    private double CalculateVerticalOffsetForTopPlacementPreferences()
    {
        return TargetCoordinatesInCoreWindowSpace.Y - TeachingTipContent.ActualHeight - _deviation;
    }

    private double CalculateHorizontalOffsetForTopRightAndBottomRightPlacementPreference()
    {

        return (TargetCoordinatesInCoreWindowSpace.X + TargetCoordinatesInCoreWindowSpace.Width) - PopUpCoordinatesInCoreWindowSpace.Width;
    }

    private double CalCulateVerticalOffsetForBottomPlacementPreference()
    {
        return TargetCoordinatesInCoreWindowSpace.Y + Target.ActualHeight - _deviation;
    }
    #endregion
        //ZTeachingTipOffset TransFormOffsetWithRespectToParent(ZTeachingTipOffset offset)
        //{
        //    if (!(Parent is UIElement parent))
        //    {
        //        return offset;
        //    }
        //    var transFormedPoints =  parent.TransformToVisual(parent).TransformPoint(new Point(offset.HorizontalOffSet, offset.VerticalOffSet));
        //    offset.HorizontalOffSet = transFormedPoints.X;
        //    offset.VerticalOffSet = transFormedPoints.Y;
        //    return offset;
        //}

*/