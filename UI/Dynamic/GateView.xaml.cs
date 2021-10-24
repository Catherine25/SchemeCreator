using SchemeCreator.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;

namespace SchemeCreator.UI.Dynamic
{
    public enum GateEnum
    {
        Buffer,
        Not,
        And,
        Nand,
        Or,
        Nor,
        Xor,
        Xnor,
        Custom
    }

    public sealed partial class GateView : UserControl, ISchemeComponent
    {
        public Action<GateBodyView, GateView> GateBodyTapped;
        public Action<GatePortView, GateView> GatePortTapped;

        public GateView(GateEnum type, Vector2 point, int inputs = 1, int outputs = 1)
        {
            InitializeComponent();

            Type = type;
            MatrixLocation = point;

            Body.GateType = type;
            Body.Tapped += (sender, args) => GateBodyTapped(Body, this);

            CreateInputs(inputs);
            CreateOutputs(outputs);
        }

        public readonly GateEnum Type;

        public Vector2 MatrixLocation
        {
            get => this.GetMatrixLocation();
            set => this.SetMatrixLocation(value);
        }

        public void Work()
        {
            List<bool?> initialValues = Inputs.Select(p => p.Value).ToList();

            List<bool?> resultValues = Type == GateEnum.Custom
                ? CustomWorkFunction(initialValues)
                : StandardGateWorkPatterns.ActionByType[Type](initialValues);
            
            this.Log(resultValues[0].ToString());
            this.Log(resultValues[0].ToString());
            this.Log(resultValues[0].ToString());
            this.Log(resultValues[0].ToString());
            this.Log(resultValues[0].ToString());
            
            var outPorts = Outputs.ToList();

            for (int i = 0; i < outPorts.Count; i++)
                outPorts[i].Value = resultValues[i];
        }

        // todo fix
        public void ConfigureCustomWorkFunction(CustomGateConfiguration exceptionsData) =>
            CustomWorkFunction = _ =>
            {
                var inputs = Inputs.Select(x => x.Value).ToList();

                bool output = exceptionsData.DefaultOutput;

                foreach (var sets in exceptionsData.ExceptionSets)
                    for (int i = 0; i < sets.Exceptions.Count(); i++)
                        if (sets.Exceptions.ElementAt(i) != inputs[i])
                            output = !exceptionsData.DefaultOutput;

                return new List<bool?> { output };
            };

        private Func<List<bool?>, List<bool?>> CustomWorkFunction;

        #region Ports

        public IEnumerable<GatePortView> Inputs { get => InputsView.Items; }

        private void CreateInputs(int count)
        {
            InputsView.CreatePorts(ConnectionTypeEnum.Input, count);
            InputsView.Tapped += (port) => GatePortTapped(port, this);
            InputsView.ValuesChanged += Work;
        }
        public void Reset() => Inputs.ToList().ForEach(x => x.Value = null);

        public IEnumerable<GatePortView> Outputs { get => OutputsView.Items; }

        private void CreateOutputs(int count)
        {
            OutputsView.CreatePorts(ConnectionTypeEnum.Output, count);
            OutputsView.Tapped += (port) => GatePortTapped(port, this);
        }

        public bool AreOutputsReady => Outputs.Any(p => p.Value != null);

        #endregion

        #region Interaction with Wires

        public void SetInputValueFromWire(WireView wire) => Inputs.First(i => i.Index == wire.Connection.EndPort).Value = wire.Value;
        public void SetOutputValueToWire(WireView wire) => wire.Value = Outputs.First(o => o.Index == wire.Connection.StartPort).Value;
        public bool WireStartConnects(WireView wire) =>
            (MatrixLocation == wire.Connection.MatrixStart) && (wire.Connection.StartPort == null || Outputs.Any(i => i.Index == wire.Connection.StartPort));

        public bool WireEndConnects(WireView wire) =>
            (MatrixLocation == wire.Connection.MatrixEnd) && (wire.Connection.EndPort == null || Inputs.Any(i => i.Index == wire.Connection.EndPort));

        public bool WireConnects(WireView wire) =>
            (MatrixLocation == wire.Connection.MatrixStart) || (MatrixLocation == wire.Connection.MatrixEnd);

        #endregion
    }
}
