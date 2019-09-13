using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using static SchemeCreator.Data.ConstantsNamespace.Constants;

namespace SchemeCreator.UI
{
    public class FrameManager
    {
        /*      constructor     */

        public FrameManager(Scheme _scheme)
        {
            scheme = _scheme;

            workspaceController = new WorkspaceController();
            menuController = new MenuController();

            menuController.SetParentGrid(Grid);
            workspaceController.SetParentGrid(Grid);

            SwitchToFrame(FrameEnum.workspace, Grid);

            EventSubscribe();
        }

        /*      methods     */

        private void EventSubscribe()
        {
            menuController.NewSchemeBtClickEvent += NewSchemeEventAsync;
            menuController.LoadSchemeBtClickEvent += LoadSchemeEvent;
            menuController.SaveSchemeBtClickEvent += SaveSchemeEvent;
            menuController.TraceSchemeBtClickEvent += TraceSchemeEvent;
            menuController.WorkSchemeBtClickEvent += WorkSchemeEvent;
            menuController.AddGateBtClickEvent += AddGateEvent;
            menuController.AddLineBtClickEvent += AddLineEvent;
            menuController.RemoveLineBtClickEvent += RemoveLineEvent;
            menuController.ChangeValueBtClickEvent += ChangeValueEvent;

            workspaceController.DotTappedEvent += DotTappedEventAsync;
            workspaceController.LogicGateInTapped += LogicGateInTapped;
            workspaceController.LogicGateOutTapped += LogicGateOutTapped;
            workspaceController.LogicGateTappedEvent += LogicGateTappedEvent;
            workspaceController.GateINTapped += GateINTappedEvent;
            workspaceController.GateOUTTapped += GateOUTTappedEvent;
        }

        private void LogicGateOutTapped(Ellipse e)
        {
            if (CurrentMode == ModeEnum.addLineStartMode)
            {
                CurrentMode = ModeEnum.addLineEndMode;

                newWire = new Wire
                {
                    start = e.GetCenterPoint()
                };

                Gate gate = scheme.gateController.GetGateByInOut(e, ConnectionType.output);

                newWire.isActive = gate.values[gate.GetIndexOfInOutByCenter(e.GetCenterPoint(), ConnectionType.output)];
            }
        }

        private void LogicGateInTapped(Ellipse e)
        {
            if (CurrentMode == ModeEnum.addLineEndMode)
            {
                CurrentMode = ModeEnum.addLineStartMode;

                newWire.end = e.GetCenterPoint();

                scheme.lineController.Wires.Add(newWire);

                workspaceController.ShowLines(ref scheme.lineController);
            }
        }

        private async void DotTappedEventAsync(Ellipse e)
        {
            NewGateDialog msg = new NewGateDialog();

            await msg.ShowAsync();

            if(msg.gateType != null)
            {
                Gate gate = new Gate(msg.gateType.Value, msg.inputs, msg.outputs, e.GetCenterPoint());
                scheme.gateController.Gates.Add(gate);

                workspaceController.Hide();
                workspaceController.ShowAll(ref scheme);
            }
        }

        private async void LogicGateTappedEvent(Button b)
        {
            Gate gate = scheme.gateController.GetGateByBody(b);

            await new Message(gate.type).ShowAsync();
        }

        private async void GateINTappedEvent(Button b)
        {
            ModeEnum curMode = CurrentMode;

            if (curMode == ModeEnum.addLineStartMode)
            {
                CurrentMode = ModeEnum.addLineEndMode;

                Point point = b.GetCenter();

                newWire = new Wire
                {
                    start = point,
                    isActive = scheme.gateController.GetGateByBody(b).values[0]
                };
            }
            else if (curMode == ModeEnum.changeValueMode)
            {
                Gate gate = scheme.gateController.GetGateByBody(b);

                if (gate.values[0] == null)
                    gate.values[0] = true;

                gate.values[0] = !gate.values[0];

                if (gate.values[0] == true)
                {
                    b.Background = brushes[AccentEnum.light1];
                    b.Foreground = brushes[AccentEnum.dark1];
                }
                else
                {
                    b.Background = brushes[AccentEnum.dark1];
                    b.Foreground = brushes[AccentEnum.light1];
                }
            }
            else
                await new Message(scheme.gateController.GetGateByBody(b).type).ShowAsync();
        }

        private async void GateOUTTappedEvent(Button b)
        {
            if (CurrentMode == ModeEnum.addLineEndMode)
            {
                CurrentMode = ModeEnum.addLineStartMode;

                Point point = b.GetCenter();

                newWire.end = point;

                scheme.lineController.Wires.Add(newWire);

                workspaceController.ShowLines(ref scheme.lineController);
            }
            else
                await new Message(scheme.gateController.GetGateByBody(b).type).ShowAsync();
        }

        public FrameEnum GetActiveFrame() => currentFrame;

        /*      data        */
        private readonly WorkspaceController workspaceController;

        private readonly MenuController menuController;

        private ModeEnum CurrentMode;

        private Scheme scheme;

