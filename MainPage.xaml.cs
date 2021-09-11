using SchemeCreator.Data.Services;
using SchemeCreator.Data.Services.Serialization;
using SchemeCreator.UI;
using System;
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
            var validationResult = SchemeValidator.ValidateAsync(XScheme);

            if (!validationResult)
                return;

            // reset all components at first
            XScheme.Reset();

            var Result = WorkAlgorithm.Visualize(XScheme);

            if (Result == WorkAlgorithmResult.SchemeIsntCorrect)
                await new Message(Messages.ImpossibleToVisualize).ShowAsync();
        }

        private void XTraceSchemeBt_Click(object sender, RoutedEventArgs e)
        {
            Tracer tracer = new(XScheme);

            var result = tracer.Run();

            XScheme.ShowTracings(result);
        }

        private async void XSaveSchemeBt_Click(object sender, RoutedEventArgs e) => await Serializer.Save(XScheme);

        private async void XLoadSchemeBt_Click(object sender, RoutedEventArgs e) => await Serializer.Load(XScheme);

        private async void XNewSchemeBt_Click(object sender, RoutedEventArgs e)
        {
            var result = await new Message(Messages.CreateNew).ShowAsync();

            if (result == ContentDialogResult.Primary)
                XScheme.Clear();
        }

        #endregion
    }
}