using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.Toolkit.Uwp.Helpers;

namespace ZTeachingTip
{
    #region Supporting Enums and Constants

    // TO DO: Make Side and Alignment  private after removing external references. 
    // Expose only PopUpPlacement enum to user.

    // Defines which side of the given element the popup has to be on.
    public enum Side { Left, Top, Right, Bottom };

    // Defines which edge of the given element has to be aligned with the corresponding edge of the targetElement.
    // For example, when Alignment is Left, the left edge of given element is aligned with left edge of targetElement.
    public enum Alignment { Left, Top, Center, Right, Bottom };

    // TO DO: Remove PlacementPreferenceOrders, HorizontalAlignmentPreferenceOrders, and VerticalAlignmentPreferenceOrders.
    public static class PlacementPreferenceOrders
    {
        // Define additional preference orders here as needed and pass them instead of creating a new array each time.
        public static Side[] TopBottomLeft = new Side[] { Side.Top, Side.Bottom, Side.Left };
        public static Side[] TopBottomLeftRight = new Side[] { Side.Top, Side.Bottom, Side.Left, Side.Right }; //Default preference
        public static Side[] BottomTopLeftRight = new Side[] { Side.Bottom, Side.Top, Side.Left, Side.Right };
        public static Side[] Left = new Side[] { Side.Left };
        public static Side[] Right = new Side[] { Side.Right };
        public static Side[] Top = new Side[] { Side.Top };
        public static Side[] Bottom = new Side[] { Side.Bottom };
    }

    public static class HorizontalAlignmentPreferenceOrders
    {
        // Define additional preference orders here as needed and pass them instead of creating a new array each time.
        public static Alignment[] LeftCenterRight = new Alignment[] { Alignment.Left, Alignment.Center, Alignment.Right }; // Default preference
        public static Alignment[] CenterLeftRight = new Alignment[] { Alignment.Center, Alignment.Left, Alignment.Right };
        public static Alignment[] CenterRightLeft = new Alignment[] { Alignment.Center, Alignment.Right, Alignment.Left };
        public static Alignment[] CenterRight = new Alignment[] { Alignment.Center, Alignment.Right };
        public static Alignment[] Right = new Alignment[] { Alignment.Right };
        public static Alignment[] Center = new Alignment[] { Alignment.Center };
    }

    public static class VerticalAlignmentPreferenceOrders
    {
        // Define additional preference orders here as needed and pass them instead of creating a new array each time.
        public static Alignment[] TopCenterBottom = new Alignment[] { Alignment.Top, Alignment.Center, Alignment.Bottom }; // Default preference
        public static Alignment[] CenterTopBottom = new Alignment[] { Alignment.Center, Alignment.Top, Alignment.Bottom };
        public static Alignment[] CenterBottomTop = new Alignment[] { Alignment.Center, Alignment.Bottom, Alignment.Top };
    }

    // Defines preferred position of the popup with respect to given element.
    // Refer https://learn.microsoft.com/en-us/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.teachingtipplacementmode?view=windows-app-sdk-1.2#fields.
    public enum PopupPlacementMode
    {
        /// <summary>
        /// Along the Top side of the xaml root when non-targeted and Above the target element with centers aligned when targeted.
        /// </summary>
        Top,

        /// <summary>
        /// The Tpo left corner of the xaml root when non-targeted and Above the target element aligning left sides when targeted
        /// </summary>
        TopLeft,

        /// <summary>
        /// The Top right corner of the xaml root when non-targeted and Above the target element aligning Right sides when targeted
        /// </summary>
        TopRight,

        /// <summary>
        /// Along the bottom side of the xaml root when non-targeted and below the target element with centers aligned when targeted.
        /// </summary>
        Bottom,

        /// <summary>
        /// The bottom right corner of the xaml root when non-targeted and below the target element aligning Right sides when targeted
        /// </summary>
        BottomRight,

        /// <summary>
        /// The bottom left corner of the xaml root when non-targeted and below the target element aligning left sides when targeted
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Along the left side of the xaml root when non-targeted and left side of the Target with centers aligned  when targeted.
        /// </summary>
        Left,

        /// <summary>
        /// Along the left side of the xaml root when non-targeted and left side of the Target with Top aligned  when targeted.
        /// </summary>
        LeftTop,


        /// <summary>
        /// Along the left side of the xaml root when non-targeted and left side of the Target with Bottom aligned  when targeted.
        /// </summary>
        LeftBottom,

        /// <summary>
        /// Along the right side of the xaml root when non-targeted and Right side of the Target with centers aligned  when targeted.
        /// </summary>
        Right,

        /// <summary>
        /// Along the right side of the xaml root when non-targeted and Right side of the Target with Top aligned  when targeted.
        /// </summary>
        RightTop,

        /// <summary>
        /// Along the right side of the xaml root when non-targeted and Right side of the Target with Bottom aligned  when targeted.
        /// </summary>
        RightBottom,
    }

