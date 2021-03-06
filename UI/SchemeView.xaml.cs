using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

using static System.Diagnostics.Debug;

namespace SchemeCreator.UI
{
    public class WireBuilder
    {
        public Action<WireView> WireReady;

        public void SetPoint(bool isStart, Point point)
        {
            if(isStart)
                _wire.Start = point;
            else
                _wire.End = point;

            if (_wire.Start.IsInited() && _wire.End.IsInited())
            {
                WireReady(_wire);
                _wire = new WireView();
            }
        }

        private WireView _wire = new WireView();
    }

    public sealed partial class SchemeView : UserControl
    {
        private WireBuilder WireBuilder;
        public List<Ellipse> Dots;
        public List<WireView> Wires;
        public IList<GateView> Gates;
        public IList<ExternalPortView> ExternalPorts;

        public SchemeView()
        {
            InitializeComponent();

            WireBuilder = new WireBuilder();
            WireBuilder.WireReady = (wire) => AddToView(wire);

            Dots = new List<Ellipse>();
            Wires = new List<WireView>();
            Gates = new List<GateView>();
            ExternalPorts = new List<ExternalPortView>();

            InitGrid(Constants.netSize, Constants.netSize);
        }

        

        private void Wire_Tapped(WireView wire)
        {
            Wires.Remove(wire);
            XSchemeGrid.Children.Remove(wire);
        }


        public bool IsAllConnected()
        {
            Stack<GateView> gates = new Stack<GateView>(Gates);
            List<WireView> wires = new List<WireView>(Wires);

            bool found = true;

            if (AllExternalPortsConnect(PortType.Input, wires))
                return false;

            if (AllExternalPortsConnect(PortType.Output, wires))
                return false;

            while (gates.Count != 0 && found)
            {
                GateView gate = gates.Pop();
                found = false;

                int connections = gate.InputCount + gate.OutputCount;

                foreach (var wire in wires)
                    if (gate.WireConnects(wire.Start) || gate.WireConnects(wire.End))
                        connections--;

                if (connections <= 0)
                    found = true;
            }

            return gates.Count == 0;
        }

        private bool AllExternalPortsConnect(PortType type, List<WireView> wires)
        {
            IEnumerable<Point> wirePointsToCheck = type == PortType.Input
                ? wires.Select(x => x.Start)
                : wires.Select(x => x.End);

            Stack<ExternalPortView> externalPorts =
                new Stack<ExternalPortView>(ExternalPorts.Where(x => x.Type == type));

            bool found = true;

            while (externalPorts.TryPop(out var externalPort) && found)
            {
                found = false;

                foreach (Point point in wirePointsToCheck)
                    if (externalPort.Center == point)
                    {
                        found = true;
                        break;
                    }
            }

            return found;
        }

        //public void ResetGates()
        //{
        //    foreach (GateView gate in Gates)
        //        gate.Reset();
        //}

        public void InitGrid(int wCount, int hCount)
        {
            for (int i = 1; i <= wCount; i++)
                XSchemeGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int j = 1; j <= hCount; j++)
                XSchemeGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 1; i <= wCount; i++)
                for (int j = 1; j <= hCount; j++)
                {
                    Ellipse ellipse = new Ellipse
                    {
                        HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                        VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
                        Width = Constants.dotSize.Width,
                        Height = Constants.dotSize.Height
                    };

                    Grid.SetRow(ellipse, i - 1);
                    Grid.SetColumn(ellipse, j - 1);

                    ellipse.PointerEntered += (sender, args) => ellipse.IncreaseSize();
                    ellipse.PointerExited += (sender, args) => ellipse.DecreaseSize();
                    ellipse.Tapped += (sender, args) => DotTappedEventAsync(sender as Ellipse);

                    Colorer.SetFillByValue(ellipse, false);

                    Dots.Add(ellipse);
                    XSchemeGrid.Children.Add(ellipse);
                }
        }

        private async void DotTappedEventAsync(Ellipse e)
        {
            NewGateDialog msg = new NewGateDialog();

            await msg.ShowAsync();

            if (msg.gateType != null)
            {
                Vector2 location = new Vector2(Grid.GetColumn(e), Grid.GetRow(e));

                if(msg.Gate != null)
                {
                    msg.Gate.MatrixLocation = location;
                    AddToView(msg.Gate);
                }
                else if(msg.ExternalPort != null)
                {
                    msg.ExternalPort.MatrixLocation = location;
                    AddToView(msg.ExternalPort);
                }
            }
        }

        private void ExternalPortTapped(ExternalPortView externalPort)
        {
            if (MainPage.CurrentMode == Constants.ModeEnum.addLineMode)
                WireBuilder.SetPoint(externalPort.Type == PortType.Input, externalPort.GetCenterRelativeTo(XSchemeGrid));
            else
                externalPort.SwitchMode();
        }

        private void GatePortTapped(GatePortView arg1, GateView arg2)
        {
            WriteLine("GatePortTapped");
            WireBuilder.SetPoint(arg1.Type != Data.Models.Enums.ConnectionTypeEnum.Input, arg1.GetCenterRelativeTo(XSchemeGrid));
        }

        private void GateBodyTapped(GateBodyView arg1, GateView arg2)
        {
            throw new NotImplementedException();
        }

        public ExternalPortView GetFirstNotInitedExternalPort() =>
            GetExternalPorts(PortType.Input)
                .FirstOrDefault(x => x.Value == null);

        public IEnumerable<ExternalPortView> GetExternalPorts(PortType type) =>
            ExternalPorts.Where(x => x.Type == type);

        private void AddToView(GateView gate)
        {
            gate.GateBodyTapped += GateBodyTapped;
            gate.GatePortTapped += GatePortTapped;
            XSchemeGrid.Children.Add(gate);
            Gates.Add(gate);
        }

        private void AddToView(ExternalPortView port)
        {
            port.Tapped += ExternalPortTapped;
            XSchemeGrid.Children.Add(port);
            ExternalPorts.Add(port);
        }

        private void AddToView(WireView wire)
        {
            WriteLine("AddToView");
            
            Grid.SetColumnSpan(wire, Constants.netSize);
            Grid.SetRowSpan(wire, Constants.netSize);

            Wires.Add(wire);
            XSchemeGrid.Children.Add(wire);
            wire.Tapped += Wire_Tapped;
        }
    }
}
