using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PreviewWindowDemo.Zoho.UWP.Common.Extensions;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PreviewWindowDemo
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
                PositionPopUp();
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

        }

        private void SubscribeToSizeChangeNotification()
        {
            Window.Current.SizeChanged += WindowSizeChanged;
            CoreWindow.GetForCurrentThread().ResizeCompleted += CoreWindowResizeCompleted;
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
            
            ZTeachingTipPopUp.HorizontalOffset = preferredOffset.HorizontalOffSet; 
            ZTeachingTipPopUp.VerticalOffset = preferredOffset.VerticalOffSet;

            //Debug.WriteLine("=========Target=========");
            //Debug.WriteLine($"target Actual Height = {targetElement.ActualHeight}");
            //Debug.WriteLine($"target Actual Width = {targetElement.ActualWidth}");
            //Debug.WriteLine("=========Content=========");
            //Debug.WriteLine($"Content Actual Height = {TeachingTipContent?.ActualHeight}");
            //Debug.WriteLine($"Content Actual Width = {TeachingTipContent?.ActualWidth}");
            //Debug.WriteLine("=========Content Presenter=========");
            //Debug.WriteLine($"Content presenter Actual Height = {RootContentPresenter?.ActualHeight}");
            //Debug.WriteLine($"Content Presenter Actual Width = {RootContentPresenter?.ActualWidth}");
            //Debug.WriteLine("=========Popup=========");
            //Debug.WriteLine($"PopUP Actual Height = {ZTeachingTipPopUp.ActualHeight}");
            //Debug.WriteLine($"POpUP Actual Width = {ZTeachingTipPopUp.ActualWidth}");
            //Debug.WriteLine($"{PreferredPlacement.ToString()} Has Space to Display : {prefferedOffset.IsFittingWithinBounds}");
            return true;

        }

        
        public void DeterminePossiblePlacementsWithTarget()
        {

            var distanceX = PopUpCoordinates.X - TargetCoordinates.X;
            var distanceY = PopUpCoordinates.Y - TargetCoordinates.Y;


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
                        break;
                    case ZTeachingTipPlacement.Bottom:
                        AssignOffsetForBottomPlacementReference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.BottomRight:
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
                        break;
                    case ZTeachingTipPlacement.Right:
                        AssignOffsetForRightPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.RightTop:
                        AssignOffsetForRightTopPlacementPreference(placement.Value,distanceX,distanceY);
                        break;
                    case ZTeachingTipPlacement.RightBottom:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        #region OffsetLOgic Region
        //for Left and Right based preference Vertical Offset Calculation will be same

        //todo
        //Find The Root Cause Which Causes Deviation in Vertical Orientation
        private readonly double _deviation = 18.5;  


        /// <summary>
        /// ZTeachingTipPlacement.Left
        /// </summary>
        private void AssignOffsetForLeftPlacementPreference(ZTeachingTipOffset offset,double distanceX,double distanceY)
        {
            offset.HorizontalOffSet = CalculateHorizontalOffsetForLeftBasedPreferences(distanceX);
            offset.VerticalOffSet = CalculateVerticalOffsetCenterForRightAndLeftPreference(distanceY);
            offset.IsFittingWithinBounds = SpaceAroundTarget.Left >= TeachingTipContent.ActualWidth && (offset.VerticalOffSet + TeachingTipContent.ActualHeight)<= WindowBounds.Height;
        }
        
        /// <summary>
        /// ZTeachingTipPlacement.LeftTop
        /// </summary>
        private void AssignOffsetForLeftTopPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
        {
            offset.HorizontalOffSet = CalculateHorizontalOffsetForLeftBasedPreferences(distanceX);
            offset.VerticalOffSet = CalculateVerticalOffsetTopForRightAndLeftPreference(distanceY);
            offset.IsFittingWithinBounds = SpaceAroundTarget.Left >= TeachingTipContent.ActualWidth && (offset.VerticalOffSet + TeachingTipContent.ActualHeight) <= WindowBounds.Height;
        }


        /// <summary>
        /// ZTeachingTipPlacement.Right
        /// </summary>
        private void AssignOffsetForRightPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
        {
            offset.HorizontalOffSet = CalculateHorizontalOffsetForRightBasedPreference();
            offset.VerticalOffSet = CalculateVerticalOffsetCenterForRightAndLeftPreference(distanceY);
            //offset validation can done using spaceAround target property and teaching tip size
            offset.IsFittingWithinBounds = SpaceAroundTarget.Right>=TeachingTipContent.ActualWidth && (offset.VerticalOffSet + TeachingTipContent.ActualHeight) <= WindowBounds.Height;
        }
        
        /// <summary>
        /// ZTeachingTipPlacement.RightTop
        /// </summary>
        private void AssignOffsetForRightTopPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
        {
            offset.HorizontalOffSet = CalculateHorizontalOffsetForRightBasedPreference();
            offset.VerticalOffSet = CalculateVerticalOffsetTopForRightAndLeftPreference(distanceY);
            offset.IsFittingWithinBounds = SpaceAroundTarget.Right >= TeachingTipContent.ActualWidth && (offset.VerticalOffSet + TeachingTipContent.ActualHeight) <= WindowBounds.Height;
        }


        /// <summary>
        /// ZTeachingTipPlacement.Top
        /// </summary>
        private void AssignOffsetForTopPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
        {
            offset.VerticalOffSet = CalculateVerticalOffsetForTopPlacementPreferences();
            offset.HorizontalOffSet = CalculateHorizontalOffsetCenterForTopAndBottomPlacement(distanceX);
        }
        
        /// <summary>
        /// ZTeachingTipPlacement.TopLeft
        /// </summary>
        private void AssignOffsetForTopLeftPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
        {
            offset.VerticalOffSet = CalculateVerticalOffsetForTopPlacementPreferences();
            offset.HorizontalOffSet = CalculateHorizontalOffsetForTopLeftAndBottomLeftPreference();
        }
       
        /// <summary>
        /// ZTeachingTipPlacement.Bottom
        /// </summary>
        private void AssignOffsetForBottomPlacementReference(ZTeachingTipOffset offset, double distanceX, double distanceY)
        {
            offset.HorizontalOffSet = CalculateHorizontalOffsetCenterForTopAndBottomPlacement(distanceX);
            offset.VerticalOffSet = CalCulateVerticalOffsetForBottomPlacementPreference();
        }
       
        /// <summary>
        /// ZTeachingTipPlacement.BottomLeft
        /// </summary>
        private void AssignOffsetForBottomLeftPlacementPreference(ZTeachingTipOffset offset, double distanceX, double distanceY)
        {
            offset.VerticalOffSet = CalCulateVerticalOffsetForBottomPlacementPreference();
            offset.HorizontalOffSet = CalculateHorizontalOffsetForTopLeftAndBottomLeftPreference();
        }
        private double CalculateHorizontalOffsetForTopLeftAndBottomLeftPreference()
        {
            return TargetCoordinates.X;
        }

        private double CalculateHorizontalOffsetForRightBasedPreference()
        {
            return TargetCoordinates.X + Target.ActualWidth;
        }

        private double CalculateHorizontalOffsetForLeftBasedPreferences(double distanceX)
        {
            return PopUpCoordinates.X - distanceX - TeachingTipContent.ActualWidth;
        }

        private double CalculateVerticalOffsetCenterForRightAndLeftPreference(double distanceY)
        {
            var sizeDifference = Math.Abs(Target.ActualHeight - TeachingTipContent.ActualHeight) / 2;
            double verticalOffset = default;

            if (Target.ActualHeight.Equals( TeachingTipContent.ActualHeight))
            {
                verticalOffset = TargetCoordinates.Y;
            }
            if (Target.ActualHeight < TeachingTipContent.ActualHeight)
            {
                verticalOffset = TargetCoordinates.Y - sizeDifference;
            }
            if (Target.ActualHeight > TeachingTipContent.ActualHeight)
            {
                verticalOffset = TargetCoordinates.Y + sizeDifference;
            }

            return verticalOffset - _deviation;
        }

        private double CalculateHorizontalOffsetCenterForTopAndBottomPlacement(double distanceX)
        {
            var sizeDifference =Math.Abs( Target.ActualWidth - TeachingTipContent.ActualWidth)/2;

            if (Target.ActualWidth < TeachingTipContent.ActualWidth)
            {
                return TargetCoordinates.X - sizeDifference;
            }
            if (Target.ActualWidth > TeachingTipContent.ActualWidth)
            {
                return TargetCoordinates.X + sizeDifference;
            }
            return TargetCoordinates.X;

        }

        private double CalculateVerticalOffsetTopForRightAndLeftPreference(double distanceY)
        {
            return TargetCoordinates.Y - _deviation;
        }
        private double CalculateVerticalOffsetForTopPlacementPreferences()
        {
            return TargetCoordinates.Y - TeachingTipContent.ActualHeight - _deviation;
        }

        private double CalCulateVerticalOffsetForBottomPlacementPreference()
        {
            return TargetCoordinates.Y + Target.ActualHeight - _deviation;
        }
        #endregion

        public Point TargetCoordinates
        {  
            get => Target.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));
        }
        public Point PopUpCoordinates
        {
            get => TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));
        }

        public Rect WindowBounds
        {
            get => Window.Current.Bounds;
        }
        public Thickness SpaceAroundTarget
        {
            get
            {
                var availableSpace = new Thickness
                {
                    Left = TargetCoordinates.X,
                    Top = TargetCoordinates.Y,
                    Bottom = WindowBounds.Height - (Target.ActualHeight + TargetCoordinates.Y),
                    Right = WindowBounds.Width - (Target.ActualWidth + TargetCoordinates.X)
                };
                return availableSpace;
            }
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