    public static class PopupPlacementOrders
    {
        public static readonly IEnumerable<PopupPlacementMode> BottomRightTopLeft = new[]
        {
            PopupPlacementMode.Bottom,
            PopupPlacementMode.BottomLeft,
            PopupPlacementMode.BottomRight,
            PopupPlacementMode.RightBottom,
            PopupPlacementMode.Right,
            PopupPlacementMode.RightTop,
            PopupPlacementMode.TopLeft,
            PopupPlacementMode.Top,
            PopupPlacementMode.TopRight,
            PopupPlacementMode.LeftTop,
            PopupPlacementMode.Left,
            PopupPlacementMode.LeftBottom
        };
        public static readonly IEnumerable<PopupPlacementMode> RightTopLeftBottom = new[]
        {
            PopupPlacementMode.Right,
            PopupPlacementMode.RightTop,
            PopupPlacementMode.RightBottom,
            PopupPlacementMode.TopLeft,
            PopupPlacementMode.Top,
            PopupPlacementMode.TopRight,
            PopupPlacementMode.LeftTop,
            PopupPlacementMode.Left,
            PopupPlacementMode.LeftBottom,
            PopupPlacementMode.BottomRight,
            PopupPlacementMode.Bottom,
            PopupPlacementMode.BottomLeft,
        };
        public static readonly IEnumerable<PopupPlacementMode> TopLeftBottomRight = new[]
        {
            PopupPlacementMode.Top,
            PopupPlacementMode.TopRight,
            PopupPlacementMode.TopLeft,
            PopupPlacementMode.LeftTop,
            PopupPlacementMode.Left,
            PopupPlacementMode.LeftBottom,
            PopupPlacementMode.BottomRight,
            PopupPlacementMode.Bottom,
            PopupPlacementMode.BottomLeft,
            PopupPlacementMode.RightBottom,
            PopupPlacementMode.Right,
            PopupPlacementMode.RightTop
        };
        public static readonly IEnumerable<PopupPlacementMode> LeftBottomRightTop = new[]
        {
            PopupPlacementMode.Left,
            PopupPlacementMode.LeftBottom,
            PopupPlacementMode.LeftTop,
            PopupPlacementMode.BottomRight,
            PopupPlacementMode.Bottom,
            PopupPlacementMode.BottomLeft,
            PopupPlacementMode.RightBottom,
            PopupPlacementMode.Right,
            PopupPlacementMode.RightTop,
            PopupPlacementMode.TopLeft,
            PopupPlacementMode.Top,
            PopupPlacementMode.TopRight,
        };
        public static readonly IEnumerable<PopupPlacementMode> TopBottomLeftRight = new[]
        {
            PopupPlacementMode.Top,
            PopupPlacementMode.TopRight,
            PopupPlacementMode.TopLeft,
            PopupPlacementMode.Bottom,
            PopupPlacementMode.BottomLeft,
            PopupPlacementMode.BottomRight,
            PopupPlacementMode.Left,
            PopupPlacementMode.LeftBottom,
            PopupPlacementMode.LeftTop,
            PopupPlacementMode.Right,
            PopupPlacementMode.RightTop,
            PopupPlacementMode.RightBottom
        };
        public static readonly IEnumerable<PopupPlacementMode> BottomTopLeftRight = new[]
        {
            PopupPlacementMode.Bottom,
            PopupPlacementMode.BottomLeft,
            PopupPlacementMode.BottomRight,
            PopupPlacementMode.Top,
            PopupPlacementMode.TopRight,
            PopupPlacementMode.TopLeft,
            PopupPlacementMode.Left,
            PopupPlacementMode.LeftBottom,
            PopupPlacementMode.LeftTop,
            PopupPlacementMode.Right,
            PopupPlacementMode.RightTop,
            PopupPlacementMode.RightBottom
        };
        public static readonly IEnumerable<PopupPlacementMode> TopBottomLeft = new[]
        {
            PopupPlacementMode.Top,
            PopupPlacementMode.TopRight,
            PopupPlacementMode.TopLeft,
            PopupPlacementMode.Bottom,
            PopupPlacementMode.BottomLeft,
            PopupPlacementMode.BottomRight,
            PopupPlacementMode.Left,
            PopupPlacementMode.LeftBottom,
            PopupPlacementMode.LeftTop
        };
        public static readonly IEnumerable<PopupPlacementMode> Left = new[]
        {
            PopupPlacementMode.Left,
            PopupPlacementMode.LeftBottom,
            PopupPlacementMode.LeftTop
        };
        public static readonly IEnumerable<PopupPlacementMode> Right = new[]
        {
            PopupPlacementMode.Right,
            PopupPlacementMode.RightTop,
            PopupPlacementMode.RightBottom
        };
        public static readonly IEnumerable<PopupPlacementMode> Top = new[]
        {
            PopupPlacementMode.Top,
            PopupPlacementMode.TopRight,
            PopupPlacementMode.TopLeft
        };
        public static readonly IEnumerable<PopupPlacementMode> Bottom = new[]
        {
            PopupPlacementMode.Bottom,
            PopupPlacementMode.BottomLeft,
            PopupPlacementMode.BottomRight
        };
    }
    #endregion

    public static class PopupExtension
    {
        #region Supporting Structs

        /// <summary>
        /// Bounds of current window. 
        /// </summary>
        private static Rect WindowBounds
        {
            get => Window.Current.Bounds;
        }

        /// <summary>
        /// Stores received values that are used to position the popup.
        /// </summary>
        private struct GivenPlacementParams
        {
            /// <summary>
            /// Initial coordinates of given <see cref="Popup"/> with respect to window's origin.
            /// </summary>
            public Rect InitialPopupBounds { get; set; }

            /// <summary>
            /// Coordinates of given target with respect to window's origin.
            /// </summary>
            public Rect TargetPositionBounds { get; set; }

            /// <summary>
            /// Margin to be applied while positioning the popup.
            /// </summary>
            public Thickness PlacementMargin { get; set; }

            /// <summary>
            /// Indicating Whether <see cref="Popup"/> Can position Outside Current Window
            /// </summary>
            public bool ShouldConstrainToWindowBounds { get; set; }

            /// <summary>
            /// Indicating Whether <see cref="Popup"/> can position inside Current Screen
            /// </summary>
            public bool ShouldConstrainToScreenBounds { get; set; }
        }

        /// <summary>
        /// Stores computed values that are used to position the popup.
        /// </summary>
        private struct ComputedPopupOffsets
        {
            public double HorizontalOffSet { get; set; }

            public double VerticalOffSet { get; set; }

            /// <summary>
            /// Indicates if the popup will be within window bounds after applying offsets.
            /// </summary>
            public bool WillPopupFitInWindow { get; set; }
        }

        

        #endregion

        #region TryShowNear and Overloads- TryShowNear, TryShowNearPoint, TryShowNearElement and TryShowNearRect.
        #region TryShowNear v1
        /// <summary>
        /// Use this method to show a popup near a Point. Note that the popup must have MaxHeight and MaxWidth properties defined.
        /// Refer TryShowNear overload below above for usage instructions.
        /// </summary>
        [Obsolete("This method will be deprecated, please use popup.TryShowNearPoint, popup.TryShowNearElement or TryShowNearRect.")]
        public static bool TryShowNear(this Popup popup, Point targetPoint, Side[] placementPreferenceOrder = null, Alignment[] verticalAlignmentPreferenceOrder = null, Alignment[] horizontalAlignmentPreferenceOrder = null, double margin = 10, bool isOverflowAllowed = false)
        {
            return popup.TryShowNear(null, targetPoint, placementPreferenceOrder, verticalAlignmentPreferenceOrder, horizontalAlignmentPreferenceOrder, margin, isOverflowAllowed);
        }

