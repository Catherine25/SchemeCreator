using SchemeCreator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

using static SchemeCreator.Data.ConstantsNamespace.Constants;
using SchemeCreator.Data.ConstantsNamespace;

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
            newGateMenuController = new NewGateMenuController();

            menuController.SetParentGrid(Grid);
            workspaceController.SetParentGrid(Grid);
            newGateMenuController.SetParentGrid(Grid);

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

            workspaceController.DotTappedEvent += DotTappedEvent;
            workspaceController.LogicGateInTapped += LogicGateInTapped;
            workspaceController.LogicGateOutTapped += LogicGateOutTapped;
            workspaceController.LogicGateTappedEvent += LogicGateTappedEvent;
            workspaceController.GateINTapped += GateINTappedEvent;
            workspaceController.GateOUTTapped += GateOUTTappedEvent;

            newGateMenuController.NewGateBtClickedEvent += NewGateBtClickedEvent;
        }

        private void LogicGateOutTapped(Ellipse e)
        {
            if (modeManager.CurrentMode == ModeEnum.addLineStartMode)
            {
                modeManager.CurrentMode = ModeEnum.addLineEndMode;

                newWire = new Wire
                {
                    start = new Point(
                    e.Margin.Left + lineStartOffset,
                    e.Margin.Top + lineStartOffset)
                };

                Gate gate = scheme.gateController.GetGateByInOut(e, false);

                newWire.isActive = gate.values[gate.GetIndexOfInOutByMargin(e.Margin, false)];
            }
        }

        private void LogicGateInTapped(Ellipse e)
        {
            if (modeManager.CurrentMode == ModeEnum.addLineEndMode)
            {
                modeManager.CurrentMode = ModeEnum.addLineStartMode;

                Point point = new Point
                {
                    X = e.Margin.Left + lineStartOffset,
                    Y = e.Margin.Top + lineStartOffset
                };

                newWire.end = point;

                scheme.lineController.Wires.Add(newWire);

                workspaceController.ShowLines(ref scheme.lineController);
            }
        }

        private void DotTappedEvent(Ellipse e)
        {
            if (modeManager.CurrentMode == ModeEnum.addGateMode)
                SwitchToFrame(FrameEnum.newGate, Grid);

            scheme.dotController.lastTapped = e;
        }

        private void LogicGateTappedEvent(Button b) => new Message(MessageTypes.functionIsNotSupported).ShowAsync();

        private void GateINTappedEvent(Button b)
        {
            ModeEnum curMode = modeManager.CurrentMode;

            if (curMode == ModeEnum.addLineStartMode)
            {
                modeManager.CurrentMode = ModeEnum.addLineEndMode;

                Point point = new Point
                {
                    X = b.Margin.Left + externalGateWidth / 2,
                    Y = b.Margin.Top + externalGateHeight / 2
                };

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
        }

        private void GateOUTTappedEvent(Button b)
        {
            if (modeManager.CurrentMode == ModeEnum.addLineEndMode)
            {
                modeManager.CurrentMode = ModeEnum.addLineStartMode;

                Point point = new Point()
                {
                    X = b.Margin.Left + (externalGateWidth / 2),
                    Y = b.Margin.Top + (externalGateHeight / 2)
                };

                newWire.end = point;

                scheme.lineController.Wires.Add(newWire);

                workspaceController.ShowLines(ref scheme.lineController);
            }
        }

        public FrameEnum GetActiveFrame() => currentFrame;

        /*      data        */
        private WorkspaceController workspaceController;

        private MenuController menuController;

        private NewGateMenuController newGateMenuController;

        private ModeManager modeManager = new ModeManager();

        private Scheme scheme;

        private FrameEnum currentFrame;

        private Wire newWire;

        public Grid Grid { get; } = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        /*      events      */

        private void NewGateBtClickedEvent(object sender, NewGateBtClickedEventArgs e)
        {
            if (external.Contains(e.type))
            {
                Point point = new Point
                {
                    X = scheme.dotController.lastTapped.Margin.Left + dotSize,
                    Y = scheme.dotController.lastTapped.Margin.Top + dotSize
                };

                Gate gate = new Gate(
                    e.type,
                    e.inputs,
                    e.outputs,
                    point.X,
                    point.Y);

                scheme.gateController.Gates.Add(gate);
            }
            else
            {
                Point point = new Point
                {
                    X = scheme.dotController.lastTapped.Margin.Left + (dotSize / 2) - (gateWidth / 2),
                    Y = scheme.dotController.lastTapped.Margin.Top + (dotSize / 2) - (gateHeight / 2)
                };

                Gate gate = new Gate(e.type,
                e.inputs,
                e.outputs,
                point.X,
                point.Y);

                scheme.gateController.Gates.Add(gate);
            }

            SwitchToFrame(FrameEnum.workspace, Grid);
            workspaceController.ShowAll(ref scheme);
        }

        private async void NewSchemeEventAsync(object sender, LastClickedBtEventArgs e)
        {
            if (scheme.gateController.Gates.Count == 0)
                workspaceController.ShowAll(ref scheme);
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

        private void WorkSchemeEvent(object sender, LastClickedBtEventArgs e)
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

            WorkAlgorithmResult Result = WorkAlgorithm.Ver5(scheme);

            if (Result == WorkAlgorithmResult.exInsNotInited)
                new Message(MessageTypes.exInsNotInited).ShowAsync();
            else if(Result == WorkAlgorithmResult.gatesNotConnected)
                new Message(MessageTypes.gatesNotConnected).ShowAsync();
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
            scheme.frameManager.modeManager.CurrentMode = ModeEnum.addGateMode;

            new Message(MessageTypes.modeChanged, ModeEnum.addGateMode).ShowAsync();
        }

        private void AddLineEvent(object sender, LastClickedBtEventArgs e)
        {
            menuController.InActivateModeButtons();
            e.button.BorderBrush = brushes[AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = ModeEnum.addLineStartMode;

            new Message(MessageTypes.modeChanged, ModeEnum.addLineStartMode).ShowAsync();
        }

        private void RemoveLineEvent(object sender, LastClickedBtEventArgs e)
        {
            menuController.InActivateModeButtons();
            e.button.BorderBrush = brushes[AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = ModeEnum.removeLineMode;

            new Message(MessageTypes.functionIsNotSupported).ShowAsync();
        }

        private void ChangeValueEvent(object sender, LastClickedBtEventArgs e)
        {
            menuController.InActivateModeButtons();
            e.button.BorderBrush = brushes[AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = ModeEnum.changeValueMode;

            new Message(MessageTypes.modeChanged, ModeEnum.changeValueMode).ShowAsync();
        }

        public void SwitchToFrame(FrameEnum frame, Grid grid)
        {
            currentFrame = frame;

            Size size = new Size(Grid.Width, Grid.Height);

            if (frame == FrameEnum.newGate)
            {
                menuController.Hide();
                workspaceController.Hide();
                newGateMenuController.Show();
                newGateMenuController.Update(size);
            }
            else
            {
                newGateMenuController.Hide();
                menuController.Show();
                menuController.Update(size);
            }
        }

        public void UpdateView()
        {
            Debug.Assert(Grid.Width != 0);
            Debug.Assert(Grid.Height != 0);

            Size size = new Size(Grid.Width, Grid.Height);

            if (currentFrame == FrameEnum.newGate)
                newGateMenuController.Update(size);
            else
            {
                menuController.Update(size);
                workspaceController.Update(size);
            }
        }

        public void SizeChanged(Size size)
        {
            Grid.Width = size.Width;
            Grid.Height = size.Height;

            UpdateView();
        }
    }
}