using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Models
{
    public class SmartGrid: IGridChild
    {
        public SmartGrid()
        {
            _grid = new Grid();
            _grid.SetStandartAlighnment();
        }

        public Rect Rect
        {
            get => _grid.GetRect();
            set => _grid.SetRect(value);
        }

        public Point GetXY() => _grid.GetLeftTop();

        public void Add(UIElement element) => _grid.Children.Add(element);
        public void Add(Grid grid) => _grid.Children.Add(grid);
        public void Remove(UIElement element) => _grid.Children.Remove(element);
        public void Clear() => _grid.Children.Clear();
        public Size GetActualSize() => new Size(_grid.ActualWidth, _grid.ActualHeight);

        public void AddToParent(SmartGrid parent) => parent._grid.Children.Add(_grid);
        public void AddToParent(MainPage parent) => parent.Content = _grid;

        private Grid _grid;
    }
}