        /// <summary>
        /// Use this method to show a popup near another FrameworkElement or a Point. Note that the popup must have MaxHeight and MaxWidth properties defined.
        /// <para>Steps to use this extension:</para>
        /// <list type="number">
        /// <item>Define popup's MaxHeight and MaxWidth properties. This is required for the extension to work as expected.</item>
        /// <item>Define the required placement preference order, horizontal alignment order and vertical alignment order. You can skip this step if the default values are enough.</item>
        /// <item>Pass either a FrameworkElement or a Point to this method. Atleast one of the two are required. When both are given, the Target FrameWorkElement  is targeted. </item>
        /// <item>Call TryShowNear extension and check the boolean returned by it.</item>
        /// <item>If it returns false, position popup manually or call this method with isOverflow = true.</item>
        /// </list>
        /// </summary>
        /// <param name="targetElement">
        /// Framework element to position popup near.
        /// </param>
        /// <param name="targetPoint">
        /// Instead of passing targetPoint to this method, use the overloaded method below.
        /// </param>
        /// <param name="placementPreferenceOrder">
        /// This preference order is used to decide which side of targetElement the popup should be placed on.
        /// Default value is PlacementPreferenceOrders.TopBottomLeftRight.
        /// </param>
        /// <param name="verticalAlignmentPreferenceOrder">
        /// When popup is placed to the left/right, this preference order is used to decide how the popup should be aligned vertically. 
        /// Default value is VerticalAlignmentPreferenceOrders.TopCenterBottom.
        /// </param>
        /// <param name="horizontalAlignmentPreferenceOrder">
        /// When popup is placed to the top/bottom, this preference order is used to decide how the popup should be aligned vertically. 
        /// Default value is HorizontalAlignmentPreferenceOrders.LeftCenterRight.
        /// </param>
        /// <param name="margin"></param>
        /// <param name="isOverflowAllowed">
        /// Default value is false.. When true, TryShowNear positions the popup in the first position that matches given preference, without checking if popup will be within bounds.
        /// The popup may be outside the window or get clipped, depending on its' ShouldConstrainToRootBounds property.
        /// </param>
        /// <returns>
        /// True when popup has been positioned in any of the sides in <paramref name="placementPreferenceOrder"/>. False otherwise.
        /// When the extension cannot position the popup with given preferences, check this flag and position popup manually.
        /// </returns>
        [Obsolete("This method will be deprecated, please Use popup.TryShowNearPoint, popup.TryShowNearElement or TryShowNearRect.")]
        public static bool TryShowNear(this Popup popup,
                                       FrameworkElement targetElement,
                                       Point targetPoint = default,
                                       Side[] placementPreferenceOrder = null,
                                       Alignment[] verticalAlignmentPreferenceOrder = null,
                                       Alignment[] horizontalAlignmentPreferenceOrder = null,
                                       double margin = 10,
                                       bool isOverflowAllowed = false) //For BackwardsCompatibility
        {

            placementPreferenceOrder ??= PlacementPreferenceOrders.TopBottomLeftRight;
            horizontalAlignmentPreferenceOrder ??= HorizontalAlignmentPreferenceOrders.LeftCenterRight;
            verticalAlignmentPreferenceOrder ??= VerticalAlignmentPreferenceOrders.TopCenterBottom;

            var requestedPlacements = new List<PopupPlacementMode>();
            foreach (var side in placementPreferenceOrder)
            {
                switch (side)
                {
                    case Side.Left:
                        requestedPlacements.AddRange(verticalAlignmentPreferenceOrder.Select(alignment => alignment switch
                        {
                            Alignment.Top => PopupPlacementMode.LeftTop,
                            Alignment.Center => PopupPlacementMode.Left,
                            Alignment.Bottom => PopupPlacementMode.LeftBottom,
                            _ => PopupPlacementMode.Left
                        }));
                        break;
                    case Side.Right:
                        requestedPlacements.AddRange(verticalAlignmentPreferenceOrder.Select(alignment => alignment switch
                        {
                            Alignment.Top => PopupPlacementMode.RightTop,
                            Alignment.Center => PopupPlacementMode.Right,
                            Alignment.Bottom => PopupPlacementMode.RightBottom,
                            _ => PopupPlacementMode.Right
                        }));
                        break;
                    case Side.Top:
                        requestedPlacements.AddRange(horizontalAlignmentPreferenceOrder.Select(alignment => alignment switch
                        {
                            Alignment.Left => PopupPlacementMode.TopLeft,
                            Alignment.Center => PopupPlacementMode.Top,
                            Alignment.Right => PopupPlacementMode.TopRight,
                            _ => PopupPlacementMode.Top
                        }));
                        break;
                    case Side.Bottom:
                        requestedPlacements.AddRange(horizontalAlignmentPreferenceOrder.Select(alignment => alignment switch
                        {
                            Alignment.Left => PopupPlacementMode.BottomLeft,
                            Alignment.Center => PopupPlacementMode.Bottom,
                            Alignment.Right => PopupPlacementMode.BottomRight,
                            _ => PopupPlacementMode.Bottom
                        }));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (targetPoint != default)//if point is Passed Placement based on point takes Priority
            {
                return popup.TryShowNearPoint(targetPoint, requestedPlacements, new Thickness(margin),
                    !isOverflowAllowed);
            }

            return popup.TryShowNearElement(targetElement, requestedPlacements, new Thickness(margin), !isOverflowAllowed);
        }
        #endregion

        #region TryShowNear v2

        /// <summary>
        /// Use this method to show a popup near another Point. Note that the popup must have MaxHeight and MaxWidth properties defined.
        /// <para>Steps to use this extension:</para>
        /// <list type="number">
        /// <item>Define popUp's MaxHeight and MaxWidth properties. This is required for the extension to work as expected.</item>
        /// <item>Define the required placement preference order, You can skip this step if the default values are enough.</item>
        /// <item>Pass a  Point ,transformed With Respect to <see cref="Window.Current.Content"/> </item>
        /// <item>Call TryShowNearPoint extension and check the boolean returned by it.</item>
        /// <item>If it returns false, position popup manually or call this method with isOverflow = true.</item>
        /// </list>
        /// </summary>
        /// <param name="targetPoint">
        /// target Point Which is Transformed with To <see cref="Window.Current.Content"/>
        /// if Default is Passed popUp will be Positioned with respect to <see cref="Window.Current.Content"/>
        /// </param>
        /// <param name="preferredPlacement">
        ///  Positions PopUp With Respect to list of placements in Provided Order
        ///  positioning order will be Enumerate if Size for Positioning pop up is Not available(Enumerate will not occur if <seealso cref="shouldConstrainToWindowBounds"/> is False),Popup Will be Positioned In the First Given Preference
        ///  Default PlacementOrder <seealso cref="PopupPlacementOrders.LeftBottomRightTop"/> will be Enumerate If Custom <see cref="preferredPlacement"/> Order is not given
        /// </param>
        /// <param name="placementMargin">
        /// Adds a margin between a targeted teaching tip and its target 
        /// </param>
        /// <param name="shouldConstrainToWindowBounds">
        /// Default value is true.. When false, TryShowNear positions the popup in the first position that matches given preference, without checking if popup will be within bounds.
        /// The popup may be outside the window or get clipped, depending on its' ShouldConstrainToRootBounds property.
        /// </param>
        /// <param name="shouldConstrainToScreenBounds">
        /// Default Value is false , When Positioning PopUp Out of Current window (i.e <see cref="shouldConstrainToWindowBounds"/> is set to False), <see cref="shouldConstrainToScreenBounds"/> ensures
        /// <see cref="Popup"/> doesn't bleed Out of Current <see cref="XamlRoot"/> Where PopUp is Positioned
        /// </param>
        /// <returns>
        /// <see cref="bool"/> True when popup has been positioned in any of the sides in <paramref name="preferredPlacement"/>. False otherwise.
        /// When the extension cannot position the popup with given preferences, check this flag and position popup manually.
        /// </returns>
        public static bool TryShowNearPoint(this Popup popup, Point targetPoint,
                                            IEnumerable<PopupPlacementMode> preferredPlacement = default,
                                            Thickness placementMargin = default, bool shouldConstrainToWindowBounds = true,
                                            bool shouldConstrainToScreenBounds = false)
        {

            var popUpCoordinatesInCoreWindowSpace = popup.TransformToVisual(Window.Current.Content)
                .TransformBounds(new Rect(0, 0, popup.MaxWidth, popup.MaxHeight));

            var targetCoordinatesInCoreWindowSpace = new Rect(targetPoint.X, targetPoint.Y, 0.0, 0.0);

            return popup.TryShowNearRect(popUpCoordinatesInCoreWindowSpace, targetCoordinatesInCoreWindowSpace,
                preferredPlacement, placementMargin, shouldConstrainToWindowBounds,shouldConstrainToScreenBounds);
        }

        /// <summary>
        /// Use this method to show a popup near another FrameworkElement. Note that the popup must have MaxHeight and MaxWidth properties defined.
        /// <para>Steps to use this extension:</para>
        /// <list type="number">
        /// <item>Define popup's MaxHeight and MaxWidth properties. This is required for the extension to work as expected.</item>
        /// <item>Define the required placement preference order, You can skip this step if the default values are enough.</item>
        /// <item>Pass  a FrameworkElement Which is Already Loaded in Visual Tree Positioning is Based on Actual Width and Actual  Height of the <see cref="targetElement"/> </item>
        /// <item>Call TryShowNear extension and check the boolean returned by it.</item>
        /// <item>If it returns false, position popup manually or call this method with shouldConstrainToWindowBounds = true.</item>
        /// </list>
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="targetElement">
        ///   <see cref="FrameworkElement"/> Should Already Loaded in Visual Tree,Pop up Positioning is Based On Target's Actual Width and Actual Height ,If Target is Not Provided PopUp Will be positioned with respect to <see cref="Window.Current.Content"/> (i.e) <see cref="XamlRoot"/>
        /// </param>
        /// <param name="preferredPlacement">
        ///  Positions PopUp With Respect to list of placements in Provided Order
        ///  positioning order will be Enumerate if Size for Positioning pop up is Not available(Enumerate will not occur if <seealso cref="shouldConstrainToWindowBounds"/> is False),Popup Will be Positioned In the First Given Preference
        ///  Default PlacementOrder <seealso cref="PopupPlacementOrders.LeftBottomRightTop"/> will be Enumerate If Custom <see cref="preferredPlacement"/> Order is not given
        /// </param>
        /// <param name="placementMargin">
        /// Adds a margin between a targeted teaching tip and its target 
        /// </param>
        /// <param name="shouldConstrainToWindowBounds">
        /// Default value is true.. When false, TryShowNear positions the popup in the first position that matches given preference, without checking if popup will be within bounds.
        /// The popup may be outside the window or get clipped, depending on its' ShouldConstrainToRootBounds property.
        /// </param>
        /// <param name="shouldConstrainToScreenBounds">
        /// Default Value is false , When Positioning PopUp Out of Current window (i.e <see cref="shouldConstrainToWindowBounds"/> is set to False), <see cref="shouldConstrainToScreenBounds"/> ensures
        /// <see cref="Popup"/> doesn't bleed Out of Current <see cref="XamlRoot"/> Where PopUp is Positioned
        /// </param>
        /// <returns>
        /// <see cref="bool"/>  True when popup has been positioned in any of the sides in <paramref name="preferredPlacement"/>. False otherwise.
        /// When the extension cannot position the popup with given preferences, check this flag and position popup manually.
        /// </returns>
        public static bool TryShowNearElement(this Popup popup,
                                              FrameworkElement targetElement,
                                              IEnumerable<PopupPlacementMode> preferredPlacement = default,
                                              Thickness placementMargin = default,
                                              bool shouldConstrainToWindowBounds = true ,
                                              bool shouldConstrainToScreenBounds = false)
        {

            var popUpCoordinatesInCoreWindowSpace = popup.TransformToVisual(Window.Current.Content).TransformBounds(new Rect(0, 0, popup.MaxWidth, popup.MaxHeight));

            var targetCoordinatesInCoreWindowSpace = targetElement == null ? default : targetElement.TransformToVisual(Window.Current.Content)
                .TransformBounds(new Rect(0, 0, targetElement.ActualWidth, targetElement.ActualHeight));

            return popup.TryShowNearRect(popUpCoordinatesInCoreWindowSpace, targetCoordinatesInCoreWindowSpace, preferredPlacement, placementMargin, shouldConstrainToWindowBounds,shouldConstrainToScreenBounds);
        }

        /// <summary>
        /// Use this method to show a popup near given Rect. Popup does not need have MaxWidth and MaxHeight defined.
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="popupBoundsRelativeToWindow">
        /// Bounds of the popup with respect to current window. Unlike other overloads, these bounds must be transformed to Window origin by user.
        /// </param>
        /// <param name="targetBoundsRelativeToWindow">
        /// Bounds of the target with respect to current window. Unlike other overloads, these bounds must be transformed to Window origin by user.
        /// if Default is Passed popUp will be Positioned with respect to <see cref="Window.Current.Content"/>
        /// </param>
        /// <param name="preferredPlacements">
        ///  Order of positions in which method should try to position popup. Popup will be positioned in the first position matching other given preferences.
        ///  Default value is <seealso cref="PopupPlacementOrders.LeftBottomRightTop"/>.
        /// </param>
        /// <param name="placementMargin">
        ///  Margin to apply after positioning the popup. In most cases, this should be used only when a single preferred position is given.
        /// </param>
        /// <param name="shouldConstrainToWindowBounds">
        ///  Default value is true.. When false, method positions the popup in the first position that matches given preference, without checking if popup will be within bounds.
        ///  The popup may be outside the window or get clipped, depending on its' ShouldConstrainToRootBounds property.
        /// </param>
        /// <param name="shouldConstrainToScreenBounds">
        /// Default Value is false , When Positioning PopUp Out of Current window (i.e <see cref="shouldConstrainToWindowBounds"/> is set to False), <see cref="shouldConstrainToScreenBounds"/> ensures
        /// <see cref="Popup"/> doesn't bleed Out of Current <see cref="XamlRoot"/> Where PopUp is Positioned ,calculation For Screen Will not be considered When <see cref="shouldConstrainToWindowBounds"/> is True
        /// </param>
        /// <returns>
        ///  <see cref="bool"/>  True when popup has been positioned in any of the <see cref="PopupPlacementMode"/> in <paramref name="preferredPlacements"/>. False otherwise.
        /// When the extension cannot position the popup with given preferences, check this flag and position popup manually.
        /// </returns>
        public static bool TryShowNearRect(this Popup popup,
                                           Rect popupBoundsRelativeToWindow,
                                           Rect targetBoundsRelativeToWindow,
                                           IEnumerable<PopupPlacementMode> preferredPlacements = default,
                                           Thickness placementMargin = default,
                                           bool shouldConstrainToWindowBounds = true,
                                           bool shouldConstrainToScreenBounds = false)
        {
            GivenPlacementParams placementParams = default;
            placementParams.InitialPopupBounds = popupBoundsRelativeToWindow;
            placementParams.TargetPositionBounds = targetBoundsRelativeToWindow;
            placementParams.PlacementMargin = placementMargin;
            placementParams.ShouldConstrainToWindowBounds = shouldConstrainToWindowBounds;
            placementParams.ShouldConstrainToScreenBounds = shouldConstrainToScreenBounds;

            preferredPlacements ??= PopupPlacementOrders.LeftBottomRightTop;
            ComputedPopupOffsets computedOffset;
            foreach (var preferredPlacement in preferredPlacements)
            {
                computedOffset = targetBoundsRelativeToWindow == default
                    ? GetOffsetForXamlRootPlacement(preferredPlacement,
                        ref placementParams)
                    : GetOffsetForPreference(preferredPlacement,
                        ref placementParams); //Todo: If Target Co-ordinates is not provided try position with respect to xamlRoot
                popup.HorizontalOffset = computedOffset.HorizontalOffSet;
                popup.VerticalOffset = computedOffset.VerticalOffSet;


                if (shouldConstrainToWindowBounds && computedOffset.WillPopupFitInWindow)
                {
                    return popup.IsOpen = true;
                }
                if (shouldConstrainToWindowBounds is false)
                {
                    if (!shouldConstrainToScreenBounds || computedOffset.WillPopupFitInWindow)
                    {
                        return popup.IsOpen =true;
                    }
                }
            }
            return false;
        }




        #endregion
        #endregion

        #region Offset calculation helper methods


        /// <summary>
        /// Calculates Vertical and Horizontal Offset, With Respect to <see cref="XamlRoot"/> 
        /// </summary>
        /// <param name="preferredPlacement"></param>
        /// <param name="placementParams"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static ComputedPopupOffsets GetOffsetForXamlRootPlacement(
            PopupPlacementMode preferredPlacement, ref GivenPlacementParams placementParams)
        {
            Rect customPopUpCoordinates = placementParams.InitialPopupBounds;
            double xCord;
            double yCord;

            switch (preferredPlacement) //Finding Target Point With Respect To Current Window and Applying margin to PopUp' s Offset's Directly 
            {
                case PopupPlacementMode.Top:
                    xCord = GetOffsetForAlignment(default, Alignment.Center, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Top, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.Y -= placementParams.PlacementMargin.Top;

                    break;
                case PopupPlacementMode.TopLeft:
                    xCord = GetOffsetForAlignment(default, Alignment.Left, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Top, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.Y -= placementParams.PlacementMargin.Top;
                    customPopUpCoordinates.X -= placementParams.PlacementMargin.Left;

                    break;
                case PopupPlacementMode.TopRight:
                    xCord = GetOffsetForAlignment(default, Alignment.Right, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Top, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.Y -= placementParams.PlacementMargin.Top;
                    customPopUpCoordinates.X += placementParams.PlacementMargin.Right;

                    break;

                case PopupPlacementMode.Bottom:
                    xCord = GetOffsetForAlignment(default, Alignment.Center, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Bottom, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.Y += placementParams.PlacementMargin.Bottom;

                    break;
                case PopupPlacementMode.BottomRight:
                    xCord = GetOffsetForAlignment(default, Alignment.Right, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Bottom, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.Y += placementParams.PlacementMargin.Bottom;
                    customPopUpCoordinates.X += placementParams.PlacementMargin.Right;

                    break;
                case PopupPlacementMode.BottomLeft:
                    xCord = GetOffsetForAlignment(default, Alignment.Left, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Bottom, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.Y += placementParams.PlacementMargin.Bottom;
                    customPopUpCoordinates.X -= placementParams.PlacementMargin.Left;

                    break;
                case PopupPlacementMode.Left:
                    xCord = GetOffsetForAlignment(default, Alignment.Left, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Center, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Width);
                    customPopUpCoordinates.X -= placementParams.PlacementMargin.Left;

                    break;
                case PopupPlacementMode.LeftTop:
                    xCord = GetOffsetForAlignment(default, Alignment.Left, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Top, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.X -= placementParams.PlacementMargin.Left;
                    customPopUpCoordinates.Y -= placementParams.PlacementMargin.Top;

                    break;
                case PopupPlacementMode.LeftBottom:
                    xCord = GetOffsetForAlignment(default, Alignment.Left, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Bottom, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.X -= placementParams.PlacementMargin.Left;
                    customPopUpCoordinates.Y += placementParams.PlacementMargin.Bottom;

                    break;
                case PopupPlacementMode.Right:
                    xCord = GetOffsetForAlignment(default, Alignment.Right, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Center, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.X += placementParams.PlacementMargin.Right;

                    break;
                case PopupPlacementMode.RightTop:
                    xCord = GetOffsetForAlignment(default, Alignment.Right, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Top, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.X += placementParams.PlacementMargin.Right;
                    customPopUpCoordinates.Y -= placementParams.PlacementMargin.Top;

                    break;
                case PopupPlacementMode.RightBottom:
                    xCord = GetOffsetForAlignment(default, Alignment.Right, WindowBounds.Width,
                        placementParams.InitialPopupBounds.Width);
                    yCord = GetOffsetForAlignment(default, Alignment.Bottom, WindowBounds.Height,
                        placementParams.InitialPopupBounds.Height);
                    customPopUpCoordinates.X += placementParams.PlacementMargin.Right;
                    customPopUpCoordinates.Y += placementParams.PlacementMargin.Bottom;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(preferredPlacement), preferredPlacement, null);
            }

            var customPlacementParams = new GivenPlacementParams()
            {
                InitialPopupBounds = customPopUpCoordinates,
                PlacementMargin = default, //margin values are applied to PopUp offset already so default is passed 
                TargetPositionBounds = new Rect(xCord, yCord, default, default)
            };

            return GetOffsetForPreference(PopupPlacementMode.BottomLeft, ref customPlacementParams);//Since Target point  calculated represents TopLeft corner of PopUps new position,PopUpNeeds to Positioned BottomLeft Of the Point always

        }


        /// <summary>
        /// Calculates Horizontal Offset,Vertical Offset, And If Space for Positioning Pop is Available 
        /// </summary>
        /// <param name="requestedPlacement"></param>
        /// <param name="placementParams"></param>
        /// <returns></returns>
        private static ComputedPopupOffsets GetOffsetForPreference(PopupPlacementMode requestedPlacement, ref GivenPlacementParams placementParams)
        {
            // Attempts to position the popup in two steps.
            // Step 1: Placement (which side of targetElement/targetPoint should the popup be on?).
            // Step 2: Alignment (which edge of the popup should be aligned with the corresponding edge of the target?).

            var distanceX = placementParams.TargetPositionBounds.X - placementParams.InitialPopupBounds.X;
            var distanceY = placementParams.TargetPositionBounds.Y - placementParams.InitialPopupBounds.Y;
            var placementOffsets = new ComputedPopupOffsets();

            switch (requestedPlacement)
            {

                case PopupPlacementMode.Top:
                    placementOffsets.VerticalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Top, placementParams);
                    placementOffsets.HorizontalOffSet = GetOffsetForAlignment(distanceX, Alignment.Center, placementParams.TargetPositionBounds.Width, placementParams.InitialPopupBounds.Width);
                    break;
                case PopupPlacementMode.TopLeft:
                    placementOffsets.VerticalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Top, placementParams);
                    placementOffsets.HorizontalOffSet = GetOffsetForAlignment(distanceX, Alignment.Left, placementParams.TargetPositionBounds.Width, placementParams.InitialPopupBounds.Width);
                    break;
                case PopupPlacementMode.TopRight:
                    placementOffsets.VerticalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Top, placementParams);
                    placementOffsets.HorizontalOffSet = GetOffsetForAlignment(distanceX, Alignment.Right, placementParams.TargetPositionBounds.Width, placementParams.InitialPopupBounds.Width);
                    break;
                case PopupPlacementMode.Bottom:
                    placementOffsets.VerticalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Bottom, placementParams);
                    placementOffsets.HorizontalOffSet = GetOffsetForAlignment(distanceX, Alignment.Center, placementParams.TargetPositionBounds.Width, placementParams.InitialPopupBounds.Width);
                    break;
                case PopupPlacementMode.BottomRight:
                    placementOffsets.VerticalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Bottom, placementParams);
                    placementOffsets.HorizontalOffSet = GetOffsetForAlignment(distanceX, Alignment.Right, placementParams.TargetPositionBounds.Width, placementParams.InitialPopupBounds.Width);
                    break;
                case PopupPlacementMode.BottomLeft:
                    placementOffsets.VerticalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Bottom, placementParams);
                    placementOffsets.HorizontalOffSet = GetOffsetForAlignment(distanceX, Alignment.Left, placementParams.TargetPositionBounds.Width, placementParams.InitialPopupBounds.Width);
                    break;
                case PopupPlacementMode.Left:
                    placementOffsets.HorizontalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Left, placementParams);
                    placementOffsets.VerticalOffSet = GetOffsetForAlignment(distanceY, Alignment.Center, placementParams.TargetPositionBounds.Height, placementParams.InitialPopupBounds.Height);
                    break;
                case PopupPlacementMode.LeftTop:
                    placementOffsets.HorizontalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Left, placementParams);
                    placementOffsets.VerticalOffSet = GetOffsetForAlignment(distanceY, Alignment.Top, placementParams.TargetPositionBounds.Height, placementParams.InitialPopupBounds.Height);
                    break;
                case PopupPlacementMode.LeftBottom:
                    placementOffsets.HorizontalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Left, placementParams);
                    placementOffsets.VerticalOffSet = GetOffsetForAlignment(distanceY, Alignment.Bottom, placementParams.TargetPositionBounds.Height, placementParams.InitialPopupBounds.Height);
                    break;
                case PopupPlacementMode.Right:
                    placementOffsets.HorizontalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Right, placementParams);
                    placementOffsets.VerticalOffSet = GetOffsetForAlignment(distanceY, Alignment.Center, placementParams.TargetPositionBounds.Height, placementParams.InitialPopupBounds.Height);
                    break;
                case PopupPlacementMode.RightTop:
                    placementOffsets.HorizontalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Right, placementParams);
                    placementOffsets.VerticalOffSet = GetOffsetForAlignment(distanceY, Alignment.Top, placementParams.TargetPositionBounds.Height, placementParams.InitialPopupBounds.Height);
                    break;
                case PopupPlacementMode.RightBottom:
                    placementOffsets.HorizontalOffSet = GetOffsetForPlacement(distanceX, distanceY, Side.Right, placementParams);
                    placementOffsets.VerticalOffSet = GetOffsetForAlignment(distanceY, Alignment.Bottom, placementParams.TargetPositionBounds.Height, placementParams.InitialPopupBounds.Height);
                    break;
            }

