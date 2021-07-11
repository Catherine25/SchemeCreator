using SchemeCreator.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using SchemeCreator.Data.Models.Enums;
using static SchemeCreator.Data.Constants;
using Windows.UI.Xaml;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GateView : UserControl
    {
        public readonly GateEnum Type;

        public readonly string Text;

        public readonly Size GateBodySize;
        public readonly GridLength PortsMargin;

        public readonly Size PortSize;
        private GridLength PortWidth => new GridLength(PortSize.Width);

        public Vector2 MatrixLocation
        {
            get
            {
                return _matrixIndex;
            }
            set
            {
                _matrixIndex = value;
                Grid.SetColumn(this, (int)_matrixIndex.X);
                Grid.SetRow(this, (int)_matrixIndex.Y);
            }
        } 
        private Vector2 _matrixIndex;

        public readonly Brush ForegroundBrush;
        public readonly Brush BackgroundBrush;

        public Action<GateBodyView, GateView> GateBodyTapped;
        public Action<GatePortView, GateView> GatePortTapped;

        public int InputCount;
        public int OutputCount;

        public bool AreOutputsReady => XOutputs.Children.Any(x => ((GatePortView) x).Value != null);

        public GateView(GateEnum type, Vector2 point, int inputs = 0, int outputs = 0)
        {
            Type = type;
            InputCount = inputs;
            OutputCount = outputs;
            MatrixLocation = point;
            Text = GateNames[type];

            ForegroundBrush = Colorer.GetGateForegroundBrush();
            BackgroundBrush = Colorer.GetGateBackgroundBrush();

            GateBodySize = LogicGateSize;
            PortSize = GatePortSize;

            PortsMargin = new GridLength(MarginBetweenPorts);

            InitializeComponent();

            xBody.Tapped += (sender, args) => GateBodyTapped(FindName("xBody") as GateBodyView, this);
            xBody.Width = LogicGateSize.Width;
            xBody.Height = LogicGateSize.Height;

            XInputs.SetSize(GatePortSize.Width, LogicGateSize.Height);

            CreatePorts(inputs, outputs);
        }

        public void Work()
        {
            List<bool?> values =
                GateWorkPatterns.ActionByType[Type](XInputs.Children
                    .Select(x => ((GatePortView) x).Value)
                    .ToList());

            var outPorts = XOutputs.Children.Select(o => o as GatePortView).ToList();
            
            for (int i = 0; i < outPorts.Count; i++)
                outPorts[i].Value = values[i];
        }

        private void CreatePorts(int inputs, int outputs)
        {
            CreateInputs(inputs);
            CreateOutputs(outputs);
        }

        private void CreateInputs(int count)
        {
            for (int i = 0; i < count; i++)
                XInputs.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < count; i++)
            {
                GatePortView port = new(ConnectionTypeEnum.Input, i);
                port.Tapped += (port) => GatePortTapped(port, this);
                port.ValueChanged += newValue => { Work(); };
                XInputs.Children.Add(port);
            }
        }
        private void CreateOutputs(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GatePortView port = new(ConnectionTypeEnum.Output, i);
                port.Tapped += (port) => GatePortTapped(port, this);
                XOutputs.Children.Add(port);
            }
        }

        public void SetInputValueFromWire(WireView wire)
        {
            XInputs.Children
                .Select(x => x as GatePortView)
                .FirstOrDefault(x => x.Center == wire.End)
                .Value = wire.Value;
        }

        public void Reset()
        {
            foreach (var child in XInputs.Children)
                (child as GatePortView).Value = null;
        }

        // public Vector3 GetLeftTop()
        // {
        //     if(external.Contains(Type))
        //     {
        //         return new Vector3
        //         {
        //             X = (float)(MatrixIndex.X - externalGateSize.Width / 2),
        //             Y = (float)(MatrixIndex.Y - externalGateSize.Height / 2)
        //         };
        //     }
        //     else
        //     {
        //         return new Vector3
        //         {
        //             X = (float)(MatrixIndex.X - logicGateSize.Width / 2),
        //             Y = (float)(MatrixIndex.Y - logicGateSize.Height / 2)
        //         };
        //     }

        // }

        // public GateBody DrawBody() => new GateBody(this);

        // public List<GatePortView> DrawPorts(ConnectionTypeEnum type)
        // {
        //     if(type == ConnectionTypeEnum.Both)
        //     {
        //         var items = DrawPorts(ConnectionTypeEnum.Input);
        //         items.AddRange(DrawPorts(ConnectionTypeEnum.Output));
        //         return items;
        //     }
        //     else
        //     {
        //         List<GatePortView> ports = new List<GatePortView>();
        //         int length = type == ConnectionTypeEnum.Input ? Inputs : Outputs;

        //         for (int i = 0; i < length; i++)
        //         {
        //             GatePortView port = new GatePortView(type);

        //             Vector3 center =
        //                 type == ConnectionTypeEnum.Input
        //                 ? new Vector3(GetLeftTop().X, (float)(GetLeftTop().Y + (logicGateSize.Height / (length + 1) * (i + 1))), 0)
        //                 : new Vector3((float)(GetLeftTop().X + logicGateSize.Width), (float)(GetLeftTop().Y + (logicGateSize.Height / (length + 1) * (i + 1))), 0);

        //             port.CenterPoint = center;

        //             port.Value = Values[i];

        //             ports.Add(port);
        //         }

        //         return ports;
        //     }            
        // }

        // public bool ContainsInOutByCenter(Vector3 center, ConnectionTypeEnum type) =>
        //     DrawPorts(type).Exists(x => x.CenterPoint == center);

        // public bool ContainsBodyByMargin(Thickness t) =>
        //     DrawBody().ContainsBodyByMargin(t);

        // public int GetIndexOfInOutByCenter(Vector3 center, ConnectionTypeEnum type) =>
        //     DrawPorts(type).FindIndex(x => x.CenterPoint == center);

        // public GateBody GetBodyByWirePart(Vector3 p)
        // {
        //     if (Type == GateEnum.IN || Type == GateEnum.OUT)
        //         if (p == MatrixIndex)
        //             return DrawBody();

        //     return null;
        // }

        // public GatePortView GetInOutByWirePart(Vector3 p)
        // {
        //     var items = DrawPorts(ConnectionTypeEnum.Both);
        //     return items.FirstOrDefault(i => i.CenterPoint == p);
        // }

        public bool WireConnects(Point point)
        {
            var inputs = XInputs.Children.Select(i => i as GatePortView);
            var outputs = XOutputs.Children.Select(i => i as GatePortView);

            return inputs.Any(i => i.Center == point) || outputs.Any(i => i.Center == point);
        }

        // public int FirstFreeValueBoxIndex()
        // {
        //     for (int i = 0; i < Inputs; i++)
        //         if (Values[i] == null)
        //             return i;

        //     return -1;
        // }

        // public void AddToParent(SmartGrid parent) =>
        //     DrawBody().AddToParent(parent);
    }
}
