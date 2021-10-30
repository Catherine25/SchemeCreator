using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Extensions
{
    public static class GridExtension
    {
        public static void SetRect(this Grid grid, Rect rect)
        {
            grid.Margin = new Windows.UI.Xaml.Thickness
            {
                Left = rect.Left,
                Top = rect.Top
            };
            grid.Width = rect.Width;
            grid.Height = rect.Height;
        }

        public static Rect GetRect(this Grid grid) => new()
        {
            X = grid.Margin.Right,
            Y = grid.Margin.Top,
            Width = grid.Width,
            Height = grid.Height
        };

        public static Size GetActualSize(this Grid grid) => new(grid.ActualWidth, grid.ActualHeight);

        public static void SetStandartAlighnment(this Grid grid)
        {
            grid.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            grid.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
        }

        public static Point GetLeftTop(this Grid grid) => new(grid.Margin.Left, grid.Margin.Top);

        public static Size GetSize(this Grid grid) => new(grid.Width, grid.Height);

        public static void SetSize(this Grid grid, int size)
        {
            grid.Width = size;
            grid.Height = size;
        }

        public static void SetSize(this Grid grid, double w, double h)
        {
            grid.Width = w;
            grid.Height = h;
        }

        public static void SetSize(this Grid grid, Size size)
        {
            grid.Width = size.Width;
            grid.Height = size.Height;
        }

        public static void InitGridColumnsAndRows(this Grid grid, Size size)
        {
            InitColumns(grid, (int)size.Width);
            InitRows(grid, (int)size.Height);
        }
        
        public static void InitRows(this Grid grid, int count)
        {
            grid.RowDefinitions.Clear();

            for (var i = 1; i <= count; i++)
                grid.RowDefinitions.Add(new RowDefinition());
        }
        
        public static void InitColumns(this Grid grid, int count)
        {
            grid.ColumnDefinitions.Clear();

            for (var i = 1; i <= count; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        public static IEnumerable<T> GetItems<T>(this Grid grid)
            where T : UIElement
        {
            return grid.Children.Select(c => c as T);
        }
        
        public static void SetItems<T>(this Grid grid, IEnumerable<T> items)
            where T : UIElement
        {
            grid.Children.Clear();
            items.ToList().ForEach(x => grid.Add(x));
        }

        public static void Add<T>(this Grid grid, T item)
            where T : UIElement
        {
            grid.Children.Add(item);
        }
        
        public static void Remove<T>(this Grid grid, T item)
            where T : UIElement
        {
            grid.Children.Remove(item);
        }
        
        public static void Clear(this Grid grid)
        {
            grid.Children.Clear();
        }
    }
}
