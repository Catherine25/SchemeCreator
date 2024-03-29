﻿using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Interfaces
{
    public static class SchemeComponentExtension
    {
        public static Vector2 GetMatrixLocation<T>(this T control) where T : UserControl, IGridComponent =>
            new(Grid.GetColumn(control), Grid.GetRow(control));

        public static void SetMatrixLocation<T>(this T control, Vector2 value) where T: UserControl, IGridComponent
        {
            Grid.SetColumn(control, (int)value.X);
            Grid.SetRow(control, (int)value.Y);

            //must be called to update coordinates immediately
            control.UpdateLayout();
        }
    }
}