        private FrameEnum currentFrame;

        private Wire newWire;

        public Grid Grid { get; } = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        /*      events      */
        private async void NewSchemeEventAsync(object sender, LastClickedBtEventArgs e)
        {
            if (scheme.gateController.Gates.Count == 0
                && scheme.lineController.Wires.Count == 0)
            {
                workspaceController.Hide();
                workspaceController.ShowAll(ref scheme);
            }
            else
            {
                ContentDialogResult result = await new Message(MessageTypes.newSchemeButtonClicked).ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    scheme.gateController.Gates.Clear();
                    scheme.lineController.Wires.Clear();

                    workspaceController.Hide();
                    workspaceController.ShowAll(ref scheme);
                }
            }
        }

        private void LoadSchemeEvent(object sender, LastClickedBtEventArgs e) =>
            Serializer.DeserializeAll(scheme);

        private void SaveSchemeEvent(object sender, LastClickedBtEventArgs e) =>
            Serializer.SerializeAll(scheme);

        private void TraceSchemeEvent(object sender, LastClickedBtEventArgs e)
        {
            Debug.Write("\n[Event] TraceSchemeEvent started");

            int gateCount = scheme.gateController.Gates.Count;
            Debug.WriteLine(gateCount);

            int wireCount = scheme.lineController.Wires.Count;
            Debug.WriteLine(wireCount);

            Tracer tracer = new Tracer(gateCount, wireCount);

            tracer.Trace(scheme.gateController, scheme.lineController);

            int[] tracedWireIndexes = tracer.tracedWireIndexes;

            int length = tracedWireIndexes.Length;

            for (int i = 0; i < length; i++)
                Debug.Write(" " + tracedWireIndexes[i]);

            workspaceController.ShowWireTraceIndexes(tracedWireIndexes,
            scheme.lineController);

            Debug.Write("\n[Event] TraceSchemeEvent ended");
        }

        private async void WorkSchemeEvent(object sender, LastClickedBtEventArgs e)
        {
            Debug.Write("\n[Event] WorkSchemeEvent started");

            //trace at first
            int gateCount = scheme.gateController.Gates.Count;
            int wireCount = scheme.lineController.Wires.Count;

            Tracer tracer = new Tracer(gateCount, wireCount);

            tracer.Trace(scheme.gateController, scheme.lineController);

            List<HistoryComponent> tracedComponentsHistory = tracer.traceHistory;
            int length = tracedComponentsHistory.Count;

            Debug.Write("\ntracedComponentsHistory:");
            for (int i = 0; i < length; i++)
                Debug.Write(" " + tracedComponentsHistory[i].index + " " + tracedComponentsHistory[i].component);

            //clear all values
            scheme.gateController.ClearValuesExcludingIN();
            Debug.Write("\nClearValuesExcludingIN() ended");

            WorkAlgorithmResult Result = WorkAlgorithm.Visualize(scheme);

            if (Result == WorkAlgorithmResult.exInsNotInited)
                await new Message(MessageTypes.exInsNotInited).ShowAsync();
            else if(Result == WorkAlgorithmResult.gatesNotConnected)
                await new Message(MessageTypes.gatesNotConnected).ShowAsync();
            else
            {
                workspaceController.Hide();
                workspaceController.ShowAll(ref scheme);
            }

            Debug.WriteLine("[Event] WorkSchemeEvent ended");
        }

        private void AddGateEvent(object sender, LastClickedBtEventArgs e)
        {
            menuController.InActivateModeButtons();
            e.button.BorderBrush = brushes[AccentEnum.light1];
            scheme.frameManager.CurrentMode = ModeEnum.addGateMode;
        }

        private void AddLineEvent(object sender, LastClickedBtEventArgs e)
        {
            menuController.InActivateModeButtons();
            e.button.BorderBrush = brushes[AccentEnum.light1];
            scheme.frameManager.CurrentMode = ModeEnum.addLineStartMode;
        }

        private async void RemoveLineEvent(object sender, LastClickedBtEventArgs e)
        {
            menuController.InActivateModeButtons();
            e.button.BorderBrush = brushes[AccentEnum.light1];
            scheme.frameManager.CurrentMode = ModeEnum.removeLineMode;

            await new Message(MessageTypes.functionIsNotSupported).ShowAsync();
        }

        private void ChangeValueEvent(object sender, LastClickedBtEventArgs e)
        {
            menuController.InActivateModeButtons();
            e.button.BorderBrush = brushes[AccentEnum.light1];
            scheme.frameManager.CurrentMode = ModeEnum.changeValueMode;
        }

        public void SwitchToFrame(FrameEnum frame, Grid grid)
        {
            currentFrame = frame;

            Size size = grid.GetSize();

            menuController.Show();
            menuController.Update(size);
        }

        public void UpdateView()
        {
            Size size = Grid.GetSize();

            menuController.Update(size);
            workspaceController.Update(size);
        }

        public void SizeChanged(Size size)
        {
            Grid.SetSize(size);
            UpdateView();
        }
    }
}