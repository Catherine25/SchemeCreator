﻿using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Extensions
{
    public static class GridExtension
    {
        public static void SetSize(this Grid grid, Size size)
        {
            grid.Width = size.Width;
            grid.Height = size.Height;
        }
        public static Size GetSize(this Grid grid) => new Size(grid.Width, grid.Height);
    }
}
