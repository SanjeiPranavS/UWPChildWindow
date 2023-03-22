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
            nameof(IsLightDismissEnabled), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(true,LightDismissPropertyChanged));

        
        public readonly static DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(false, TeachingTipClosingChanged));



        public readonly static DependencyProperty ShouldBoundToXamlRootProperty = DependencyProperty.Register(
            nameof(ShouldBoundToXamlRoot), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(default(bool), ShouldBoundToXamlRootChangedCallBack));


        public readonly static DependencyProperty TeachingTipContentProperty = DependencyProperty.Register(
            nameof(TeachingTipContent), typeof(FrameworkElement), typeof(ZTeachingTip), new PropertyMetadata(default,TeachingTipContentChanged));


        public readonly static DependencyProperty TargetProperty = DependencyProperty.Register(
            nameof(Target), typeof(FrameworkElement), typeof(ZTeachingTip), new PropertyMetadata(default(FrameworkElement)));


        public readonly static DependencyProperty PlacementOffsetMarginProperty = DependencyProperty.Register(
            nameof(PlacementOffsetMargin), typeof(Thickness), typeof(ZTeachingTip), new PropertyMetadata(new Thickness(0),PlacementOffsetMarginPropertyChangedCallBack));


        public readonly static DependencyProperty PreferredPlacementProperty = DependencyProperty.Register(
            nameof(PreferredPlacement), typeof(ZTeachingTipPlacement), typeof(ZTeachingTip), new PropertyMetadata(ZTeachingTipPlacement.Left,(PlacementPReferenceOnPropertyChanged)));

      
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
                    teachingTip.PositionPopUp();
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
        

        private void ContentElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement contentElement)
            {
                ZTeachingTipPopUp.MaxWidth=contentElement.ActualWidth;
                ZTeachingTipPopUp.MaxHeight=contentElement.ActualHeight;
                ZTeachingTipPopUp.TryShowNear(Target, default, PlacementPreferenceOrders.Right, VerticalAlignmentPreferenceOrders.TopCenterBottom,HorizontalAlignmentPreferenceOrders.Center, 0,true);
                //PositionPopUp();
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
            RootContentPresenter.Translation += new Vector3(0, 0, 70);
        }
        private Dictionary<ZTeachingTipPlacement,ZTeachingTipOffset> PopulateOffsets()
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
                    ZTeachingTipPlacement.TopRight,new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.Right,new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.RightTop,new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.RightBottom,new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.Bottom,new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.BottomLeft,new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.BottomRight,new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.Left,new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.LeftTop,new ZTeachingTipOffset()
                },
                {
                    ZTeachingTipPlacement.LeftBottom,new ZTeachingTipOffset()
                }

            };
        }


        private void IsOpenPropertyChanged()
        {
            if (IsOpen)
            {
                ZTeachingTipPopUp.IsOpen = IsOpen;
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
            DeterminePossiblePlacementsWithTarget();
            var preferredOffset = PlacementOffsets[PreferredPlacement];

           // var transFormedOffsetWithRespectToParent = TransFormOffsetWithRespectToParent(preferredOffset);

           
            ZTeachingTipPopUp.HorizontalOffset = preferredOffset.HorizontalOffSet; 
            ZTeachingTipPopUp.VerticalOffset = preferredOffset.VerticalOffSet;

            PrintOffsetDetails(preferredOffset, preferredOffset);
            return true;

        }
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
        private void PrintOffsetDetails(ZTeachingTipOffset preferredOffset, ZTeachingTipOffset transFormedOffsetWithRespectToParent)
        {
            Debug.WriteLine("================================================================================================================");
            Debug.WriteLine($"Prefered Offset X = {preferredOffset.HorizontalOffSet} prefered offset Y = {preferredOffset.VerticalOffSet}");
            Debug.WriteLine($"transFormedOffset with respect to parent X = {transFormedOffsetWithRespectToParent.HorizontalOffSet} Y = {transFormedOffsetWithRespectToParent.VerticalOffSet}");
            Debug.WriteLine($"PopUPDimentions  Height = {PopUpCoordinatesInCoreWindowSpace.Height} Width ='{PopUpCoordinatesInCoreWindowSpace.Width}");
            Debug.WriteLine($"popupCordinates X= {PopUpCoordinatesInCoreWindowSpace.X} Y = {PopUpCoordinatesInCoreWindowSpace.Y}");
            Debug.WriteLine($"PopVetical And Horizontal OFfset Vertical={ZTeachingTipPopUp.VerticalOffset} Horizontal = {ZTeachingTipPopUp.HorizontalOffset}");
            Debug.WriteLine($"Target Cordinates X= {TargetCoordinatesInCoreWindowSpace.X}  Y = {TargetCoordinatesInCoreWindowSpace.Y}" );
        }


        public void DeterminePossiblePlacementsWithTarget()
        {

            var distanceX = PopUpCoordinatesInCoreWindowSpace.X - TargetCoordinatesInCoreWindowSpace.X;
            var distanceY = PopUpCoordinatesInCoreWindowSpace.Y - TargetCoordinatesInCoreWindowSpace.Y;


            foreach (var placement in PlacementOffsets)
            {
                
                switch (placement.Key)
                {

                    case ZTeachingTipPlacement.Top:
                        AssignOffsetForTopPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.TopLeft:
                        AssignOffsetForTopLeftPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.TopRight:
                        AssignOffsetForTopRightPlacementPreference(placement.Value, distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.Bottom:
                        AssignOffsetForBottomPlacementReference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.BottomRight:
                        AssignOffsetForBottomRightPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.BottomLeft:
                        AssignOffsetForBottomLeftPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.Left:
                        AssignOffsetForLeftPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.LeftTop:
                        AssignOffsetForLeftTopPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.LeftBottom:
                        AssignOffsetForLeftBottomPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.Right:
                        AssignOffsetForRightPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.RightTop:
                        AssignOffsetForRightTopPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.RightBottom:
                        AssignOffsetForRightBottomPlacementPreference(placement.Value, distanceX,distanceY);
                        break;
                    default:
                        continue;
                }
            }
        }
        private void AssignOffsetForRightBottomPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
        {
            offset.HorizontalOffSet = CalculateHorizontalOffsetForRightBasedPreference();
            offset.VerticalOffSet = CalculateVerticalOffsetForLeftBottomAndRightBottomBasedPreference(distanceY);
        }

        #region OffsetLOgic Region
        //for Left and Right based preference Vertical Offset Calculation will be same

        //todo
        //Find The Root Cause Which Causes Deviation in Vertical Orientation
        private readonly double _deviation = 0;  


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

        public Rect WindowBounds
        {

            get
            {
                if (XamlRoot is XamlRoot xamlRoot)
                {
                    return new Rect(0, 0, xamlRoot.Size.Width, xamlRoot.Size.Width);
                }
                return Window.Current.Bounds;
            }
        }

        public Rect PopUpCoordinatesInCoreWindowSpace
        {
            get => TransformToVisual(Window.Current.Content).TransformBounds(new Rect(0.0, 0.0, TeachingTipContent.ActualWidth, TeachingTipContent.ActualHeight));
        }

        public Rect TargetCoordinatesInCoreWindowSpace
        {
            get => Target.TransformToVisual(Window.Current.Content).TransformBounds(new Rect(0.0, 0.0, Target.ActualWidth, Target.ActualHeight));
        }
        public Thickness SpaceAroundTarget
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
        private static double GetOffsetForAlignment(double popupCoord, double targetElementCoord, double distance, double popupMaxDimension, double targetElementDimension, double windowDimension, Alignment alignmentPreference)
        {
               switch (alignmentPreference)
                {
                    case (Alignment.Left):
                    case (Alignment.Top):
                    {
                        // Check if aligning left/top causes overflow. Assumes left/top of targetElement is within window bounds.
                        var isOverFlowing = popupCoord + distance + popupMaxDimension <= windowDimension;
                        return distance;
                    }

                    case (Alignment.Center):
                    {
                        // Calculates the offset needed to align the center of the popup with the center of the target element.
                        return  distance - (popupMaxDimension - targetElementDimension) / 2;
                        //var isOVerFlowing = popupCoord + offsetForCenterAlignment >= 0 && popupCoord + offsetForCenterAlignment + popupMaxDimension <= windowDimension)
                        // If the calculated offset positions the popup within the window boundaries, align the center of the popup with the center of the target element.
                    }
                    case (Alignment.Right):
                    case (Alignment.Bottom):
                    {
                        var isOverFlowing = popupCoord + distance - popupMaxDimension >= 0;
                        return distance - popupMaxDimension + targetElementDimension;
                        
                    }
                }
                return default;

        }
        #endregion

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
