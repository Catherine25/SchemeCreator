using System.Collections.Generic;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace SchemeCreator.Data.Services
{
    public static class Colorer
    {
        private enum AccentEnum { Accent, Dark1, Dark2, Dark3, Light1, Light2, Light3, Background, Foreground }

        private static readonly Dictionary<AccentEnum, SolidColorBrush> Brushes = new()
        {
            { AccentEnum.Accent, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.Accent)) },
            { AccentEnum.Background, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.Background)) },
            { AccentEnum.Foreground, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.Foreground)) },

            { AccentEnum.Dark1, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentDark1)) },
            { AccentEnum.Dark2, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentDark2)) },
            { AccentEnum.Dark3, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentDark3)) },

            { AccentEnum.Light1, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentLight1)) },
            { AccentEnum.Light2, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentLight2)) },
            { AccentEnum.Light3, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentLight3)) }
        };
        
        public static readonly SolidColorBrush ActiveBrush;
        public static readonly SolidColorBrush InactiveBrush;
        public static readonly Brush ActivatedColor;
        public static readonly Brush ErrorBrush;
        public static readonly Brush DeactivatedColor;

        static Colorer()
        {
            ActiveBrush = Brushes[AccentEnum.Light1];
            InactiveBrush = Brushes[AccentEnum.Dark1];
            DeactivatedColor = Brushes[AccentEnum.Background];
            ActivatedColor = new SolidColorBrush(Colors.White);
            ErrorBrush = new SolidColorBrush(Colors.Maroon);
        }
        
        public static Brush GetBrushByValue(bool? value) =>
            value == true ? ActiveBrush : value == false ? InactiveBrush : ErrorBrush;
    }
}
