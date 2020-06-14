using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Models;
using SchemeCreator.Data.Services;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.UI
{
    public class FrameManager
    {
        private readonly WorkspaceController workspaceController;

        private readonly MenuController menuController;

        private ModeEnum CurrentMode;

        private Scheme scheme;

        private FrameEnum currentFrame;

        private Wire newWire;

        public Grid Grid { get; }


        public FrameManager(Scheme _scheme)
        {
            scheme = _scheme;

            newWire = null;

            workspaceController = new WorkspaceController();
            menuController = new MenuController();

            Grid = new Grid();
            Grid.SetStandartAlighnment();

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
            menuController.ChangeModeEvent += ChangeModeEvent;

            workspaceController.DotTappedEvent += DotTappedEventAsync;
            workspaceController.PortTapped += PortTapped;
            workspaceController.LogicGateTappedEvent += LogicGateTappedEvent;
            workspaceController.GateINTapped += GateINTappedEvent;
            workspaceController.GateOUTTapped += GateOUTTappedEvent;
            workspaceController.LineTappedEvent += WorkspaceController_LineTappedEvent;
        }

        private void WorkspaceController_LineTappedEvent(Line line)
        {
            scheme.lineController.Wires.Remove(scheme.lineController.FindWireByLine(line));
            workspaceController.RemoveLine(line);
        }

        private void PortTapped(Port p)
        {
            if(p.Type == ConnectionType.input)
            {
                if (CurrentMode == ModeEnum.addLineMode)
                TryCreate(p.CenterPoint, false);
            }
            else
            {
                if (CurrentMode == ModeEnum.addLineMode)
                {
                    Gate gate = scheme.gateController.GetGateByInOut(p, ConnectionType.output);

                    bool? isActive = gate.values[gate.GetIndexOfInOutByCenter(p.CenterPoint, ConnectionType.output)];

                    TryCreate(p.CenterPoint, true, isActive);
                }
            }
        }

        private async void DotTappedEventAsync(Ellipse e)
        {
            NewGateDialog msg = new NewGateDialog();

            await msg.ShowAsync();

            if (msg.gateType != null)
            {
                Gate gate = new Gate(msg.gateType.Value, msg.inputs, msg.outputs, e.GetCenterPoint());
                scheme.gateController.Gates.Add(gate);

                workspaceController.AddGateToGrid(gate);
            }
        }

        private async void LogicGateTappedEvent(Gate gate, Button b) =>
            await new Message(gate.type).ShowAsync();

        private async void GateINTappedEvent(Gate gate, Button b)
        {
            ModeEnum curMode = CurrentMode;

            if (curMode == ModeEnum.addLineMode)
                TryCreate(gate.center, true);
            else if (curMode == ModeEnum.changeValueMode)
            {
                if (gate.values[0] == null)
                    gate.values[0] = true;

                gate.values[0] = !gate.values[0];

                Colorer.SetFillByValue(b, gate.values[0]);
            }
            else
                await new Message(scheme.gateController.GetGateByBody(b).type).ShowAsync();
        }

        private async void GateOUTTappedEvent(Gate gate, Button b)
        {
            if (CurrentMode == ModeEnum.addLineMode)
                TryCreate(gate.center, false);
            else
                await new Message(scheme.gateController.GetGateByBody(b).type).ShowAsync();
        }

        private bool WireCanBeCreated() => newWire != null
            && newWire.Start.X != 0
            && newWire.Start.Y != 0
            && newWire.End.X != 0
            && newWire.End.Y != 0;
        
        private void TryCreate(Point p, bool isStart, bool? value = null)
        {
            if (newWire == null)
                newWire = new Wire();   

            if (isStart)
            {
                newWire.Start = p;
                newWire.isActive = value;
            }
            else
                newWire.End = p;

            if (WireCanBeCreated())
            {
                scheme.lineController.Wires.Add(newWire);

                workspaceController.ShowLines(ref scheme.lineController);

                newWire = null;
            }
        }

        public FrameEnum GetActiveFrame() => currentFrame;

        private async void NewSchemeEventAsync()
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

        private async void LoadSchemeEvent()
        {
            await Serializer.Load(scheme);
            workspaceController.Hide();
            workspaceController.ShowAll(ref scheme);
        }

        private async void SaveSchemeEvent() =>
            await Serializer.Save(scheme);

        private void TraceSchemeEvent()
        {
            int gateCount = scheme.gateController.Gates.Count;

            int wireCount = scheme.lineController.Wires.Count;

            Tracer tracer = new Tracer(gateCount, wireCount);

            tracer.Trace(scheme.gateController, scheme.lineController);

            int[] tracedWireIndexes = tracer.tracedWireIndexes;

            int length = tracedWireIndexes.Length;

            workspaceController.ShowWireTraceIndexes(tracedWireIndexes,
            scheme.lineController);
        }

        private async void WorkSchemeEvent()
        {
            int gateCount = scheme.gateController.Gates.Count;
            int wireCount = scheme.lineController.Wires.Count;

            Tracer tracer = new Tracer(gateCount, wireCount);

            tracer.Trace(scheme.gateController, scheme.lineController);

            scheme.gateController.ClearValuesExcludingIN();
            
            WorkAlgorithmResult Result = WorkAlgorithm.Visualize(scheme);

            if (Result == WorkAlgorithmResult.exInsNotInited)
                await new Message(MessageTypes.exInsNotInited).ShowAsync();
            else if(Result == WorkAlgorithmResult.gatesNotConnected)
                await new Message(MessageTypes.gatesNotConnected).ShowAsync();
            else if(Result == WorkAlgorithmResult.schemeIsntCorrect)
                await new Message(MessageTypes.visualizingFailed).ShowAsync();
            else
            {
                workspaceController.Hide();
                workspaceController.ShowAll(ref scheme);
            }
        }

        private void ChangeModeEvent(BtId obj)
        {
            menuController.InActivateModeButtons();

            if (obj == BtId.addLineBt)
                scheme.frameManager.CurrentMode = ModeEnum.addLineMode;
            else
                scheme.frameManager.CurrentMode = ModeEnum.changeValueMode;
        }

        public void SwitchToFrame(FrameEnum frame, Grid grid)
        {
            currentFrame = frame;

            Rect rect = grid.GetRect();

            menuController.Show();
            menuController.Update(rect);
        }

        public void SizeChanged(Rect rect)
        {
            Grid.SetRect(rect);

            Point menuPoint = Grid.GetLeftTop();
            Size menuSize = new Size(rect.Width, rect.Height / 20);
            Rect menuRect = new Rect(menuPoint, menuSize);

            menuController.Update(menuRect);

            Point workSpaceRectPoint = Grid.GetLeftTop();
            workSpaceRectPoint.Y += menuSize.Height;
            Size workSpaceSize = new Size(rect.Width, rect.Height);
            workSpaceSize.Height -= menuSize.Height;
            Rect workSpaceRect = new Rect(workSpaceRectPoint, workSpaceSize);
            workspaceController.Update(workSpaceRect);
        }
    }
}