using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using SchemeCreator.Data.ConstantsNamespace;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SchemeCreator.UI
{
    public sealed partial class Message : ContentDialog
    {
        public Message(Constants.MessageTypes mt)
        {
            InitializeComponent();

            string[] messageInfo = Text.GetText(mt);

            Title = messageInfo[0];
            TextBlock.Text = messageInfo[1];
            PrimaryButtonText = messageInfo[2];
            SecondaryButtonText = messageInfo[3];
        }

        public Message(Constants.MessageTypes mt, Constants.ModeEnum newMode)
        {
            InitializeComponent();

            string[] messageInfo = Text.GetText(mt);

            Title = messageInfo[0];
            TextBlock.Text = messageInfo[1] + " to " + newMode.ToString();
            PrimaryButtonText = messageInfo[2];
            SecondaryButtonText = messageInfo[3];
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }
    }
}
