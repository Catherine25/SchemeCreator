using Windows.UI.Xaml.Controls;
using static SchemeCreator.Data.Constants;
using SchemeCreator.Data.Services;

namespace SchemeCreator.UI
{
    public sealed partial class Message : ContentDialog
    {
        public Message(MessageTypes mt)
        {
            InitializeComponent();

            string[] messageInfo = TextController.GetText(mt);

            Title = messageInfo[0];
            PrimaryButtonText = messageInfo[2];
            SecondaryButtonText = messageInfo[3];

            textBlock.Text = messageInfo[1];
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }
    }
}