            CheckIfWithinWindowBounds(ref placementOffsets, ref placementParams, requestedPlacement, distanceX, distanceY);
            if (placementParams is { ShouldConstrainToWindowBounds: false, ShouldConstrainToScreenBounds: true })//Calculation for Whether popUp will Place outside of Screen is only done when placement Outside of the application is allowed
            {
                CheckIfWithinScreenBounds(ref placementOffsets, ref placementParams, requestedPlacement, distanceX, distanceY);
            }
            return placementOffsets;
        }


        /// <summary>
        /// Checks if Pop Will Position inside current Screen
        /// </summary>
        private static void CheckIfWithinScreenBounds(ref ComputedPopupOffsets placementOffset,
                                                      ref GivenPlacementParams placementParams,
                                                      PopupPlacementMode preferredPlacement, double distanceX,
                                                      double distanceY)
        {

            var taskBarPadding =ScreenUtil.GetTaskBarPadding();
            var displayInfo = DisplayInformation.GetForCurrentView();
            var windowBounds = WindowBounds;
            var screenWidth = displayInfo.ScreenWidthInRawPixels / displayInfo.RawPixelsPerViewPixel;
            var screenHeight = displayInfo.ScreenHeightInRawPixels / displayInfo.RawPixelsPerViewPixel;
            bool hasHorizontalSpace = default;
            bool hasVerticalSpace = default;


            var spaceAroundTarget = new Thickness();
            //Screen space OnLeft Side Of Target
            spaceAroundTarget.Left = windowBounds.Left + placementParams.TargetPositionBounds.X - taskBarPadding.Left;
            //Screen space OnRight Side of Target
            spaceAroundTarget.Right = screenWidth - (spaceAroundTarget.Left + placementParams.TargetPositionBounds.Width) - taskBarPadding.Right;
            //Screen Space On Top Of The Target
            spaceAroundTarget.Top = windowBounds.Top + placementParams.TargetPositionBounds.Y - taskBarPadding.Top;
            //Screen Space below  The Target
            spaceAroundTarget.Bottom = screenHeight - (spaceAroundTarget.Top + placementParams.TargetPositionBounds.Height) - taskBarPadding.Bottom;


            switch (preferredPlacement)
            {
                case PopupPlacementMode.Top:
                    hasVerticalSpace = spaceAroundTarget.Top >= placementParams.InitialPopupBounds.Height;
                    hasHorizontalSpace = WindowBounds.X + placementParams.InitialPopupBounds.X + placementOffset.HorizontalOffSet +
                                         placementParams.InitialPopupBounds.Width <= screenWidth - taskBarPadding.Right //ignore Task Bar Width Form Total screen Width
                                         && WindowBounds.X + placementParams.InitialPopupBounds.X + placementOffset.HorizontalOffSet >= 0;
                    break;
                case PopupPlacementMode.TopLeft:
                    hasVerticalSpace = spaceAroundTarget.Top >= placementParams.InitialPopupBounds.Height;
                    hasHorizontalSpace = WindowBounds.X + placementParams.InitialPopupBounds.X +
                        placementOffset.HorizontalOffSet + placementParams.InitialPopupBounds.Width <= screenWidth - taskBarPadding.Right; //ignore Task Bar Width Form Total screen Width;
                    break;
                case PopupPlacementMode.TopRight:
                    hasVerticalSpace = spaceAroundTarget.Top >= placementParams.InitialPopupBounds.Height;
                    hasHorizontalSpace = WindowBounds.X + placementParams.InitialPopupBounds.X + placementOffset.HorizontalOffSet >= 0;
                    break;
                case PopupPlacementMode.Bottom:
                    hasVerticalSpace = spaceAroundTarget.Bottom >= placementParams.InitialPopupBounds.Height;
                    hasHorizontalSpace = WindowBounds.X + placementParams.InitialPopupBounds.X + placementOffset.HorizontalOffSet +
                                         placementParams.InitialPopupBounds.Width <= screenWidth - taskBarPadding.Right //ignore Task Bar Width Form Total screen Width
                                         && WindowBounds.X + placementParams.InitialPopupBounds.X + placementOffset.HorizontalOffSet >= 0;
                    break;
                case PopupPlacementMode.BottomRight:
                    hasVerticalSpace = spaceAroundTarget.Bottom >= placementParams.InitialPopupBounds.Height;
                    hasHorizontalSpace = WindowBounds.X + placementParams.InitialPopupBounds.X + placementOffset.HorizontalOffSet >= 0;

                    break;
                case PopupPlacementMode.BottomLeft:
                    hasVerticalSpace = spaceAroundTarget.Bottom >= placementParams.InitialPopupBounds.Height;
                    hasHorizontalSpace = WindowBounds.X + placementParams.InitialPopupBounds.X +
                        placementOffset.HorizontalOffSet + placementParams.InitialPopupBounds.Width <= screenWidth - taskBarPadding.Right;//ignore Task Bar Width Form Total screen Width;
                    break;
                case PopupPlacementMode.Left:
                    hasHorizontalSpace = spaceAroundTarget.Left >= placementParams.InitialPopupBounds.Width;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + placementOffset.VerticalOffSet + WindowBounds.Y >= 0 &&
                                       placementParams.InitialPopupBounds.Y + placementOffset.VerticalOffSet + WindowBounds.Y +
                                       placementParams.InitialPopupBounds.Height <= screenHeight - taskBarPadding.Bottom;
                    break;
                case PopupPlacementMode.LeftTop:
                    hasHorizontalSpace = spaceAroundTarget.Left >= placementParams.InitialPopupBounds.Width;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + placementOffset.VerticalOffSet + WindowBounds.Y +
                        placementParams.InitialPopupBounds.Height <= screenHeight - taskBarPadding.Bottom;
                    break;
                case PopupPlacementMode.LeftBottom:
                    hasHorizontalSpace = spaceAroundTarget.Left >= placementParams.InitialPopupBounds.Width;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + placementOffset.VerticalOffSet + WindowBounds.Y >= 0 &&
                                       placementParams.InitialPopupBounds.Y + placementOffset.VerticalOffSet + WindowBounds.Y +
                                       placementParams.InitialPopupBounds.Height <= screenHeight - taskBarPadding.Bottom;
                    break;
                case PopupPlacementMode.Right:
                    hasHorizontalSpace = spaceAroundTarget.Right >= placementParams.InitialPopupBounds.Width;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + placementOffset.VerticalOffSet + WindowBounds.Y >= 0 &&
                                       placementParams.InitialPopupBounds.Y + placementOffset.VerticalOffSet + WindowBounds.Y +
                                       placementParams.InitialPopupBounds.Height <= screenHeight - taskBarPadding.Bottom;
                    break;
                case PopupPlacementMode.RightTop:
                    hasHorizontalSpace = spaceAroundTarget.Right >= placementParams.InitialPopupBounds.Width;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + placementOffset.VerticalOffSet + WindowBounds.Y +
                        placementParams.InitialPopupBounds.Height <= screenHeight - taskBarPadding.Bottom;
                    break;
                case PopupPlacementMode.RightBottom:
                    hasHorizontalSpace = spaceAroundTarget.Right >= placementParams.InitialPopupBounds.Width;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + placementOffset.VerticalOffSet + WindowBounds.Y >= 0 &&
                                       placementParams.InitialPopupBounds.Y + placementOffset.VerticalOffSet + WindowBounds.Y +
                                       placementParams.InitialPopupBounds.Height <= screenHeight - taskBarPadding.Bottom;//ignore Task Bar Height Form Total screen height
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(preferredPlacement), preferredPlacement, null);
            }

