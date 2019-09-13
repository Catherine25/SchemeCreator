using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.ConstantsNamespace;
using Windows.Foundation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SchemeCreator.UI
{
    public sealed partial class Message : ContentDialog
    {
        public Constants.GateEnum choosedGate;
        public int inputs, outputs = 1;

        public Message(Constants.MessageTypes mt)
        {
            InitializeComponent();

            string[] messageInfo = Text.GetText(mt);

            Title = messageInfo[0];
            PrimaryButtonText = messageInfo[2];
            SecondaryButtonText = messageInfo[3];

            textBlock.Text = messageInfo[1];
        }

        public Message(Constants.GateEnum type)
        {
            InitializeComponent();

            Data.Gate gate = new Data.Gate(type, 2, 1, new Point(0.0, 0.0));

            if (type == Constants.GateEnum.IN) {
                textBlock.Text =
                    "It's an external input.\n" +
                    "You can arrange 'False' or 'True' value by clicking on it.\n" +
                    "By default it's null.";
            }
            else if(type == Constants.GateEnum.OUT) {
                textBlock.Text =
                    "It's an external output.\n" +
                    "It gets it's value via signals visualisation.\n" +
                    "You can try it clicking on 'Work' button.";
            }
            else if(type == Constants.GateEnum.Buffer || type == Constants.GateEnum.NOT)
            {
                gate.values[0] = false;
                gate.Work();
                textBlock.Text = "False" + "\t|\t" + gate.values[0] + "\n";

                gate.values[0] = true;
                gate.Work();
                textBlock.Text = "False" + "\t|\t" + gate.values[0] + "\n";
            }
            else
            {
                gate.values[0] = false;
                gate.values[1] = false;
                gate.Work();
                textBlock.Text = "False" + "\t" + "False" + "\t|\t" + gate.values[0] + "\n";

                gate.values[0] = false;
                gate.values[1] = true;
                gate.Work();
                textBlock.Text += "False" + "\t" + "True" + "\t|\t" + gate.values[0] + "\n";

                gate.values[0] = true;
                gate.values[1] = false;
                gate.Work();
                textBlock.Text += "True" + "\t" + "False" + "\t|\t" + gate.values[0] + "\n";

                gate.values[0] = true;
                gate.values[1] = true;
                gate.Work();
                textBlock.Text += "True" + "\t" + "True" + "\t|\t" + gate.values[0] + "\n";
            }

            PrimaryButtonText = "Close";
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }
    }
}
