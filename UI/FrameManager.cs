using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using static SchemeCreator.Data.ConstantsNamespace.Constants;

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

        public Grid grid { get; }


        public FrameManager(Scheme _scheme)
        {
            scheme = _scheme;

            workspaceController = new WorkspaceController();
            menuController = new MenuController();

            grid = new Grid();
            grid.SetStandartAlighnment();

            menuController.SetParentGrid(grid);
            workspaceController.SetParentGrid(grid);

            SwitchToFrame(FrameEnum.workspace, grid);

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
            workspaceController.LogicGateInTapped += LogicGateInTapped;
            workspaceController.LogicGateOutTapped += LogicGateOutTapped;
            workspaceController.LogicGateTappedEvent += LogicGateTappedEvent;
            workspaceController.GateINTapped += GateINTappedEvent;
            workspaceController.GateOUTTapped += GateOUTTappedEvent;
            workspaceController.LineTappedEvent += WorkspaceController_LineTappedEvent;
        }

        private void WorkspaceController_LineTappedEvent(Line line)
        {
            if (CurrentMode == ModeEnum.removeLineMode)
            {
                scheme.lineController.Wires.Remove(scheme.lineController.FindWireByLine(line));
                workspaceController.RemoveLine(line);
            }
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

            if (msg.gateType != null)
            {
                Gate gate = new Gate(msg.gateType.Value, msg.inputs, msg.outputs, e.GetCenterPoint());
                scheme.gateController.Gates.Add(gate);

                workspaceController.AddGateToGrid(gate);
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
        private readonly WorkspaceController workspaceController;

        private readonly MenuController menuController;

        private ModeEnum CurrentMode;

        private Scheme scheme;

        private FrameEnum currentFrame;

        private Wire newWire;

        public Grid grid { get; }

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

            if (obj == BtId.addGateBt)
                scheme.frameManager.CurrentMode = ModeEnum.addGateMode;
            else if (obj == BtId.addLineBt)
                scheme.frameManager.CurrentMode = ModeEnum.addLineStartMode;
            else if (obj == BtId.removeLineBt)
                scheme.frameManager.CurrentMode = ModeEnum.removeLineMode;
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
            grid.SetRect(rect);

            Point menuPoint = grid.GetLeftTop();
            Size menuSize = new Size(rect.Width, rect.Height / 20);
            Rect menuRect = new Rect(menuPoint, menuSize);

            menuController.Update(menuRect);

            Point workSpaceRectPoint = grid.GetLeftTop();
            workSpaceRectPoint.Y += menuSize.Height;
            Size workSpaceSize = new Size(rect.Width, rect.Height);
            workSpaceSize.Height -= menuSize.Height;
            Rect workSpaceRect = new Rect(workSpaceRectPoint, workSpaceSize);
            workspaceController.Update(workSpaceRect);
        }
    }
}