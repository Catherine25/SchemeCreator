using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.History;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class TraceLayer : UserControl
    {
        public TraceLayer()
        {
            InitializeComponent();
            Grid.InitGridColumnsAndRows(Constants.GridSize);
        }

        public void ShowTracings(IEnumerable<HistoryComponent> history) => throw new NotImplementedException();
    }
}
