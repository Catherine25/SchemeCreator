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
using Windows.UI.Xaml.Shapes;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SchemeCreator.UI
{
    public sealed partial class Message : ContentDialog
    {
        private Button button;
        private IList<Ellipse> ports;
        public static int choosedPortIndex;
        public Message(Constants.MessageTypes mt)
        {
            InitializeComponent();

            string[] messageInfo = Text.GetText(mt);

            Title = messageInfo[0];
            TextBlock.Text = messageInfo[1];
            PrimaryButtonText = messageInfo[2];
            SecondaryButtonText = messageInfo[3];
        }

        //public Message(Constants.ModeEnum newMode)
        //{
        //    InitializeComponent();

        //    string[] messageInfo = Text.GetText(Constants.MessageTypes.modeChanged);

        //    Title = messageInfo[0];
        //    TextBlock.Text = messageInfo[1] + " to " + newMode.ToString();
        //    PrimaryButtonText = messageInfo[2];
        //    SecondaryButtonText = messageInfo[3];
        //}

        public Message(Constants.GateEnum type)
        {
            InitializeComponent();

            Data.Gate gate = new Data.Gate(type, 2, 1, 0.0, 0.0);

            if(type == Constants.GateEnum.IN) {
                TextBlock.Text =
                    "It's an external input.\n" +
                    "You can arrange 'False' or 'True' value by clicking on it.\n" +
                    "By default it's null.";
            }
            else if(type == Constants.GateEnum.OUT) {
                TextBlock.Text =
                    "It's an external output.\n" +
                    "It gets it's value via signals visualisation.\n" +
                    "You can try it clicking on 'Work' button.";
            }
            else if(type == Constants.GateEnum.Buffer || type == Constants.GateEnum.NOT)
            {
                gate.values[0] = false;
                gate.Work();
                TextBlock.Text = "False" + "\t|\t" + gate.values[0] + "\n";

                gate.values[0] = true;
                gate.Work();
                TextBlock.Text = "False" + "\t|\t" + gate.values[0] + "\n";
            }
            else
            {
                gate.values[0] = false;
                gate.values[1] = false;
                gate.Work();
                TextBlock.Text = "False" + "\t" + "False" + "\t|\t" + gate.values[0] + "\n";

                gate.values[0] = false;
                gate.values[1] = true;
                gate.Work();
                TextBlock.Text += "False" + "\t" + "True" + "\t|\t" + gate.values[0] + "\n";

                gate.values[0] = true;
                gate.values[1] = false;
                gate.Work();
                TextBlock.Text += "True" + "\t" + "False" + "\t|\t" + gate.values[0] + "\n";

                gate.values[0] = true;
                gate.values[1] = true;
                gate.Work();
                TextBlock.Text += "True" + "\t" + "True" + "\t|\t" + gate.values[0] + "\n";
            }

            PrimaryButtonText = "Close";
        }

        public Message(Constants.ModeEnum mode, Data.Gate gate)
        {
            InitializeComponent();

            //mainGrid.Children.Clear();

            button = gate.DrawBody();
            
            if (mode == Constants.ModeEnum.addLineStartMode)
            {
                Title = "Choose an output";
                ports = gate.DrawGateInOut(Constants.ConnectionType.output);
            }
            else if (mode == Constants.ModeEnum.addLineEndMode)
            {
                Title = "Choose an input";
                ports = gate.DrawGateInOut(Constants.ConnectionType.input);
            }
            else throw new Exception();

            button.Width *= 2;
            button.Height *= 2;
            button.Margin = new Thickness
            {
                Left = 25,
                Top = 25
            };
            button.Click += Button_Click;
            mainGrid.Children.Add(button);

            int portCount = ports.Count;
            for (int i = 0; i < portCount; i++)
            {
                ports[i].Width *= 2;
                ports[i].Height *= 2;

                if (mode == Constants.ModeEnum.addLineStartMode)
                {
                    ports[i].Margin = new Thickness
                    {
                        Left = button.Margin.Left + button.Width,
                        Top = button.Margin.Top + i * ports[i].Height
                    };
                }
                else if (mode == Constants.ModeEnum.addLineEndMode)
                {
                    ports[i].Margin = new Thickness
                    {
                        Left = button.Margin.Left - (ports[i].Width),
                        Top = button.Margin.Top + i * ports[i].Height
                    };
                }

                ports[i].Tapped += Port_Tapped;

                mainGrid.Children.Add(ports[i]);

                PrimaryButtonText = "Cancel";
            }
        }

        private void Port_Tapped(object sender, TappedRoutedEventArgs e)
        {
            choosedPortIndex = ports.IndexOf(sender as Ellipse);
            //throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO: SHOW THE CARNO TABLE
            new Message(Constants.MessageTypes.functionIsNotSupported).ShowAsync();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }
    }
}
