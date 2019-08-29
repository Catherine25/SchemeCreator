using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI {
    interface IFrameInterface { 
        void SetParentGrid(Grid parentGrid);
        void Hide();
        void Update(Size size);
    }
}