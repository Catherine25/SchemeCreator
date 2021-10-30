using SchemeCreator.Data.Services;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Dialogs
{
    public sealed partial class Message : ContentDialog
    {
        public Message(MessageData data)
        {
            InitializeComponent();

            Title = data.Title;
            PrimaryButtonText = data.PrimaryButton ?? "";
            SecondaryButtonText = data.SecondaryButton ?? "";

            textBlock.Text = data.Description;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }
    }
}
