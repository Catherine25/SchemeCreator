using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SchemeCreator.UI.Dynamic;
using SchemeCreator.UI.Layers;

namespace SchemeCreator.UI
{
    public class CustomGateConfiguration
    {
        public string Name { get; set; }
        public bool DefaultOutput { get; set; }
        public IEnumerable<ExceptionSet> ExceptionSets { get; set; }
    }

    public class ExceptionSet
    {
        public IEnumerable<bool?> Exceptions;
    }
    
    public sealed partial class CustomGateDialog : ContentDialog
    {
        public readonly int PortCount;
        public bool defaultOutput;
        public CustomGateConfiguration exceptionsData;

        public CustomGateDialog(int inputsCount)
        {
            InitializeComponent();
            
            PortCount = inputsCount;
            exceptionsData = new CustomGateConfiguration();
            
            AddLayer();

            AddCaseBt.Click += AddCaseBtOnClick;
        }

        private void AddCaseBtOnClick(object sender, RoutedEventArgs e) => AddLayer();

        private void AddLayer()
        {
            var layer = new PortConfigurationLayer();
            for (int i = 0; i < PortCount; i++)
                layer.Add(new PortConfigurationView());
            Exceptions.Add(layer);
        }

        private ExceptionSet BuildExceptionSet(IEnumerable<bool?> values) => new(){ Exceptions = values };
        private CustomGateConfiguration BuildCustomGateConfiguration(string name, bool output, IEnumerable<ExceptionSet> sets) => new CustomGateConfiguration()
        {
            Name = name,
            DefaultOutput = output,
            ExceptionSets = sets
        };

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            defaultOutput = OutputSwitch.IsOn;
            exceptionsData = BuildCustomGateConfiguration(Name.Text, defaultOutput,
                Exceptions.Items.Select(x => BuildExceptionSet(x.Items.Select(y => (bool?) y.Value))));
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
