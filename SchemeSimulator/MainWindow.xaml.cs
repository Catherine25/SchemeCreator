﻿using System;
using System.Windows;

namespace SchemeSimulator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            NewSchemeBt.Click += NewSchemeBt_Click;
            LoadSchemeBt.Click += LoadSchemeBt_Click;
            SaveSchemeBt.Click += SaveSchemeBt_Click;
            TraceSchemeBt.Click += TraceSchemeBt_Click;
            WorkSchemeBt.Click += WorkSchemeBt_Click;
            
            //todo: AddLineBt
            //todo: ChangeValueBt
        }

        private void NewSchemeBt_Click(object sender, RoutedEventArgs e) => throw new NotImplementedException();

        private void LoadSchemeBt_Click(object sender, RoutedEventArgs e) => throw new NotImplementedException();
        
        private void SaveSchemeBt_Click(object sender, RoutedEventArgs e) => throw new NotImplementedException();

        private void TraceSchemeBt_Click(object sender, RoutedEventArgs e) => throw new NotImplementedException();

        private void WorkSchemeBt_Click(object sender, RoutedEventArgs e) => throw new NotImplementedException();
    }
}
