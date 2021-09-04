using SchemeCreator.Data.Services;
using SchemeCreator.Data.Services.Serialization;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator
{
    public sealed partial class MainPage : Page
    {
        private Brush ActiveBrush;
        private Brush InactiveBrush;
        private Brush UnknownBrush;

        public MainPage()
        {
            InitializeComponent();

            ActiveBrush = Colorer.GetBrushByValue(true);
            InactiveBrush = Colorer.GetBrushByValue(false);
            UnknownBrush = Colorer.GetBrushByValue(null);

            XNewSchemeBt.Click += XNewSchemeBt_Click;
            XLoadSchemeBt.Click += XLoadSchemeBt_Click;
            XSaveSchemeBt.Click += XSaveSchemeBt_Click;
            XTraceSchemeBt.Click += XTraceSchemeBt_Click;
            XWorkSchemeBt.Click += XWorkSchemeBt_Click;
        }

        #region Menu

        private async void XWorkSchemeBt_Click(object sender, RoutedEventArgs e)
        {
            Tracer tracer = new(XScheme);

            var result = tracer.Run();

            foreach (GateView gate in XScheme.Gates)
                gate.Reset();

            WorkAlgorithmResult Result = WorkAlgorithm.Visualize(XScheme, result);

            if (Result == WorkAlgorithmResult.ExInsNotInited)
                await new Message(MessageTypes.ExInsNotInited).ShowAsync();
            else if (Result == WorkAlgorithmResult.GatesNotConnected)
                await new Message(MessageTypes.GatesNotConnected).ShowAsync();
            else if (Result == WorkAlgorithmResult.SchemeIsntCorrect)
                await new Message(MessageTypes.VisualizingFailed).ShowAsync();
        }

        private void XTraceSchemeBt_Click(object sender, RoutedEventArgs e)
        {
            Tracer tracer = new(XScheme);

            var result = tracer.Run();

            //TODO
            //XScheme.ShowTracings(tracer.TraceHistory);
        }

        private async void XSaveSchemeBt_Click(object sender, RoutedEventArgs e) => await Serializer.Save(XScheme);

        private async void XLoadSchemeBt_Click(object sender, RoutedEventArgs e) => await Serializer.Load(XScheme);

        private async void XNewSchemeBt_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogResult result = await new Message(MessageTypes.NewSchemeButtonClicked).ShowAsync();

            if (result == ContentDialogResult.Primary)
                XScheme.Clear();
        }

        #endregion
    }
}