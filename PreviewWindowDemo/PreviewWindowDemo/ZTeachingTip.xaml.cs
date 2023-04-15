using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Zoho.UWP.Common.Extensions;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZTeachingTip
{
    [ContentProperty(Name = nameof(ZTeachingTipContent))]
    public sealed partial class ZTeachingTip : UserControl
    {

        #region Dependendcy PRoperty And Dp CallBAcks

        /// <summary>Enables light-dismiss functionality so that a teaching tip will dismiss when a user scrolls or interacts with other elements of the application.</summary>
        public bool IsLightDismissEnabled
        {
            get => (bool)GetValue(IsLightDismissEnabledProperty);
            set => SetValue(IsLightDismissEnabledProperty, value);
        }


        public static readonly DependencyProperty IsLightDismissEnabledProperty = DependencyProperty.Register(
            nameof(IsLightDismissEnabled), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(true, LightDismissPropertyChanged));

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

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(false, TeachingTipClosingChanged));

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

        /// <summary>Gets or sets a value that indicates whether the teaching tip will constrain to the bounds of its xaml root.</summary>
        public bool ShouldBoundToXamlRoot
        {
            get => (bool)GetValue(ShouldBoundToXamlRootProperty);
            set => SetValue(ShouldBoundToXamlRootProperty, value);
        }

        public static readonly DependencyProperty ShouldBoundToXamlRootProperty = DependencyProperty.Register(
            nameof(ShouldBoundToXamlRoot), typeof(bool), typeof(ZTeachingTip), new PropertyMetadata(true, ShouldBoundToXamlRootChangedCallBack));

        private static void ShouldBoundToXamlRootChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip && e.OldValue is bool oldValue && e.NewValue is bool newValue)
            {
                if (oldValue != newValue)
                {
                    teachingTip.ShouldBoundToXamlRootChanged();
                }
            }
        }


        /// <summary>
        /// Default MaxHeight ,MaxWidth of Content Presenter is Set to 500 each, to override Default MaxWidth, MaxHeight set MaxWidth, MaxHeight in ZTeachingTip's MaxWidth
        /// MaxHeight 
        /// </summary>
        public FrameworkElement ZTeachingTipContent
        {
            get => (FrameworkElement)GetValue(ZTeachingTipContentProperty);
            set => SetValue(ZTeachingTipContentProperty, value);
        }

        public static readonly DependencyProperty ZTeachingTipContentProperty = DependencyProperty.Register(
            nameof(ZTeachingTipContent), typeof(FrameworkElement), typeof(ZTeachingTip), new PropertyMetadata(default, TeachingTipContentChanged));

        private static void TeachingTipContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip && e.NewValue is FrameworkElement newElement)
            {
                if (!newElement.Equals(e.OldValue as FrameworkElement))
                {
                    teachingTip.ContentChanged(newElement);
                    //newElement.SizeChanged += teachingTip.ContentElement_SizeChanged;
                }
                if (e.OldValue is FrameworkElement oldContentElement)
                {
                    oldContentElement.SizeChanged -= teachingTip.ContentElement_SizeChanged;
                }
            }
        }

        /// <summary>Sets the target for a teaching tip to position itself relative to and point at with its tail.</summary>
        public FrameworkElement Target
        {
            get => (FrameworkElement)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            nameof(Target), typeof(FrameworkElement), typeof(ZTeachingTip), new PropertyMetadata(default(FrameworkElement), (TargetPropertyChangedCallBack)));


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

        /// <summary>Adds a margin between a targeted teaching tip and its target </summary>
        public Thickness PlacementMargin
        {
            get => (Thickness)GetValue(PlacementMarginProperty);
            set => SetValue(PlacementMarginProperty, value);
        }

        public static readonly DependencyProperty PlacementMarginProperty = DependencyProperty.Register(
            nameof(PlacementMargin), typeof(Thickness), typeof(ZTeachingTip), new PropertyMetadata(default(Thickness), PlacementMarginChangedCallBack));

        private static void PlacementMarginChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip)
            {
                teachingTip.OnPlacementMarginOffsetChanged();
            }
        }


        /// <summary>Preferred placement to be used for the teaching tip. If there is not enough space to show at the preferred placement, a new placement will be automatically chosen.
        /// Placement is relative to its target if Target is non-null or to the <see cref="Window.Current.Content"/>  if Target is null.
        /// Default Placement Preference : LeftTop
        /// if <see cref="ShouldBoundToXamlRoot"/> is set to true ,Preferred Placement Positions 
        /// In Clockwise Direction i.e Left,LeftTop,TopRight,Top,TopLeft,RightTop,Right,RightBottom,BottomRight,Bottom,BottomLeft,LeftBottom  if Size constrain doesn't meet
        /// if <see cref="ShouldBoundToXamlRoot"/> is set to false Preferred Placement positions Regardless of Size Constraints  
        /// </summary>
        public ZTeachingTipPlacement PreferredPlacement
        {
            get => (ZTeachingTipPlacement)GetValue(PreferredPlacementProperty);
            set => SetValue(PreferredPlacementProperty, value);
        }

        public static readonly DependencyProperty PreferredPlacementProperty = DependencyProperty.Register(
            nameof(PreferredPlacement), typeof(ZTeachingTipPlacement), typeof(ZTeachingTip), new PropertyMetadata(ZTeachingTipPlacement.LeftTop, (PlacementPReferenceOnPropertyChanged)));


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

        private static readonly DependencyProperty TailPolygonMarginProperty = DependencyProperty.Register(
            nameof(TailPolygonMargin), typeof(Thickness), typeof(ZTeachingTip), new PropertyMetadata(default(Thickness)));

        /// <summary>
        /// By default the margin of tail polygon is set to certain value based on the placement.
        /// If extra margin is to be added or removed, add or subtract the required margin to the defined default margin
        /// </summary>
        public Thickness TailPolygonMargin
        {
            get => (Thickness)GetValue(TailPolygonMarginProperty);
            set => SetValue(TailPolygonMarginProperty, value);
        }

        /// <summary>Gets or sets the Style to apply to the teaching tip's close button.</summary>
        public Style CloseButtonStyle
        {
            get => (Style)GetValue(CloseButtonStyleProperty);
            set => SetValue(CloseButtonStyleProperty, value);
        }

        public static readonly DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register(
            nameof(CloseButtonStyle), typeof(Style), typeof(ZTeachingTip), new PropertyMetadata(default, (CloseButtonStyleChangedCallBack)));

        private static void CloseButtonStyleChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip teachingTip && e.NewValue is Style closeButtonStyle)
            {
                teachingTip.TeachingTipCloseBtn.Style = closeButtonStyle;
            }
        }


        public LightDismissOverlayMode LightDismissMode
        {
            get => (LightDismissOverlayMode)GetValue(LightDismissModeProperty);
            set => SetValue(LightDismissModeProperty, value);
        }

        public static readonly DependencyProperty LightDismissModeProperty = DependencyProperty.Register(
            nameof(LightDismissMode), typeof(LightDismissOverlayMode), typeof(ZTeachingTip), new PropertyMetadata(LightDismissOverlayMode.Auto, OnLightDismissModePropertyChanged));


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

        /// <summary>
        /// Default  ApplicationBackGroundThemeBrush,
        /// </summary>
        public Brush TailBackGround
        {
            get => (Brush)GetValue(TailBackGroundProperty);
            set => SetValue(TailBackGroundProperty, value);
        }

        public static readonly DependencyProperty TailBackGroundProperty = DependencyProperty.Register(
            nameof(TailBackGround), typeof(Brush), typeof(ZTeachingTip), new PropertyMetadata(default(Brush), (TailBackGroundChangedCallback)));

        private static void TailBackGroundChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip tip && e.NewValue is Brush newBrush)
            {
                tip.TailPolygon.Fill = newBrush;
                tip.TailPolygon.Stroke = newBrush;
            }
        }


        /// <summary>Toggles collapse of a teaching tip's tail. Can be used to override auto behavior to make a tail visible on a non-targeted teaching tip and hidden on a targeted teaching tip.</summary>
        public Visibility TailVisibility
        {
            get => (Visibility)GetValue(TailVisibilityProperty);
            set => SetValue(TailVisibilityProperty, value);
        }

        public static readonly DependencyProperty TailVisibilityProperty = DependencyProperty.Register(
            nameof(TailVisibility), typeof(Visibility), typeof(ZTeachingTip), new PropertyMetadata(Visibility.Visible));


        public double ContentHeight
        {
            get => (double)GetValue(ContentHeightProperty);
            set => SetValue(ContentHeightProperty, value);
        }

        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register(
            nameof(ContentHeight), typeof(double), typeof(ZTeachingTip), new PropertyMetadata(default(double), (InnerContentHeightChangedCallBack)));

        private static void InnerContentHeightChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip tip && e.NewValue is double height)
            {
                tip.RootContentPresenter.Height = height;
            }
        }

        public double ContentWidth
        {
            get => (double)GetValue(ContentWidthProperty);
            set => SetValue(ContentWidthProperty, value);
        }

        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register(
            nameof(ContentWidth), typeof(double), typeof(ZTeachingTip), new PropertyMetadata(default(double), (InnerContentWightChangedCallBack)));

        private static void InnerContentWightChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip tip && e.NewValue is double width)
            {
                tip.RootContentPresenter.Width = width;
            }
        }

        /// <summary>Gets or sets the content of the teaching tip's close button.</summary>
        public object CloseButtonContent
        {
            get => (object)GetValue(CloseButtonContentProperty);
            set => SetValue(CloseButtonContentProperty, value);
        }
        public static readonly DependencyProperty CloseButtonContentProperty = DependencyProperty.Register(
            nameof(CloseButtonContent), typeof(object), typeof(ZTeachingTip), new PropertyMetadata(default, (CloseButtonContentChangedCallBack)));

        private static void CloseButtonContentChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip tip && e.NewValue != null)
            {
                tip.TeachingTipCloseBtn.Content = e.NewValue;
            }
        }

        /// <summary>
        /// Indicates  Actual Current Placement Of the TeachingTip Which PLaced Respect to Available Size, 
        /// If ActualPlacement is null Then there is no space for Positioning of Teaching Tip
        /// </summary>
        public ZTeachingTipPlacement? ActualPlacement
        {
            get => _actualPlacement;
            private set
            {
                if (_actualPlacement == value)
                {
                    return;
                }
                _actualPlacement = value;
                ActualPlacementChanged?.Invoke(this, new ActualPlacementChangedEventArgs(_actualPlacement));
            }
        }

        private ZTeachingTipPlacement? _actualPlacement;

        /// <summary>
        /// Invoked When ZTeachingTip is Position Changed with respect  To Available Size 
        /// </summary>
        public event Action<ZTeachingTip, ActualPlacementChangedEventArgs> ActualPlacementChanged;

        public event Action<object, RoutedEventArgs> CloseButtonClicked;

        public event Action<ZTeachingTip, ZTeachingTipOpenedEventArgs> Opened;

        public event Action<ZTeachingTip, ZTeachingTipClosedEventArgs> Closed;


        private double _targetWidth;

        private double _targetHeight;

        private double _popUpWidth;

        private double _popUpHeight;

        private Rect _popupRect;

        private Rect _targetRect;

        private Thickness _spaceAroundTarget;

        #endregion

        #region EventsAndRegistedDpCallbacks

        private void MaxHeightPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            RootContentPresenter.MaxHeight = (double)GetValue(dp);
        }

        private void MaxWidthPropertyChangedCallBack(DependencyObject sender, DependencyProperty dp)
        {
            RootContentPresenter.MaxWidth = (double)GetValue(dp);
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            PositionPopUp();
        }
        private void CoreWindowResizeCompleted(CoreWindow sender, object args)
        {
            PositionPopUp();
        }
        private void TailPositioningStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Equals(e.OldState))
            {
                return;
            }
            ContentElement_SizeChanged(default, default);
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            CloseButtonClicked?.Invoke(this, e);
            IsOpen = false;
        }

        private void ZTeachingTipPopUp_Closed(object sender, object e)
        {
            IsOpen = false;
            Closed?.Invoke(this, new ZTeachingTipClosedEventArgs());
        }
        private void ZTeachingTipPopUp_Opened(object sender, object e)
        {
            Opened?.Invoke(this, new ZTeachingTipOpenedEventArgs());
        }

        #endregion

        public ZTeachingTip()
        {

            this.InitializeComponent();
            PlacementOffsets = PopulateOffsets();
            RegisterEventsAndProperties();

        }
        private void RegisterEventsAndProperties()
        {
            AssignShadowTarget();
            ZTeachingTipPopUp.Loaded += ZTeachingTipPopUp_Loaded;
            ZTeachingTipPopUp.Closed += ZTeachingTipPopUp_Closed;
            ZTeachingTipPopUp.Opened += ZTeachingTipPopUp_Opened;
            RootGrid.Loaded += RootGrid_Loaded;
            RootGrid.SizeChanged += ContentElement_SizeChanged;
            ActualPlacementChanged += ZTeachingTip_ActualPlacementChanged;
            TailPositioningStates.CurrentStateChanged += TailPositioningStates_CurrentStateChanged;
            RegisterPropertyChangedCallback(MaxHeightProperty, MaxHeightPropertyChanged);
            RegisterPropertyChangedCallback(MaxWidthProperty, (MaxWidthPropertyChangedCallBack));

            void AssignShadowTarget()
            {
                if (Window.Current.Content is FrameworkElement rootElement)
                {
                    RootShadow.CastTo = rootElement;
                    PolygonShadow.CastTo = rootElement;
                }

            }
        }

        #region ArrangementAndInitialPositioningCalaculation

        private void ZTeachingTipPopUp_Loaded(object sender, RoutedEventArgs e)
        {
            Bindings.Update();
        }
        private void ContentChanged(FrameworkElement newContentElement)
        {
            RootContentPresenter.Content = newContentElement;
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

        #region TailPositioningAndMarginCalculation

        private readonly double _tailCornerMargin = 15;

        private void ZTeachingTip_ActualPlacementChanged(ZTeachingTip cntrl, ActualPlacementChangedEventArgs arg)
        {
            if (arg.ActualPlacement == null)
            {
                return;
            }
            AssignTailPlacementBasedOnPlacementPReference(arg.ActualPlacement.Value);
        }
        private void AssignTailPlacementBasedOnPlacementPReference(ZTeachingTipPlacement placement)
        {
            var horizontalCornerTailOffset = CalculateHorizontalTailCornerMargin();
            var verticalCornerTailOffset = CalculateVerticalTailCornerMargin();
            switch (placement)
            {

                case ZTeachingTipPlacement.Top:
                    VisualStateManager.GoToState(this, nameof(Top), false);
                    AssignPolygonMargin(new Thickness(0, -1.5, 0, 0));
                    break;
                case ZTeachingTipPlacement.TopLeft:
                    VisualStateManager.GoToState(this, nameof(TopLeft), false);
                    AssignPolygonMargin(new Thickness(horizontalCornerTailOffset, -1.5, 0, 0));

                    break;
                case ZTeachingTipPlacement.TopRight:
                    VisualStateManager.GoToState(this, nameof(TopRight), false);
                    AssignPolygonMargin(new Thickness(0, -1.5, horizontalCornerTailOffset, 0));
                    break;
                case ZTeachingTipPlacement.Bottom:
                    VisualStateManager.GoToState(this, nameof(Bottom), false);
                    AssignPolygonMargin(new Thickness(0, 0, 0, -1.5));
                    break;
                case ZTeachingTipPlacement.BottomRight:
                    VisualStateManager.GoToState(this, nameof(BottomRight), false);
                    AssignPolygonMargin(new Thickness(0, 0, horizontalCornerTailOffset, -1.5));

                    break;
                case ZTeachingTipPlacement.BottomLeft:
                    VisualStateManager.GoToState(this, nameof(BottomLeft), false);
                    AssignPolygonMargin(new Thickness(horizontalCornerTailOffset, 0, 0, -1.5));

                    break;
                case ZTeachingTipPlacement.Left:
                    VisualStateManager.GoToState(this, nameof(Left), false);
                    AssignPolygonMargin(new Thickness(-1.5, 0, 0, 0));
                    break;
                case ZTeachingTipPlacement.LeftTop:
                    VisualStateManager.GoToState(this, nameof(LeftTop), false);
                    AssignPolygonMargin(new Thickness(-1.5, verticalCornerTailOffset, 0, 0));

                    break;
                case ZTeachingTipPlacement.LeftBottom:
                    VisualStateManager.GoToState(this, nameof(LeftBottom), false);
                    AssignPolygonMargin(new Thickness(-1.5, 0, 0, verticalCornerTailOffset));

                    break;
                case ZTeachingTipPlacement.Right:
                    VisualStateManager.GoToState(this, nameof(Right), false);
                    AssignPolygonMargin(new Thickness(0, 0, -1.5, 0));
                    break;
                case ZTeachingTipPlacement.RightTop:
                    VisualStateManager.GoToState(this, nameof(RightTop), true);
                    AssignPolygonMargin(new Thickness(0, verticalCornerTailOffset, -1.5, 0));

                    break;
                case ZTeachingTipPlacement.RightBottom:
                    VisualStateManager.GoToState(this, nameof(RightBottom), true);
                    AssignPolygonMargin(new Thickness(0, 0, -1.5, verticalCornerTailOffset));
                    break;
                default:
                    break;
            }
            void AssignPolygonMargin(Thickness polygonMargin)
            {
                TailPolygonMargin = polygonMargin;
            }


            //Tail Will  align With Target's Center if Target is Smaller  Than popup else tail will Offset From corner using Default Values   
            double CalculateHorizontalTailCornerMargin()
            {
                if (Target is null)
                {
                    return _tailCornerMargin;
                }
                var oneThirdWidthOfPopUp = PopUpCoordinatesInCoreWindowSpace.Width / 3;
                if (TargetCoordinatesInCoreWindowSpace.Width <= oneThirdWidthOfPopUp)
                {
                    var targetCenterPoint = TargetCoordinatesInCoreWindowSpace.Width / 2;
                    var tailHalfWidth = TailPolygon.ActualWidth / 2;
                    return targetCenterPoint - tailHalfWidth;
                }
                return _tailCornerMargin;
            }

            double CalculateVerticalTailCornerMargin()
            {
                if (Target is null)
                {
                    return _tailCornerMargin;
                }
                var oneThirdHeightOfPopup = PopUpCoordinatesInCoreWindowSpace.Height / 3;
                if (TargetCoordinatesInCoreWindowSpace.Height <= oneThirdHeightOfPopup)
                {
                    var targetCenterPoint = TargetCoordinatesInCoreWindowSpace.Height / 2;
                    var tailHalfHeight = TailPolygon.ActualHeight / 2;
                    return targetCenterPoint - tailHalfHeight;
                }
                return _tailCornerMargin;
            }

        }
        #endregion



        private void IsOpenPropertyChanged()
        {
            if (IsOpen)
            {
                SubscribeToSizeChangeNotification();
                ZTeachingTipPopUp.IsOpen = true;
                return;
            }
            UnSubscribeToSizeChangeNotification();
            ZTeachingTipPopUp.IsOpen = false;
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
            CoreWindow.GetForCurrentThread().SizeChanged += CoreWindowResizeCompleted;

        }

        private void OnLightDismissModeChanged(LightDismissOverlayMode dissDismissMode)
        {
            ZTeachingTipPopUp.LightDismissOverlayMode = dissDismissMode;
        }

        private void ContentElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _popUpHeight = RootGrid.ActualHeight;
            _popUpWidth = RootGrid.ActualWidth;
            AssignTargetDimentionIfExist();
            PositionPopUp();

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


        private List<ZTeachingTipPlacement> PopulateOffsets()
        {
            return new List<ZTeachingTipPlacement>()
            {
                {
                    ZTeachingTipPlacement.TopLeft
                },
                {
                    ZTeachingTipPlacement.Top
                },
                {
                    ZTeachingTipPlacement.TopRight
                },
                {
                    ZTeachingTipPlacement.RightTop
                },
                {
                    ZTeachingTipPlacement.Right
                },
                {
                    ZTeachingTipPlacement.RightBottom
                },
                {
                    ZTeachingTipPlacement.BottomRight
                },
                {
                    ZTeachingTipPlacement.Bottom
                },
                {
                    ZTeachingTipPlacement.BottomLeft
                },
                {
                    ZTeachingTipPlacement.LeftBottom
                },
                {
                    ZTeachingTipPlacement.Left
                },
                {
                    ZTeachingTipPlacement.LeftTop
                }

            };
        }

        #region PopUpPositioning Region

        private List<ZTeachingTipPlacement> PlacementOffsets { get; }

        private Rect WindowBounds
        {
            get => Window.Current.Bounds;
        }

        private Rect PopUpCoordinatesInCoreWindowSpace
        {
            get => ZTeachingTipPopUp.TransformToVisual(Window.Current.Content).TransformBounds(AssignDefaultsToRect(ref _popupRect, _popUpHeight, _popUpWidth));
        }

        private Rect TargetCoordinatesInCoreWindowSpace
        {
            get => Target.TransformToVisual(Window.Current.Content).TransformBounds(AssignDefaultsToRect(ref _targetRect, _targetHeight, _targetWidth));
        }

        private Thickness SpaceAroundTarget
        {
            get
            {
                _spaceAroundTarget.Left = TargetCoordinatesInCoreWindowSpace.X;
                _spaceAroundTarget.Top = TargetCoordinatesInCoreWindowSpace.Y;
                _spaceAroundTarget.Bottom = WindowBounds.Height - (Target.ActualHeight + TargetCoordinatesInCoreWindowSpace.Y);
                _spaceAroundTarget.Right = WindowBounds.Width - (Target.ActualWidth + TargetCoordinatesInCoreWindowSpace.X);
                return _spaceAroundTarget;
            }
        }

        private Rect AssignDefaultsToRect(ref Rect rect, double height, double width)
        {
            rect.X = 0.0;
            rect.Y = 0.0;
            rect.Width = width;
            rect.Height = height;
            return rect;
        }


        private void PositionPopUp()
        {
            if (!IsOpen)//No Positioning calculation are Made if Popup is Not opened 
            {
                return;
            }
            var isTeachingTipFit = Target is null ? PositionPopUpUnTargeted() : PositionPopUpBasedOnTarget(Target);
            if (!ShouldBoundToXamlRoot || isTeachingTipFit)
            {
                return;
            } //if Control should be contained within xaml root but size not enough to show so  Hiding the PopUp to Avoid Clipping
            ActualPlacement = null;
            IsOpen = false;
        }

        private bool PositionPopUpUnTargeted()
        {
            //Calculation For Window Width and Placement is Done accordingly
            return false;
        }

        private bool PositionPopUpBasedOnTarget(FrameworkElement targetElement)
        {
            return CallExtensionToPositionPopUp();
        }


        private bool CallExtensionToPositionPopUp()
        {

            var isPopUpPositioned = CallTryShowNearWithMappedPlacement(PreferredPlacement);


            if (!ShouldBoundToXamlRoot || isPopUpPositioned)
            {
                ActualPlacement = PreferredPlacement;
                AssignTailPlacementBasedOnPlacementPReference(PreferredPlacement);
                return isPopUpPositioned;
            }

            var startIndex = PlacementOffsets.IndexOf(PreferredPlacement);

            for (var i = 0; i < PlacementOffsets.Count - 1; i++)
            {
                if (startIndex >= PlacementOffsets.Count - 1)
                {
                    startIndex = -1;
                }
                isPopUpPositioned = CallTryShowNearWithMappedPlacement(PlacementOffsets[++startIndex]);

                if (!isPopUpPositioned)
                {
                    continue;
                }
                ActualPlacement = PlacementOffsets[startIndex];
                AssignTailPlacementBasedOnPlacementPReference(PlacementOffsets[startIndex]);
                return true;
            }
            return false;
        }

        private bool CallTryShowNearWithMappedPlacement(ZTeachingTipPlacement placement)
        {

            var mappedPlacement = MapTeachingTipPlacementToPopUpPlacement(placement);

            var popUpDimensionToBeConsideredForPositioning = AddPolygonDimensionToPopUpDimension(mappedPlacement);

            return ZTeachingTipPopUp.TryShowNearRect(popUpDimensionToBeConsideredForPositioning,
                 TargetCoordinatesInCoreWindowSpace,
                 new[] { mappedPlacement },
                 PlacementMargin,
                 ShouldBoundToXamlRoot);

        }

        /*
         * When performing Positioning Calculation for Popup ,Dimension of Polygon must be included to avoid Layout Cycle
         * (i.e)if Actual Placement is Top(Initial placement is left there is no space ,so placement on top),
         * Due To size changed event in RootGrid due to Polygon moving to different location, Positing popup Occurs again now when calculating space for left ,space is available
         * because Polygon dimension  is not taken into consideration,when polygon moves to left (size changes occurs popup moves to top) cycles repeats,
         * to Avoid the cycle polygon dimension are Taken into Consideration even if the polygon is not on respective Side  
         */
        private Rect AddPolygonDimensionToPopUpDimension(PopupPlacementMode requestedPlacement)
        {
            var popUpDimensionToBeConsideredForPositioning = PopUpCoordinatesInCoreWindowSpace;
            const double polygonWidth = 10;

            if (!(ActualPlacement is { } actualPlacement))//Popup Not yet Placed No Die
            {
                return popUpDimensionToBeConsideredForPositioning;
            }

            if (IsVerticalPlacement(MapTeachingTipPlacementToPopUpPlacement(actualPlacement)) == IsVerticalPlacement(requestedPlacement))//Calculating popUp Positioning,where requested placement Axis  current axis (Y or X Axis) is same or not 
            {
                return popUpDimensionToBeConsideredForPositioning;
            }

            if (IsHorizontalPlacement(requestedPlacement))//Popup is in Top or Bottom But Calculation Dimension should be Suitable for both Left and Right 
            {

                popUpDimensionToBeConsideredForPositioning.Width += polygonWidth;
                return popUpDimensionToBeConsideredForPositioning;
            }

            if (IsVerticalPlacement(requestedPlacement))//Popup is in Left or Right  But Calculation Dimension should be Suitable for both Top and Bottom
            {
                popUpDimensionToBeConsideredForPositioning.Height += polygonWidth;
                return popUpDimensionToBeConsideredForPositioning;
            }

            return popUpDimensionToBeConsideredForPositioning;


            //Checking Whether The Given placement is Top or Bottom side placement
            bool IsVerticalPlacement(PopupPlacementMode placement)
            {
                return placement switch
                {
                    PopupPlacementMode.TopLeft => true,
                    PopupPlacementMode.TopRight => true,
                    PopupPlacementMode.BottomLeft => true,
                    PopupPlacementMode.BottomRight => true,
                    PopupPlacementMode.Bottom => true,
                    PopupPlacementMode.Top => true,
                    _ => false,
                };

            }

            //Checking Whether The Given placement is LEft or Right  side placement
            bool IsHorizontalPlacement(PopupPlacementMode placement)
            {
                return placement switch
                {
                    PopupPlacementMode.Left => true,
                    PopupPlacementMode.Right => true,
                    PopupPlacementMode.LeftBottom => true,
                    PopupPlacementMode.RightBottom => true,
                    PopupPlacementMode.RightTop => true,
                    PopupPlacementMode.LeftTop => true,
                    _ => false
                };

            }
        }

        private PopupPlacementMode MapTeachingTipPlacementToPopUpPlacement(ZTeachingTipPlacement placement)
        {
            var mappedPlacement = placement switch
            {
                ZTeachingTipPlacement.Top => PopupPlacementMode.Top,
                ZTeachingTipPlacement.TopLeft => PopupPlacementMode.TopLeft,
                ZTeachingTipPlacement.TopRight => PopupPlacementMode.TopRight,
                ZTeachingTipPlacement.Bottom => PopupPlacementMode.Bottom,
                ZTeachingTipPlacement.BottomRight => PopupPlacementMode.BottomRight,
                ZTeachingTipPlacement.BottomLeft => PopupPlacementMode.BottomLeft,
                ZTeachingTipPlacement.Left => PopupPlacementMode.Left,
                ZTeachingTipPlacement.LeftTop => PopupPlacementMode.LeftTop,
                ZTeachingTipPlacement.LeftBottom => PopupPlacementMode.LeftBottom,
                ZTeachingTipPlacement.Right => PopupPlacementMode.Right,
                ZTeachingTipPlacement.RightTop => PopupPlacementMode.RightTop,
                ZTeachingTipPlacement.RightBottom => PopupPlacementMode.RightBottom,
                _ => throw new ArgumentOutOfRangeException(nameof(placement), placement, null)
            };
            return mappedPlacement;
        }


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

    public class ZTeachingTipOpenedEventArgs : EventArgs
    {

    }

    public class ZTeachingTipClosedEventArgs : EventArgs
    {

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