            $"{preferredPlacement} placement has Vertical Space To Display {hasVerticalSpace} || has HorizontalSpace To Display {hasHorizontalSpace} ".Print();

            placementOffset.WillPopupFitInWindow = hasHorizontalSpace && hasVerticalSpace;
        }



        private static void CheckIfWithinWindowBounds(ref ComputedPopupOffsets placementOffset, ref GivenPlacementParams placementParams, PopupPlacementMode preferredPlacement, double distanceX, double distanceY)
        {
            bool hasVerticalSpace = default;
            bool hasHorizontalSpace = default;
            var verticalOffset = placementOffset.VerticalOffSet;
            var horizontalOffset = placementOffset.HorizontalOffSet;

            switch (preferredPlacement)
            {

                case PopupPlacementMode.Top:
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + verticalOffset >= 0;
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + horizontalOffset >= 0 && placementParams.InitialPopupBounds.X + horizontalOffset + placementParams.InitialPopupBounds.Width <= WindowBounds.Width;
                    break;
                case PopupPlacementMode.TopLeft:
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + verticalOffset >= 0;
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + distanceX + placementParams.InitialPopupBounds.Width <= WindowBounds.Width;
                    break;
                case PopupPlacementMode.TopRight:
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + verticalOffset >= 0;
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + horizontalOffset >= 0;
                    break;
                case PopupPlacementMode.Bottom:
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + verticalOffset + placementParams.InitialPopupBounds.Height <= WindowBounds.Height;
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + horizontalOffset >= 0 && placementParams.InitialPopupBounds.X + horizontalOffset + placementParams.InitialPopupBounds.Width <= WindowBounds.Width;
                    break;
                case PopupPlacementMode.BottomRight:
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + verticalOffset + placementParams.InitialPopupBounds.Height <= WindowBounds.Height;
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + horizontalOffset >= 0;
                    break;
                case PopupPlacementMode.BottomLeft:
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + verticalOffset + placementParams.InitialPopupBounds.Height <= WindowBounds.Height;
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + distanceX + placementParams.InitialPopupBounds.Width <= WindowBounds.Width;
                    break;
                case PopupPlacementMode.Left:
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + horizontalOffset >= 0;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + verticalOffset >= 0 && placementParams.InitialPopupBounds.Y + verticalOffset + placementParams.InitialPopupBounds.Height <= WindowBounds.Height;
                    break;
                case PopupPlacementMode.LeftTop:
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + horizontalOffset >= 0;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + distanceY + placementParams.InitialPopupBounds.Height <= WindowBounds.Height;
                    break;
                case PopupPlacementMode.LeftBottom:
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + horizontalOffset >= 0;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + distanceY - placementParams.InitialPopupBounds.Height >= 0 && placementParams.InitialPopupBounds.Y + verticalOffset + placementParams.InitialPopupBounds.Height <= WindowBounds.Height;
                    break;
                case PopupPlacementMode.Right:
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + horizontalOffset + placementParams.InitialPopupBounds.Width <= WindowBounds.Width;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + verticalOffset >= 0 && placementParams.InitialPopupBounds.Y + verticalOffset + placementParams.InitialPopupBounds.Height <= WindowBounds.Height;
                    break;
                case PopupPlacementMode.RightTop:
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + horizontalOffset + placementParams.InitialPopupBounds.Width <= WindowBounds.Width;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + distanceY + placementParams.InitialPopupBounds.Height <= WindowBounds.Height;
                    break;
                case PopupPlacementMode.RightBottom:
                    hasHorizontalSpace = placementParams.InitialPopupBounds.X + horizontalOffset + placementParams.InitialPopupBounds.Width <= WindowBounds.Width;
                    hasVerticalSpace = placementParams.InitialPopupBounds.Y + distanceY - placementParams.InitialPopupBounds.Height >= 0 && placementParams.InitialPopupBounds.Y + verticalOffset + placementParams.InitialPopupBounds.Height <= WindowBounds.Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(preferredPlacement), preferredPlacement, "Unknown popup position.");
            }

            placementOffset.WillPopupFitInWindow = hasHorizontalSpace && hasVerticalSpace;
        }

        /// <summary>
        /// This method calculates the offset needed to place the popup on given side of a target element.
        /// </summary>
        /// <param name="distanceX"></param>
        /// <param name="distanceY"></param>
        /// <param name="side"></param>
        /// <param name="placementParams"></param>
        /// <returns></returns>
        private static double GetOffsetForPlacement(double distanceX, double distanceY, Side side, GivenPlacementParams placementParams)
        {
            return side switch
            {
                Side.Left => distanceX - placementParams.InitialPopupBounds.Width - placementParams.PlacementMargin.Left,

                Side.Right => distanceX + placementParams.TargetPositionBounds.Width + placementParams.PlacementMargin.Right,

                Side.Top => distanceY - placementParams.InitialPopupBounds.Height - placementParams.PlacementMargin.Top,

                Side.Bottom => distanceY + placementParams.TargetPositionBounds.Height + placementParams.PlacementMargin.Bottom,

                _ => default
            };
        }

        /// <summary>
        /// This method calculates the offset needed to align a popup with a target element.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="alignment"></param>
        /// <param name="targetElementDimension"></param>
        /// <param name="popupDimension"></param>
        /// <returns></returns>
        private static double GetOffsetForAlignment(double distance, Alignment alignment, double targetElementDimension, double popupDimension)
        {
            return alignment switch
            {
                Alignment.Center => distance - (popupDimension - targetElementDimension) / 2,

                Alignment.Right => distance - popupDimension + targetElementDimension,

                Alignment.Left => distance,

                Alignment.Top => distance,

                Alignment.Bottom => distance - popupDimension + targetElementDimension,
                _ => default
            };
        }
        #endregion
    }
}

