﻿using SchemeCreator.Data.Services;
using SchemeCreator.Data.Services.Alignment;
using SchemeCreator.Data.Services.Serialization;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Exceptions.Displayable;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.History;
using SchemeCreator.Test;
using SchemeCreator.UI.Dialogs;

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

        private async void TraceBt_Click(object sender, RoutedEventArgs e)
        {
            Tracer tracer = new(Scheme);

            HistoryService? result = null;
            
            try
            {
                result = tracer.Run();
            }
            catch (TracingErrorException)
            {
                await new Message(Messages.TracingError).ShowAsync();
                return;
            }
            
            if (result != null)
                Scheme.ShowTracings(result);
        }

        private async void AlignBt_Click(object sender, RoutedEventArgs e)
        {
            Aligner liner = new(Scheme);
            
            try
            {
                liner.Run();
            }
            catch (DisplayableException exception)
            {
                this.Log(exception.Message);
                await new Message(exception).ShowAsync();
            }
            
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