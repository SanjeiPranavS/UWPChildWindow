using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace ZTeachingTip
{

    namespace Zoho.UWP.Common.Extensions
    {
        #region Supporting Enums and Constants
        public enum Side { Left, Top, Right, Bottom };

        // Defines which edge of the given element has to be aligned with the corresponding edge of the targetElement.
        // For example, when Alignment is Left, the left edge of given element is aligned with left edge of targetElement.
        public enum Alignment { Left, Top, Center, Right, Bottom };
        // TO DO: Can we split Alignment into VerticalAlignment and HorizontalAlignment enums with single implementation for GetOffsetForAlignment?

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
        #endregion

        #region Steps to Use TryShowNear

        #endregion

        public static class PopupExtension
        {
            /// <summary>
            /// Use this method to show a popup near another FrameworkElement. Note that the popup must have MaxHeight and MaxWidth properties defined.
            /// <para>Steps to use this extension:</para>
            /// <list type="number">
            /// <item>Define popup's MaxHeight and MaxWidth properties. This is required for the extension to work as expected.</item>
            /// <item>Define the required placement preference order, horizontal alignment order and vertical alignment order. You can skip this step if the default values are enough.</item>
            /// <item>Pass either a FrameworkElement or a Point to this method. Atleast one of the two are required. When both are given, the Point is targeted. </item>
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
            /// Default value is VerticalAlignmentPreferenceOrders.TopCenterBottom.
            /// </param>
            /// <param name="margin"></param>
            /// <param name="isOverflowAllowed">
            /// Defines if the popup can overflow window bounds. 
            /// When true, TryShowNear positions the popup in the first position that matches given preference, without checking if popup will be within bounds.
            /// Some of our components don't work as expected when shown outside the window. It is recommended to set this as false for now.
            /// </param>
            /// <returns>
            /// True when popup has been positioned in any of the sides in <paramref name="placementPreferenceOrder"/>. False otherwise.
            /// When the extension cannot position the popup with given preferences, check this flag and position popup manually.
            /// </returns>
            public static bool TryShowNear(this Popup popup,
                                           FrameworkElement targetElement,
                                           Point targetPoint = default,
                                           Side[] placementPreferenceOrder = null,
                                           Alignment[] verticalAlignmentPreferenceOrder = null, // Default: VerticalAlignmentPreferenceOrders.TopCenterBottom
                                           Alignment[] horizontalAlignmentPreferenceOrder = null, // Default: HorizontalAlignmentPreferenceOrders.LeftCenterBottom
                                           double margin = 10,
                                           bool isOverflowAllowed = false)
            {
                try
                {
                    // TO DO: Instead of checking only the given alignments (eg: Left, Center, Right), calculate by how much we have to shift to fit popup.
                    // TO DO: Require MaxHeight and MaxWidth only when ActualHeight and ActualWidth aren't available- ie when the control isn't added to visual tree yet.

                    // Check if targetElement (a Framework element) or targetPoint (a Point) should be targeted.
                    Point targetElementCoords;
                    double targetActualHeight = 0f;
                    double targetActualWidth = 0f;


                    if (targetPoint != default)
                    {
                        // Store targetPoint coordinates.
                        targetElementCoords = targetPoint;
                    }
                    else if (targetElement != null)
                    { 
                        // Store targetElement coordinates and dimensions.
                        var targetElementTransform = targetElement.TransformToVisual(Window.Current.Content);
                        targetElementCoords = targetElementTransform.TransformPoint(new Point(0, 0));
                        targetActualHeight = targetElement.ActualHeight;
                        targetActualWidth = targetElement.ActualWidth;
                    }
                    else
                    {
                        // No targetPoint or targetElement was passed.
                        return false;
                    }

                    // Set default placement preferences if nothing is passed.
                    if (placementPreferenceOrder == null)
                    {
                        placementPreferenceOrder = PlacementPreferenceOrders.TopBottomLeftRight;
                    }

                    // Set default horizontal and vertical alignment preferences if nothing is passed.
                    if (verticalAlignmentPreferenceOrder == null)
                    {
                        verticalAlignmentPreferenceOrder = VerticalAlignmentPreferenceOrders.TopCenterBottom;
                    }
                    if (horizontalAlignmentPreferenceOrder == null)
                    {
                        horizontalAlignmentPreferenceOrder = HorizontalAlignmentPreferenceOrders.LeftCenterRight;
                    }

                    // Tranform (0, 0) of popup and target element with respect to root page.
                    var popupTransform = popup.TransformToVisual(Window.Current.Content);
                    Point popupCoords = popupTransform.TransformPoint(new Point(0, 0));


                    // Calculate horizontal and vertical distance between popup and target element.
                    double distanceX = targetElementCoords.X - popupCoords.X; // distanceX > 0 : popup is to left of target element
                    double distanceY = targetElementCoords.Y - popupCoords.Y; // distanceY > 0 : popup is above target element

                    Rect windowBounds = Window.Current.Bounds;
                    double? verticalOffsetForAlignment = null;
                    double? horizontalOffsetForAlignment = null;

                    foreach (var side in placementPreferenceOrder)
                    {
                        switch (side)
                        {
                            case Side.Left:
                                // Calculates the horizontal offset needed to position the popup completely to the left side of the target element.
                                var horizontalOffset = distanceX - popup.MaxWidth - margin;
                                // If the calculated offset positions popup within window boundaries, set the horizontal offset and align popup vertically.
                                if (isOverflowAllowed || popupCoords.X + horizontalOffset >= 0)
                                {
                                    popup.HorizontalOffset = horizontalOffset;
                                    if (!verticalOffsetForAlignment.HasValue)
                                    {
                                        verticalOffsetForAlignment = GetOffsetForAlignment(popupCoords.Y,
                                                                                           targetElementCoords.Y,
                                                                                           distanceY,
                                                                                           popup.MaxHeight,
                                                                                           targetActualHeight,
                                                                                           windowBounds.Height,
                                                                                           verticalAlignmentPreferenceOrder,
                                                                                           isOverflowAllowed);
                                        if (verticalOffsetForAlignment == null)
                                        {
                                            return false;
                                        }
                                    }
                                    popup.VerticalOffset = verticalOffsetForAlignment.Value;
                                    popup.IsOpen = true;
                                    return true;
                                }
                                continue;

                            case Side.Right:
                                horizontalOffset = distanceX + targetActualWidth + margin;
                                if (isOverflowAllowed || popupCoords.X + horizontalOffset + popup.MaxWidth <= windowBounds.Width)
                                {
                                    popup.HorizontalOffset = horizontalOffset;
                                    if (!verticalOffsetForAlignment.HasValue)
                                    {
                                        verticalOffsetForAlignment = GetOffsetForAlignment(popupCoords.Y,
                                                                                           targetElementCoords.Y,
                                                                                           distanceY,
                                                                                           popup.MaxHeight,
                                                                                           targetActualHeight,
                                                                                           windowBounds.Height,
                                                                                           verticalAlignmentPreferenceOrder,
                                                                                           isOverflowAllowed);
                                        if (verticalOffsetForAlignment == null)
                                        {
                                            return false;
                                        }
                                    }
                                    popup.VerticalOffset = verticalOffsetForAlignment.Value;
                                    popup.IsOpen = true;
                                    return true;
                                }
                                continue;

                            case Side.Top:
                                // Calculates the vertical offset needed to position the popup completely above of the target element.
                                var verticalOffset = distanceY - popup.MaxHeight - margin;
                                // If the calculated offset positions popup within window boundaries, set the vertical offset and align popup horizontally.
                                if (isOverflowAllowed || popupCoords.Y + verticalOffset >= 0)
                                {
                                    popup.VerticalOffset = verticalOffset;
                                    if (!horizontalOffsetForAlignment.HasValue)
                                    {
                                        horizontalOffsetForAlignment = GetOffsetForAlignment(popupCoords.X,
                                                                                             targetElementCoords.X,
                                                                                             distanceX,
                                                                                             popup.MaxWidth,
                                                                                             targetActualWidth,
                                                                                             windowBounds.Width,
                                                                                             horizontalAlignmentPreferenceOrder,
                                                                                             isOverflowAllowed);
                                        if (horizontalOffsetForAlignment == null)
                                        {
                                            return false;
                                        }
                                    }
                                    popup.HorizontalOffset = horizontalOffsetForAlignment.Value;
                                    popup.IsOpen = true;
                                    return true;
                                }
                                continue;

                            case Side.Bottom:
                                verticalOffset = distanceY + targetActualHeight + margin;
                                if (isOverflowAllowed || popupCoords.Y + verticalOffset + popup.MaxHeight <= windowBounds.Height)
                                {
                                    popup.VerticalOffset = verticalOffset;
                                    if (!horizontalOffsetForAlignment.HasValue)
                                    {
                                        horizontalOffsetForAlignment = GetOffsetForAlignment(popupCoords.X,
                                                                                             targetElementCoords.X,
                                                                                             distanceX,
                                                                                             popup.MaxWidth,
                                                                                             targetActualWidth,
                                                                                             windowBounds.Width,
                                                                                             horizontalAlignmentPreferenceOrder,
                                                                                             isOverflowAllowed);
                                        if (horizontalOffsetForAlignment == null)
                                        {
                                            return false;
                                        }
                                    }
                                    popup.HorizontalOffset = horizontalOffsetForAlignment.Value;
                                    popup.IsOpen = true;
                                    return true;
                                }
                                continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                return false;
            }

            /// <summary>
            /// Use this method to show a popup near a Point. Note that the popup must have MaxHeight and MaxWidth properties defined.
            /// Refer TryShowNear method above for usage instructions.
            /// </summary>
            public static bool TryShowNear(this Popup popup,
                                           Point targetPoint,
                                           Side[] placementPreferenceOrder = null,
                                           Alignment[] verticalAlignmentPreferenceOrder = null, // Default: VerticalAlignmentPreferenceOrders.TopCenterBottom
                                           Alignment[] horizontalAlignmentPreferenceOrder = null, // Default: HorizontalAlignmentPreferenceOrders.LeftCenterBottom
                                           double margin = 10,
                                           bool isOverflowAllowed = false)
            {
                return popup.TryShowNear(null, targetPoint, placementPreferenceOrder, verticalAlignmentPreferenceOrder, horizontalAlignmentPreferenceOrder, margin, isOverflowAllowed);
            }

            /// <summary>
            /// This method calculates the offset needed to align a popup with a target element.
            /// </summary>
            /// <param name="popupCoord"></param>
            /// <param name="targetElementCoord"></param>
            /// <param name="distance"></param>
            /// <param name="popupMaxDimension"></param>
            /// <param name="targetElementDimension"></param>
            /// <param name="windowDimension"></param>
            /// <returns></returns>
            private static double? GetOffsetForAlignment(double popupCoord,
                                                         double targetElementCoord,
                                                         double distance,
                                                         double popupMaxDimension,
                                                         double targetElementDimension,
                                                         double windowDimension,
                                                         Alignment[] alignmentPreferenceOrder,
                                                         bool isOverflowAllowed = false)
            {
                foreach (var alignment in alignmentPreferenceOrder)
                {
                    switch (alignment)
                    {
                        case (Alignment.Left):
                        case (Alignment.Top):
                            // Check if aligning left/top causes overflow. Assumes left/top of targetElement is within window bounds.
                            if (isOverflowAllowed || popupCoord + distance + popupMaxDimension <= windowDimension)
                            {
                                return distance;
                            }
                            break;
                        case (Alignment.Center):
                            // Calculates the offset needed to align the center of the popup with the center of the target element.
                            double offsetForCenterAlignment = distance - (popupMaxDimension - targetElementDimension) / 2;

                            // If the calculated offset positions the popup within the window boundaries, align the center of the popup with the center of the target element.
                            if (isOverflowAllowed ||
                               (popupCoord + offsetForCenterAlignment >= 0 && popupCoord + offsetForCenterAlignment + popupMaxDimension <= windowDimension))
                            {
                                return offsetForCenterAlignment;
                            }
                            break;
                        case (Alignment.Right):
                        case (Alignment.Bottom):
                            if (isOverflowAllowed || popupCoord + distance - popupMaxDimension >= 0)
                            {
                                return distance - popupMaxDimension + targetElementDimension;
                            }
                            break;
                    }
                }
                return null;
            }
        }
    }

}