public static class ScreenUtil
{

    /// <summary>
    ///Default TaskBar position is Bottom and Task Bar height is 47 in 100% scaling in Win11 ,Cannot Change TaskBar Orientation unless User performs Registry Editor manipulation
    /// </summary>
    public const double DefaultTaskBarHeightWin11 = 47.0;

    public const double DefaultTaskBarHeightWin10 = 47.0;

    /// <summary>
    /// In Win 10 Task can can be positioned on Right /Left in that case default Task Bar Width is 90 in 100% Scaling
    /// </summary>
    public const double DefaultTaskBarWidthWin10 = 90;

    /// <summary>
    /// Calculates TaskBar Thickness  with Respect To Windows Version 
    /// </summary>
    /// <returns></returns>
    public static Thickness GetTaskBarPadding()
    {

        var scalingFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

        if (Environment.OSVersion.Version.Build >= 22000) // 22000 is the build number for Windows 11
        {

            var taskBarThickness = new Thickness(default, default, default, DefaultTaskBarHeightWin11 * scalingFactor);

            return taskBarThickness;
        }
        else  // The app is running on Windows 10 or an older version
        {

            var taskBarThickness = new Thickness(DefaultTaskBarWidthWin10 * scalingFactor,
                DefaultTaskBarHeightWin10 * scalingFactor,
                DefaultTaskBarWidthWin10 * scalingFactor,
                DefaultTaskBarHeightWin10 * scalingFactor);
            return taskBarThickness;

        }

    }
}
