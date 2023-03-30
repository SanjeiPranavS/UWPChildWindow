using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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
            nameof(CloseButtonStyle), typeof(Style), typeof(ZTeachingTip), new PropertyMetadata(new Style()
            {
                TargetType = typeof(Button),
                Setters =
                {
                    new Setter(Button.BackgroundProperty, Windows.UI.Colors.Transparent),
                    new Setter(Button.BorderBrushProperty, Windows.UI.Colors.Transparent),
                    new Setter(Button.CornerRadiusProperty, new CornerRadius(4)),
                    new Setter(Button.PaddingProperty, new Thickness(3)),
                    new Setter(Button.FontSizeProperty, 7)
                }
            }));

        public readonly static DependencyProperty LightDismissModeProperty = DependencyProperty.Register(
            nameof(LightDismissMode), typeof(LightDismissOverlayMode), typeof(ZTeachingTip), new PropertyMetadata(LightDismissOverlayMode.Auto, OnLightDismissModePropertyChanged));


        public readonly static DependencyProperty TailBackGroundProperty = DependencyProperty.Register(
            nameof(TailBackGround), typeof(Brush), typeof(ZTeachingTip), new PropertyMetadata(default(Brush), (TailBackGroundChangedCallback)));

        public readonly static DependencyProperty TailVisibilityProperty = DependencyProperty.Register(
            nameof(TailVisibility), typeof(Visibility), typeof(ZTeachingTip), new PropertyMetadata(Visibility.Visible));

        public readonly static DependencyProperty ShadowCastTargetProperty = DependencyProperty.Register(
            nameof(ShadowCastTarget), typeof(FrameworkElement), typeof(ZTeachingTip), new PropertyMetadata(default(FrameworkElement), (ShadowCastTargetChangedCallBack)));


        public FrameworkElement ShadowCastTarget
        {
            get => (FrameworkElement)GetValue(ShadowCastTargetProperty);
            set => SetValue(ShadowCastTargetProperty, value);
        }
        public Visibility TailVisibility
        {
            get => (Visibility)GetValue(TailVisibilityProperty);
            set => SetValue(TailVisibilityProperty, value);
        }

        public readonly static DependencyProperty ContentHeightProperty = DependencyProperty.Register(
            nameof(ContentHeight), typeof(double), typeof(ZTeachingTip), new PropertyMetadata(default(double), (InnerContentHeightChangedCallBack)));


        public readonly static DependencyProperty ContentWidthProperty = DependencyProperty.Register(
            nameof(ContentWidth), typeof(double), typeof(ZTeachingTip), new PropertyMetadata(default(double), (InnerContentWightChangedCallBack)));


        public double ContentWidth
        {
            get => (double)GetValue(ContentWidthProperty);
            set => SetValue(ContentWidthProperty, value);
        }

        public double ContentHeight
        {
            get => (double)GetValue(ContentHeightProperty);
            set => SetValue(ContentHeightProperty, value);
        }

        public Brush TailBackGround
        {
            get => (Brush)GetValue(TailBackGroundProperty);
            set => SetValue(TailBackGroundProperty, value);
        }

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

        public readonly static DependencyProperty CloseButtonContentProperty = DependencyProperty.Register(
            nameof(CloseButtonContent), typeof(object), typeof(ZTeachingTip), new PropertyMetadata(default));

        public object CloseButtonContent
        {
            get => (object)GetValue(CloseButtonContentProperty);
            set => SetValue(CloseButtonContentProperty, value);
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

        private readonly static DependencyProperty TailPolygonMarginProperty = DependencyProperty.Register(
            nameof(TailPolygonMargin), typeof(Thickness), typeof(ZTeachingTip), new PropertyMetadata(default(Thickness)));

        public Thickness TailPolygonMargin
        {
            get => (Thickness)GetValue(TailPolygonMarginProperty);
            set => SetValue(TailPolygonMarginProperty, value);
        }

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


        public event Action<ZTeachingTip, ActualPlacementChangedEventArgs> ActualPlacementChanged;

        public event Action<object, RoutedEventArgs> CloseButtonClicked;

        public event Action<ZTeachingTip, ZTeachingTipOpenedEventArgs> Opened;

        public event Action<ZTeachingTip, ZTeachingTipClosedEventArgs> Closed;

        #endregion

        #region PropertyChangedCallBack

        private static void TailBackGroundChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip tip && e.NewValue is Brush newBrush)
            {
                tip.TailPolygon.Fill = newBrush;
                tip.TailPolygon.Stroke = newBrush;
            }
        }
        private static void InnerContentHeightChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip tip && e.NewValue is double height)
            {
                tip.RootContentPresenter.Height = height;
            }
        }
        private static void ShadowCastTargetChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip tip && e.NewValue is FrameworkElement newShadowCastTarget)
            {
                tip.RootShadow.CastTo = newShadowCastTarget;
                tip.PolygonShadow.CastTo = newShadowCastTarget;
            }
        }

        private static void InnerContentWightChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTeachingTip tip && e.NewValue is double width)
            {
                tip.RootContentPresenter.Width = width;
            }
        }

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
            if (d is ZTeachingTip teachingTip && e.OldValue is bool oldValue && e.NewValue is bool newValue)
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
                    //newElement.SizeChanged += teachingTip.ContentElement_SizeChanged;
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
        private void TailPositioningStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name.Equals(e.OldState.Name))
            {
                return;
            }
            ContentElement_SizeChanged(default,default);
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            CloseButtonClicked?.Invoke(this, e);
            IsOpen = false;
        }

        private void ZTeachingTipContainerContentChangedCallBack(DependencyObject sender, DependencyProperty dp)
        {
            if (!(GetValue(dp) is FrameworkElement newContent))
            {
                return;
            }
            ContentChanged(newContent);
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

            PlacementOffsets = PopulateOffsets();
            this.InitializeComponent();
            RegisterEventsAndProperties();

        }


        private void RegisterEventsAndProperties()
        {
            //RegisterPropertyChangedCallback(ContentProperty, ZTeachingTipContainerContentChangedCallBack);
            AssignShadowTarget();
            ZTeachingTipPopUp.Loaded += ZTeachingTipPopUp_Loaded;
            ZTeachingTipPopUp.Closed += ZTeachingTipPopUp_Closed;
            ZTeachingTipPopUp.Opened += ZTeachingTipPopUp_Opened;
            RootGrid.Loaded += RootGrid_Loaded;
            RootGrid.SizeChanged += ContentElement_SizeChanged;
            ActualPlacementChanged += ZTeachingTip_ActualPlacementChanged;
            TailPositioningStates.CurrentStateChanged += TailPositioningStates_CurrentStateChanged;

            void AssignShadowTarget()
            {
                if (!(Window.Current.Content is FrameworkElement rootElement))
                {
                    return;
                }
                RootShadow.CastTo = rootElement;
                PolygonShadow.CastTo = rootElement;
            }
        }

        #region ArrangementAndInitialPositioningCalaculation

        private void ZTeachingTipPopUp_Loaded(object sender, RoutedEventArgs e)
        {
            Bindings.Update();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return new Size(0.0, 0.0);
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(0.0, 0.0);
        }


        private void ContentChanged(FrameworkElement newContentElement)
        {
            RootContentPresenter.Content = newContentElement;
            if (newContentElement.IsLoaded)
            {
                _popUpHeight = RootGrid.ActualHeight;
                _popUpWidth = RootGrid.ActualWidth;
                return;
            }
            RootGrid.Measure(new Size(double.MaxValue, double.MaxValue));
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
                    VisualStateManager.GoToState(this, nameof(RightTop), false);
                    AssignPolygonMargin(new Thickness(0, verticalCornerTailOffset, -1.5, 0));

                    break;
                case ZTeachingTipPlacement.RightBottom:
                    VisualStateManager.GoToState(this, nameof(RightBottom), false);
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
                //SelfAddToRootElementWithTargetIfParentNotAvailable();
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


        private List<ZTeachingTipPlacement> PlacementOffsets { get; }


        private void PositionPopUp()
        {
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
                AssignTailPlacementBasedOnPlacementPReference(PreferredPlacement);
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
                    AssignTailPlacementBasedOnPlacementPReference(PreferredPlacement);
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
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + distanceX + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;

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
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + distanceX + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;
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
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + distanceY - PopUpCoordinatesInCoreWindowSpace.Height >= 0 && PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;


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
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + distanceY - PopUpCoordinatesInCoreWindowSpace.Height >= 0 && PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;


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

        #region TeachingTipAnimationRegion

        //public void CreateExpandAnimation()
        //{
        //    var compositor = Window.Current.Compositor;

        //    CompositionEasingFunction expandEasingFunction = null;
        //    expandEasingFunction = () =>
        //    {
        //        if (m_expandEasingFunction == null)
        //        {
        //            var easingFunction = compositor.CreateCubicBezierEasingFunction(s_expandAnimationEasingCurveControlPoint1, s_expandAnimationEasingCurveControlPoint2);
        //            m_expandEasingFunction = easingFunction;
        //            return easingFunction;
        //        }
        //        return m_expandEasingFunction;
        //    }();

        //    m_expandAnimation = () =>
        //    {
        //        var expandAnimation = compositor.CreateVector3KeyFrameAnimation();
        //        if (m_tailOcclusionGrid != null)
        //        {
        //            expandAnimation.SetScalarParameter("Width", (float)m_tailOcclusionGrid.ActualWidth);
        //            expandAnimation.SetScalarParameter("Height", (float)m_tailOcclusionGrid.ActualHeight);
        //        }
        //        else
        //        {
        //            expandAnimation.SetScalarParameter("Width", s_defaultTipHeightAndWidth);
        //            expandAnimation.SetScalarParameter("Height", s_defaultTipHeightAndWidth);
        //        }

        //        expandAnimation.InsertExpressionKeyFrame(0.0f, "Vector3(Min(0.01, 20.0 / Width), Min(0.01, 20.0 / Height), 1.0)");
        //        expandAnimation.InsertKeyFrame(1.0f, new Vector3(1.0f, 1.0f, 1.0f), expandEasingFunction);
        //        expandAnimation.Duration = m_expandAnimationDuration;
        //        expandAnimation.Target = s_scaleTargetName;
        //        return expandAnimation;
        //    };

        //    m_expandElevationAnimation = () =>
        //    {
        //        var expandElevationAnimation = compositor.CreateVector3KeyFrameAnimation();
        //        expandElevationAnimation.InsertExpressionKeyFrame(1.0f, "Vector3(this.Target.Translation.X, this.Target.Translation.Y, contentElevation)", expandEasingFunction);
        //        expandElevationAnimation.SetScalarParameter("contentElevation", m_contentElevation);
        //        expandElevationAnimation.Duration = m_expandAnimationDuration;
        //        expandElevationAnimation.Target = s_translationTargetName;
        //        return expandElevationAnimation;
        //    };
        //}
        //public void CreateContractAnimation()
        //{
        //    var compositor = Window.Current.Compositor;
        //    var contractEasingFunction = () =>
        //    {
        //        if (m_contractEasingFunction == null)
        //        {
        //            var easingFunction = compositor.CreateCubicBezierEasingFunction(s_contractAnimationEasingCurveControlPoint1, s_contractAnimationEasingCurveControlPoint2);
        //            m_contractEasingFunction = easingFunction;
        //            return easingFunction;
        //        }
        //        return m_contractEasingFunction;
        //    }();

        //    m_contractAnimation = () =>
        //    {
        //        var contractAnimation = compositor.CreateVector3KeyFrameAnimation();
        //        if (m_tailOcclusionGrid != null)
        //        {
        //            contractAnimation.SetScalarParameter("Width", (float)m_tailOcclusionGrid.ActualWidth);
        //            contractAnimation.SetScalarParameter("Height", (float)m_tailOcclusionGrid.ActualHeight);
        //        }
        //        else
        //        {
        //            contractAnimation.SetScalarParameter("Width", s_defaultTipHeightAndWidth);
        //            contractAnimation.SetScalarParameter("Height", s_defaultTipHeightAndWidth);
        //        }

        //        contractAnimation.InsertKeyFrame(0.0f, new Vector3(1.0f, 1.0f, 1.0f));
        //        contractAnimation.InsertExpressionKeyFrame(1.0f, "Vector3(20.0 / Width, 20.0 / Height, 1.0)", contractEasingFunction);
        //        contractAnimation.Duration = m_contractAnimationDuration;
        //        contractAnimation.Target = s_scaleTargetName;
        //        return contractAnimation;
        //    };

        //    m_contractElevationAnimation = () =>
        //    {
        //        var contractElevationAnimation = compositor.CreateVector3KeyFrameAnimation();
        //        // animating to 0.01f instead of 0.0f as work around to internal issue 26001712 which was causing text clipping.
        //        contractElevationAnimation.InsertExpressionKeyFrame(1.0f, "Vector3(this.Target.Translation.X, this.Target.Translation.Y, 0.01f)", contractEasingFunction);
        //        contractElevationAnimation.Duration = m_contractAnimationDuration;
        //        contractElevationAnimation.Target = s_translationTargetName;
        //        return contractElevationAnimation;
        //    };
        //}
        //{
        //void TeachingTip.StartExpandToOpen()
        //{
        //    Debug.Assert(this is winrt.IUIElement9, "The contract and expand animations currently use facade's which were not available pre-RS5.");
        //    if (m_expandAnimation == null)
        //    {
        //        CreateExpandAnimation();
        //    }
        //    var scopedBatch = Window.Current.Compositor().CreateScopedBatch(CompositionBatchTypes.Animation);
        //    {
        //        if (m_expandAnimation is { } expandAnimation)
        //        {
        //            if (m_tailOcclusionGrid is { } tailOcclusionGrid)
        //            {
        //                tailOcclusionGrid.StartAnimation(expandAnimation);
        //                m_isExpandAnimationPlaying = true;
        //            }
        //            if (m_tailEdgeBorder is { }
        //                    tailEdgeBorder)
        //            {
        //                tailEdgeBorder.StartAnimation(expandAnimation);
        //                m_isExpandAnimationPlaying = true;
        //            }
        //        }
        //        if (m_expandElevationAnimation is { } expandElevationAnimation)
        //        {
        //            if (m_contentRootGrid is { } contentRootGrid)
        //            {
        //                contentRootGrid.StartAnimation(expandElevationAnimation);
        //                m_isExpandAnimationPlaying = true;
        //            }
        //        }
        //    }
        //    scopedBatch.End();

        //    scopedBatch.Completed += (batch, status) =>
        //    {
        //        m_isExpandAnimationPlaying = false;
        //        if (!m_isContractAnimationPlaying)
        //        {
        //            SetIsIdle(true);
        //        }
        //    };

        //    // Under normal circumstances we would have launched an animation just now, if we did not then we should make sure that the idle state is correct
        //    if (!m_isExpandAnimationPlaying && !m_isContractAnimationPlaying)
        //    {
        //        SetIsIdle(true);
        //    }

        //}
        //public void StartContractToClose()
        //{
        //    if (!(this is winrt.IUIElement9))
        //    {
        //        throw new Exception("The contract and expand animations currently use facades which were not available pre RS5.");
        //    }

        //    if (m_contractAnimation == null)
        //    {
        //        CreateContractAnimation();
        //    }

        //    using (var scopedBatch = Window.Current.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation))
        //    {
        //        if (m_contractAnimation != null)
        //        {
        //            if (m_tailOcclusionGrid != null)
        //            {
        //                m_tailOcclusionGrid.StartAnimation(m_contractAnimation);
        //                m_isContractAnimationPlaying = true;
        //            }
        //            if (m_tailEdgeBorder != null)
        //            {
        //                m_tailEdgeBorder.StartAnimation(m_contractAnimation);
        //                m_isContractAnimationPlaying = true;
        //            }
        //        }

        //        if (m_contractElevationAnimation != null)
        //        {
        //            if (m_contentRootGrid != null)
        //            {
        //                m_contentRootGrid.StartAnimation(m_contractElevationAnimation);
        //                m_isContractAnimationPlaying = true;
        //            }
        //        }

        //        scopedBatch.End();

        //        scopedBatch.Completed += (sender, args) =>
        //        {
        //            m_isContractAnimationPlaying = false;
        //            ClosePopup();
        //            if (!m_isExpandAnimationPlaying)
        //            {
        //                SetIsIdle(true);
        //            }
        //        };
        //    }
        //}

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