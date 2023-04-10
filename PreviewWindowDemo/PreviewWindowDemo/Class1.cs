using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

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

    public static class PopExtension
    {

        #region Properties&Feilds

        private static Thickness PlacementMargin { get; set; }
        private static Thickness PlacementOffset { get; set; }

        private static Rect PopUpCoordinatesInCoreWindowSpace { get; set; }

        private static Rect TargetCoordinatesInCoreWindowSpace { get; set; }

        private static Rect WindowBounds
        {
            get => Window.Current.Bounds;
        }

        #endregion

        #region InnerClassAndEnums
        private class PopUpOffset
        {
            public double HorizontalOffSet { get; set; }

            public double VerticalOffSet { get; set; }

            public bool IsFittingWithinBounds { get; set; }
        }

        #endregion


        #region TryShowNearOverLoads

        /// <summary>
        /// Use this method to show a popup near a Point. Note that the popup must have MaxHeight and MaxWidth properties defined.
        /// Refer TryShowNear method above for usage instructions.
        /// </summary>
        public static bool TryShowNear(this Popup popup, Point targetPoint, Side[] placementPreferenceOrder = null, Alignment[] verticalAlignmentPreferenceOrder = null, Alignment[] horizontalAlignmentPreferenceOrder = null, double margin = 10, bool isOverflowAllowed = false) //For BackWards Compatibility
        {
            return popup.TryShowNear(null, targetPoint, placementPreferenceOrder, verticalAlignmentPreferenceOrder, horizontalAlignmentPreferenceOrder, margin, isOverflowAllowed);
        }
        /// <summary>
        /// Use this method to show a popup near another FrameworkElement or a Point. Note that the popup must have MaxHeight and MaxWidth properties defined.
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
        /// Default value is PlacementPreferenceOrders.Left.
        /// </param>
        /// <param name="verticalAlignmentPreferenceOrder">
        /// When popup is placed to the left/right, this preference order is used to decide how the popup should be aligned vertically. 
        /// Default value is VerticalAlignmentPreferenceOrders.Center.
        /// </param>
        /// <param name="horizontalAlignmentPreferenceOrder">
        /// When popup is placed to the top/bottom, this preference order is used to decide how the popup should be aligned vertically. 
        /// Default value is VerticalAlignmentPreferenceOrders.Center.
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
        public static bool TryShowNear(this Popup popup, FrameworkElement targetElement, Point targetPoint = default, Side[] placementPreferenceOrder = null, Alignment[] verticalAlignmentPreferenceOrder = null, Alignment[] horizontalAlignmentPreferenceOrder = null, double margin = 10, bool isOverflowAllowed = false) //For BackwardsCompatibility
        {
            var requestedPlacements = new List<PopUpPlacement>();

            placementPreferenceOrder ??= new[]
            {
                    Side.Left
            };
            horizontalAlignmentPreferenceOrder ??= new[]
            {
                    Alignment.Center
            };
            verticalAlignmentPreferenceOrder ??= new[]
            {
                    Alignment.Center
            };

            foreach (var side in placementPreferenceOrder)
            {
                switch (side)
                {
                    case Side.Left:
                        requestedPlacements.AddRange(verticalAlignmentPreferenceOrder.Select(alignment => alignment switch
                        {
                            Alignment.Top => PopUpPlacement.LeftTop,
                            Alignment.Center => PopUpPlacement.Left,
                            Alignment.Bottom => PopUpPlacement.LeftBottom,
                            _ => PopUpPlacement.Left
                        }));
                        break;
                    case Side.Right:
                        requestedPlacements.AddRange(verticalAlignmentPreferenceOrder.Select(alignment => alignment switch
                        {
                            Alignment.Top => PopUpPlacement.RightTop,
                            Alignment.Center => PopUpPlacement.Right,
                            Alignment.Bottom => PopUpPlacement.RightBottom,
                            _ => PopUpPlacement.Right
                        }));
                        break;
                    case Side.Top:
                        requestedPlacements.AddRange(horizontalAlignmentPreferenceOrder.Select(alignment => alignment switch
                        {
                            Alignment.Left => PopUpPlacement.TopLeft,
                            Alignment.Center => PopUpPlacement.Top,
                            Alignment.Right => PopUpPlacement.TopRight,
                            _ => PopUpPlacement.Top
                        }));
                        break;
                    case Side.Bottom:
                        requestedPlacements.AddRange(horizontalAlignmentPreferenceOrder.Select(alignment => alignment switch
                        {
                            Alignment.Left => PopUpPlacement.BottomLeft,
                            Alignment.Center => PopUpPlacement.Bottom,
                            Alignment.Right => PopUpPlacement.BottomRight,
                            _ => PopUpPlacement.Bottom
                        }));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return popup.TryShowNear(targetElement, targetPoint, requestedPlacements, new Thickness(margin),default ,!isOverflowAllowed);
        }
        public static bool TryShowNear(this Popup popup, Point targetPoint, IEnumerable<PopUpPlacement> preferredPlacement = default, Thickness placementMargin = default, Thickness placementOffset = default, bool shouldBoundToXamlRoot = true)
        {
            return targetPoint != default && popup.TryShowNear(null, targetPoint, preferredPlacement,placementMargin ,placementOffset, shouldBoundToXamlRoot);
        }
        public static bool TryShowNear(this Popup popup, FrameworkElement targetElement, Point targetPoint = default, IEnumerable<PopUpPlacement> preferredPlacement = default, Thickness placementMargin = default, Thickness placementOffset = default, bool shouldBoundToXamlRoot = true)
        {
            if (targetElement is null && targetPoint == default)
            {
                return false;
            }
            var popUpCoordinatesInCoreWindowSpace = popup.TransformToVisual(Window.Current.Content).TransformBounds(new Rect(0, 0, popup.MaxWidth, popup.MaxHeight));


            var targetCoordinatesInCoreWindowSpace = targetElement != default ? targetElement.TransformToVisual(Window.Current.Content).TransformBounds(new Rect(0, 0, targetElement.ActualWidth, targetElement.ActualHeight))
                                                                         : new Rect(targetPoint.X, targetPoint.Y, 0.0, 0.0);

            return popup.TryShowNear(popUpCoordinatesInCoreWindowSpace, targetCoordinatesInCoreWindowSpace, preferredPlacement,placementMargin ,placementOffset, shouldBoundToXamlRoot);
        }
        public static bool TryShowNear(this Popup popup, Rect popCoordinatesInCoreWindowSpace, Rect targetCoordinatesInCoreWindowSpace, IEnumerable<PopUpPlacement> preferredPlacements = default,Thickness placementMargin = default,Thickness placementOffset = default, bool shouldBoundToXamlRoot = true)
        {

            PopUpCoordinatesInCoreWindowSpace = popCoordinatesInCoreWindowSpace;

            TargetCoordinatesInCoreWindowSpace = targetCoordinatesInCoreWindowSpace;

            PlacementOffset = placementOffset;

            PlacementMargin = placementMargin;

            preferredPlacements ??= new[]
            {
                    PopUpPlacement.Left
            };

            foreach (var preferredPlacement in preferredPlacements)
            {

                var requestedOffset = CalculatePlacementOffsetForPopUp(preferredPlacement);
                popup.HorizontalOffset = requestedOffset.HorizontalOffSet + HorizontalOffsetDeviation();
                popup.VerticalOffset = requestedOffset.VerticalOffSet + VerticalOffsetDeviation();

                if (!shouldBoundToXamlRoot || requestedOffset.IsFittingWithinBounds)
                {
                    return popup.IsOpen = true;
                }
            }
            return false;
        }

        #endregion


        #region PositioningCalculation


        /// <summary>
        /// Calculates Horizontal Offset,Vertical Offset, And If Space For Pop is Available 
        /// </summary>
        /// <param name="requestedPlacement"></param>
        /// <returns></returns>
        private static PopUpOffset CalculatePlacementOffsetForPopUp(PopUpPlacement requestedPlacement)
        {

            var distanceX = TargetCoordinatesInCoreWindowSpace.X - PopUpCoordinatesInCoreWindowSpace.X;
            var distanceY = TargetCoordinatesInCoreWindowSpace.Y - PopUpCoordinatesInCoreWindowSpace.Y;
            var placementOffset = new PopUpOffset();

            switch (requestedPlacement)
            {

                case PopUpPlacement.Top:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Top);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Center, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case PopUpPlacement.TopLeft:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Top);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Left, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case PopUpPlacement.TopRight:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Top);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Right, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;

                case PopUpPlacement.Bottom:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Bottom);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Center, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case PopUpPlacement.BottomRight:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Bottom);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Right, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case PopUpPlacement.BottomLeft:
                    placementOffset.VerticalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Bottom);
                    placementOffset.HorizontalOffSet = CalculateOffsetForAlignment(distanceX, Alignment.Left, TargetCoordinatesInCoreWindowSpace.Width, PopUpCoordinatesInCoreWindowSpace.Width);
                    break;
                case PopUpPlacement.Left:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Left);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Center, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);

                    break;
                case PopUpPlacement.LeftTop:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Left);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Top, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);
                    break;
                case PopUpPlacement.LeftBottom:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Left);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Bottom, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);

                    break;
                case PopUpPlacement.Right:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Right);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Center, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);

                    break;
                case PopUpPlacement.RightTop:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Right);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Top, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);
                    break;
                case PopUpPlacement.RightBottom:
                    placementOffset.HorizontalOffSet = CalculateOffsetForSidePlacement(distanceX, distanceY, Side.Right);
                    placementOffset.VerticalOffSet = CalculateOffsetForAlignment(distanceY, Alignment.Bottom, TargetCoordinatesInCoreWindowSpace.Height, PopUpCoordinatesInCoreWindowSpace.Height);
                    break;

            }

            CheckIfSpaceForPopupPositioningAvailableAvailable(placementOffset, requestedPlacement, distanceX, distanceY);

            return placementOffset;
        }
        private static void CheckIfSpaceForPopupPositioningAvailableAvailable(PopUpOffset placementOffset, PopUpPlacement preferredPlacement, double distanceX, double distanceY)
        {
            bool hasVerticalSpace = default;
            bool hasHorizontalSpace = default;
            var verticalOffset = placementOffset.VerticalOffSet - VerticalOffsetDeviation();
            var horizontalOffset = placementOffset.HorizontalOffSet - HorizontalOffsetDeviation();

            switch (preferredPlacement)
            {

                case PopUpPlacement.Top:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset >= 0;
                    hasHorizontalSpace = (PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0 && PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width);
                    break;
                case PopUpPlacement.TopLeft:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset >= 0;
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + distanceX + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;

                    break;
                case PopUpPlacement.TopRight:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset >= 0;
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0;
                    break;
                case PopUpPlacement.Bottom:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;
                    hasHorizontalSpace = (PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0 && PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width);
                    break;

                case PopUpPlacement.BottomRight:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0;

                    break;
                case PopUpPlacement.BottomLeft:
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + distanceX + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;
                    break;
                case PopUpPlacement.Left:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset >= 0 && PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;
                    break;
                case PopUpPlacement.LeftTop:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + distanceY + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;

                    break;
                case PopUpPlacement.LeftBottom:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset >= 0;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + distanceY - PopUpCoordinatesInCoreWindowSpace.Height >= 0 && PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;


                    break;
                case PopUpPlacement.Right:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset >= 0 && PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;

                    break;
                case PopUpPlacement.RightTop:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + distanceY + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;

                    break;
                case PopUpPlacement.RightBottom:
                    hasHorizontalSpace = PopUpCoordinatesInCoreWindowSpace.X + horizontalOffset + PopUpCoordinatesInCoreWindowSpace.Width <= WindowBounds.Width;
                    hasVerticalSpace = PopUpCoordinatesInCoreWindowSpace.Y + distanceY - PopUpCoordinatesInCoreWindowSpace.Height >= 0 && PopUpCoordinatesInCoreWindowSpace.Y + verticalOffset + PopUpCoordinatesInCoreWindowSpace.Height <= WindowBounds.Height;


                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(preferredPlacement), preferredPlacement, "UnKnown PopUp Position");
            }

            placementOffset.IsFittingWithinBounds = hasHorizontalSpace && hasVerticalSpace;

        }


        private static double VerticalOffsetDeviation()
        {
            return PlacementOffset.Top - PlacementOffset.Bottom;
        }

        private static double HorizontalOffsetDeviation()
        {
            return PlacementOffset.Left - PlacementOffset.Right;
        }

        private static double CalculateOffsetForSidePlacement(double distanceX, double distanceY, Side side)
        {
            return side switch
            {
                Side.Left => distanceX - PopUpCoordinatesInCoreWindowSpace.Width - PlacementMargin.Left,

                Side.Right => distanceX + TargetCoordinatesInCoreWindowSpace.Width + PlacementMargin.Right,

                Side.Top => distanceY - PopUpCoordinatesInCoreWindowSpace.Height - PlacementMargin.Top,

                Side.Bottom => distanceY + TargetCoordinatesInCoreWindowSpace.Height + PlacementMargin.Bottom,

                _ => default
            };
           
        }

        private static double CalculateOffsetForAlignment(double distance, Alignment alignment, double targetElementDimention, double popUpElementDimentions)
        {
            return  alignment switch
            {
                Alignment.Center => distance - (popUpElementDimentions - targetElementDimention) / 2,

                Alignment.Right => distance - popUpElementDimentions + targetElementDimention,

                Alignment.Left => distance,

                Alignment.Top => distance,

                Alignment.Bottom => distance - popUpElementDimentions + targetElementDimention,
                _ => default
            };
          
        }


        #endregion


    }
    public enum PopUpPlacement
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
