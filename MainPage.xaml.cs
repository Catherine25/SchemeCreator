using SchemeCreator.Data.Services;
using SchemeCreator.Data.Services.Alignment;
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

            NewBt.Click += NewBt_Click;
            LoadBt.Click += LoadBt_Click;
            SaveBt.Click += SaveBt_Click;
            AlignBt.Click += AlignBt_ClickAsync;
            TraceBt.Click += TraceBt_Click;
            WorkBt.Click += WorkBt_Click;
        }

        private async void WorkBt_Click(object sender, RoutedEventArgs e)
        {
            var validationResult = SchemeValidator.ValidateAsync(Scheme);

            if (!validationResult)
                return;

            // reset all components at first
            Scheme.Reset();

            var Result = WorkAlgorithm.Visualize(Scheme);

            if (Result == WorkAlgorithmResult.SchemeIsntCorrect)
                await new Message(Messages.ImpossibleToVisualize).ShowAsync();
        }

        private void TraceBt_Click(object sender, RoutedEventArgs e)
        {
            Tracer tracer = new(Scheme);

            var result = tracer.Run();

            Scheme.ShowTracings(result.TraceHistory);
        }

        private async void AlignBt_ClickAsync(object sender, RoutedEventArgs e)
        {
            Tracer tracer = new(Scheme);
            var tracerResult = tracer.Run();

            Aliner liner = new(Scheme, tracerResult);
            liner.Run();
        }

        private async void SaveBt_Click(object sender, RoutedEventArgs e) => await Serializer.Save(Scheme);

        private async void LoadBt_Click(object sender, RoutedEventArgs e) => await Serializer.Load(Scheme);

        private async void NewBt_Click(object sender, RoutedEventArgs e)
        {
            var result = await new Message(Messages.CreateNew).ShowAsync();

            if (result == ContentDialogResult.Primary)
                Scheme.Clear();
        }
    }
}