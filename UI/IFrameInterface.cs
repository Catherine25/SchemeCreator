using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI {
    interface IFrameInterface {
        bool IsActive { get; }
        void SetParentGrid(Grid parentGrid);
        void Hide();
        void Update(Size size);
    }
}