using System.Collections.Generic;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data.Services
{
    static class Colorer
    {
        private enum AccentEnum { accent, dark1, dark2, dark3, light1, light2, light3, background, foreground, accent2 }

        private static Dictionary<AccentEnum, SolidColorBrush> brushes = new Dictionary<AccentEnum, SolidColorBrush>
        {
            { AccentEnum.accent, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.Accent)) },
            { AccentEnum.background, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.Background)) },
            { AccentEnum.foreground, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.Foreground)) },

            { AccentEnum.dark1, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentDark1)) },
            { AccentEnum.dark2, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentDark2)) },
            { AccentEnum.dark3, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentDark3)) },

            { AccentEnum.light1, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentLight1)) },
            { AccentEnum.light2, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentLight2)) },
            { AccentEnum.light3, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentLight3)) },

            { AccentEnum.accent2, new SolidColorBrush(Windows.UI.Colors.Maroon) }
        };

        static Colorer()
        {
            ActiveBrush = brushes[AccentEnum.light1];
            InactiveBrush = brushes[AccentEnum.dark1];
            UnknownBrush = brushes[AccentEnum.accent2];
        }

        private static SolidColorBrush ActiveBrush;
        private static SolidColorBrush InactiveBrush;
        private static SolidColorBrush UnknownBrush;

        public static SolidColorBrush GetBrushByValue(bool? value) =>
            value == true ? ActiveBrush : value == false ? InactiveBrush : UnknownBrush;

        public static SolidColorBrush GetGateForegroundBrush() => ActiveBrush;
        public static SolidColorBrush GetGateBackgroundBrush() => InactiveBrush;

        public static void SetFillByValue(Button button, bool? value)
        {
            if (value == true)
            {
                button.Foreground = InactiveBrush;
                button.Background = ActiveBrush;
            }
            else if (value == false)
            {
                button.Foreground = ActiveBrush;
                button.Background = InactiveBrush;
            }
            else
            {
                button.Foreground = ActiveBrush;
                button.Background = UnknownBrush;
            }
        }

        public static void SetFillByValue(Ellipse ellipse, bool? value)
        {
            if (value == true)
                ellipse.Fill = ActiveBrush;
            else if (value == false)
                ellipse.Fill = InactiveBrush;
            else
                ellipse.Fill = UnknownBrush;
            ellipse.Stroke = GetBrushByValue(value);
        }

        public static void SetFillByValue(Line line, bool? value)
        {
            if (value == true)
            {
                line.Stroke = ActiveBrush;
                line.Fill = InactiveBrush;
            }
            else if (value == false)
            {
                line.Stroke = InactiveBrush;
                line.Fill = ActiveBrush;
            }
            else
            {
                line.Stroke = UnknownBrush;
                line.Fill = UnknownBrush;
            }
        }

        public static void ColorGrid(StackPanel grid) => grid.Background = InactiveBrush;

        public static void ColorMenuButton(Button button)
        {
            button.Background = InactiveBrush;
            button.Foreground = ActiveBrush;
        }

        public static void ColorMenuButtonBorderByValue(Button button, bool value) =>
            button.BorderBrush = value ? ActiveBrush : InactiveBrush;

        public static Brush ActivatedColor = new SolidColorBrush(Colors.White);
        public static Brush DeactivatedColor = new SolidColorBrush(Colors.Transparent);
    }
}
