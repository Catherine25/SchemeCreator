using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Extensions
{
    public static class GridExtension
    {
        public static void SetSize(this Grid grid, double w, double h)
        {
            grid.Width = w;
            grid.Height = h;
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

        private static void InitColumns(this Grid grid, int count)
        {
            grid.ColumnDefinitions.Clear();

            for (var i = 1; i <= count; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        public static IEnumerable<T> GetItems<T>(this Grid grid) where T : UIElement =>
            grid.Children.Select(c => c as T)!;

        public static void SetItems<T>(this Grid grid, IEnumerable<T> items)
            where T : UIElement
        {
            grid.Children.Clear();
            items.ToList().ForEach(grid.Add);
        }

        public static void Add<T>(this Grid grid, T item) where T : UIElement => grid.Children.Add(item);
        public static void Remove<T>(this Grid grid, T item) where T : UIElement => grid.Children.Remove(item);
        public static void Clear(this Grid grid) => grid.Children.Clear();
    }
}
