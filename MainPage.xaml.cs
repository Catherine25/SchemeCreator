using SchemeCreator.Data.Services;
using SchemeCreator.Data.Services.Alignment;
using SchemeCreator.Data.Services.Serialization;
using SchemeCreator.UI;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            NewBt.Click += NewBt_Click;
            LoadBt.Click += LoadBt_Click;
            SaveBt.Click += SaveBt_Click;
            AlignBt.Click += AlignBt_Click;
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

            var result = WorkAlgorithm.Visualize(Scheme);

            if (result == WorkAlgorithmResult.BadScheme)
                await new Message(Messages.ImpossibleToVisualize).ShowAsync();
        }

        private void TraceBt_Click(object sender, RoutedEventArgs e)
        {
            Tracer tracer = new(Scheme);

            var result = tracer.Run();

            Scheme.ShowTracings(result);
        }

        private void AlignBt_Click(object sender, RoutedEventArgs e)
        {
            Aliner liner = new(Scheme);
            liner.Run();
            Scheme.ClearTracings();
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