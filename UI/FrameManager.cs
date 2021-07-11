using SchemeCreator.Data.Controllers;
using SchemeCreator.Data.Models;
using SchemeCreator.Data.Models.Enums;
using SchemeCreator.Data.Services;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using static SchemeCreator.Data.Constants;
using Type = SchemeCreator.UI.Dynamic.PortType;

namespace SchemeCreator.UI
{
    public class FrameManager
    {
        private readonly WorkspaceController workspaceController;

        //private readonly MenuController menuController;

        //private ModeEnum CurrentMode;

        private SchemeView scheme;

        private FrameEnum currentFrame;

        private WireView newWire;

        private Stack<ConnectionPair> connections = new Stack<ConnectionPair>();

        public Grid MainGrid { get; }

        //public FrameManager(Scheme _scheme)
        //{
        //    scheme = _scheme;

        //    newWire = null;

        //    workspaceController = new WorkspaceController();
        //    //menuController = new MenuController();
        //    MainGrid = new Grid();

        //    //menuController.SetParentGrid(MainGrid);
        //    workspaceController.SetParentGrid(MainGrid);

        //    SwitchToFrame(FrameEnum.workspace, MainGrid);

        //    EventSubscribe();
        //}

        /*      methods     */

        //private void EventSubscribe()
        //{
        //    //menuController.NewSchemeBtClickEvent += NewSchemeEventAsync;
        //    //menuController.LoadSchemeBtClickEvent += LoadSchemeEvent;
        //    //menuController.SaveSchemeBtClickEvent += SaveSchemeEvent;
        //    //menuController.TraceSchemeBtClickEvent += TraceSchemeEvent;
        //    //menuController.WorkSchemeBtClickEvent += WorkSchemeEvent;
        //    //menuController.ChangeModeEvent += ChangeModeEvent;

        //    workspaceController.DotTappedEvent += DotTappedEventAsync;
        //    workspaceController.PortTapped += PortTapped;
        //    workspaceController.LogicGateTappedEvent += LogicGateTappedEvent;
        //    workspaceController.ExternalPortViewTappedEvent += ExternalPortViewTappedEvent;
        //    workspaceController.LineTappedEvent += WorkspaceController_LineTappedEvent;
        //}

        //private void ExternalPortViewTappedEvent(ExternalPortView obj)
        //{
        //    if (obj.Type == Type.Input)
        //    {
        //        ModeEnum curMode = CurrentMode;

        //        if (curMode == ModeEnum.addLineMode)
        //            TryCreate(obj, true);
        //        else if (curMode == ModeEnum.changeValueMode)
        //        {
        //            if (obj.Value == null)
        //                obj.Value = true;

        //            obj.Value = !obj.Value;
        //        }
        //        //else
        //            //await new Message(scheme.gateController.GetGateByBody(body).Type).ShowAsync();
        //    }
        //    else
        //    {
        //        if (CurrentMode == ModeEnum.addLineMode)
        //            TryCreate(obj, false);
        //        //else
        //            //await new Message(scheme.gateController.GetGateByBody(body).Type).ShowAsync();
        //    }
        //}

        //private void WorkspaceController_LineTappedEvent(Line line)
        //{
        //    scheme.Wires.Remove(scheme.FindWireByLine(line));
        //    workspaceController.RemoveLine(line);
        //}

        //TODO
        //private void PortTapped(GatePortView p, GateView gate)
        //{
        //    if (p.Type == ConnectionTypeEnum.Input && CurrentMode == ModeEnum.addLineMode)
        //        TryCreate(gate, p, false);
        //    else if (CurrentMode == ModeEnum.addLineMode)
        //        TryCreate(gate, p, true, p.Value);
        //}

        //private async void DotTappedEventAsync(Ellipse e, Vector2 point)
        //{
        //    NewGateDialog msg = new NewGateDialog();

        //    await msg.ShowAsync();

        //    if (msg.gateType != null)
        //    {
        //        //TODO here e.CenterPoint -> row number
        //        GateView gate = new GateView(msg.gateType.Value, point, msg.inputs, msg.outputs);
        //        scheme.gateController.Gates.Add(gate);

        //        workspaceController.AddGateToGrid(gate);
        //    }
        //}

        //TODO
        //private async void LogicGateTappedEvent(GateBodyView body, GateView gate) =>
        //    await new Message(gate.Type).ShowAsync();

        //private async void GateINTappedEvent(GateView gate, Button body) {}

        //private async void GateOUTTappedEvent(GateView gate, Button body){}

        //private bool WireCanBeCreated() => newWire != null
        //    && newWire.Start.X != 0
        //    && newWire.Start.Y != 0
        //    && newWire.End.X != 0
        //    && newWire.End.Y != 0;
        
        //private void TryCreate(Vector2 p, bool isStart, bool? value = null)
        //{
        //    if (newWire == null)
        //        newWire = new Wire();   

        //    if (isStart)
        //    {
        //        newWire.Start = p;
        //        newWire.isActive = value;
        //    }
        //    else
        //        newWire.End = p;

        //    if (WireCanBeCreated())
        //    {
        //        scheme.lineController.Wires.Add(newWire);

        //        workspaceController.ShowLines(ref scheme.lineController);

        //        newWire = null;
        //    }
        //}

        private void TryCreate(ExternalPortView port, bool isStart, bool? value = null)
        {
            if (connections.TryPop(out ConnectionPair pair))
            {
                if(pair.Start != null && pair.End != null)
                    connections.Push(pair);

                if (isStart)
                {
                    pair.Start.Clear();
                    pair.Start.externalPort = port;
                    pair.Start.externalPort.Value = value;
                }
                else
                {
                    pair.End.Clear();
                    pair.End.externalPort = port;
                }
            }
            else
            {
                throw new NotImplementedException();

                //ConnectionPair newPair = new ConnectionPair();

                //if (isStart)
                //{
                //    newPair.Start = new ConnectionPair { externalPort = port};
                //    newPair.Value = value;
                //}
                //else
                //    newPair.End = new ConnectionPair { externalPort = port};
                
                //connections.Push(newPair);
            }
        }
        private void TryCreate(GateView gate, GatePortView gatePort, bool isStart, bool? value = null)
        {
            throw new NotImplementedException();
            //if (connections.TryPop(out ConnectionPair pair))
            //{
            //    if (pair.Start != null && pair.End != null)
            //        connections.Push(pair);

            //    if (isStart)
            //    {
            //        pair.Start.Clear();
            //        pair.Start.gate = gate;
            //        pair.Start.port = gatePort;
            //        pair.Start.port.Value = value;
            //    }
            //    else
            //    {
            //        pair.End.Clear();
            //        pair.End.gate = gate;
            //        pair.End.port = gatePort;
            //    }
            //}
            //else
            //{
            //    ConnectionPair newPair = new ConnectionPair();

            //    if (isStart)
            //    {
            //        newPair.Start = new Connection
            //        {
            //            gate = gate,
            //            port = gatePort
            //        };

            //        newPair.Value = value;
            //    }
            //    else
            //        newPair.End = new Connection
            //        {
            //            gate = gate,
            //            port = gatePort
            //        };

            //    connections.Push(newPair);
            //}
        }

        public FrameEnum GetActiveFrame() => currentFrame;

        //private async void NewSchemeEventAsync()
        //{
        //    if (scheme.gateController.Gates.Count == 0
        //        && scheme.lineController.Wires.Count == 0)
        //    {
        //        workspaceController.Hide();
        //        workspaceController.ShowAll(ref scheme);
        //    }
        //    else
        //    {
        //        ContentDialogResult result = await new Message(MessageTypes.newSchemeButtonClicked).ShowAsync();

        //        if (result == ContentDialogResult.Primary)
        //        {
        //            scheme.gateController.Gates.Clear();
        //            scheme.lineController.Wires.Clear();

        //            workspaceController.Hide();
        //            workspaceController.ShowAll(ref scheme);
        //        }
        //    }
        //}

        //private async void LoadSchemeEvent()
        //{
        //    await Serializer.Load(scheme);
        //    workspaceController.Hide();
        //    workspaceController.ShowAll(ref scheme);
        //}

        //private async void SaveSchemeEvent() =>
        //    await Serializer.Save(scheme);

        //private void TraceSchemeEvent()
        //{
        //    Tracer tracer = new Tracer();

        //    tracer.Trace(scheme.gateController, scheme.lineController);

        //    //int[] tracedWireIndexes = tracer.tracedWireIndexes;

        //    //int length = tracedWireIndexes.Length;

        //    //TODO
        //    //workspaceController.ShowWireTraceIndexes(tracedWireIndexes,scheme.lineController);
        //}

        //private async void WorkSchemeEvent()
        //{
        //    int gateCount = scheme.gateController.Gates.Count;
        //    int wireCount = scheme.lineController.Wires.Count;

        //    Tracer tracer = new Tracer();

        //    tracer.Trace(scheme.gateController, scheme.lineController);

        //    scheme.gateController.ResetGates();
            
        //    WorkAlgorithmResult Result = WorkAlgorithm.Visualize(scheme, tracer.TraceHistory);

        //    if (Result == WorkAlgorithmResult.exInsNotInited)
        //        await new Message(MessageTypes.exInsNotInited).ShowAsync();
        //    else if(Result == WorkAlgorithmResult.gatesNotConnected)
        //        await new Message(MessageTypes.gatesNotConnected).ShowAsync();
        //    else if(Result == WorkAlgorithmResult.schemeIsntCorrect)
        //        await new Message(MessageTypes.visualizingFailed).ShowAsync();
        //    else
        //    {
        //        workspaceController.Hide();
        //        workspaceController.ShowAll(ref scheme);
        //    }
        //}

        //private void ChangeModeEvent(BtId obj)
        //{
        //    menuController.InActivateModeButtons();

        //    if (obj == BtId.addLineBt)
        //        scheme.frameManager.CurrentMode = ModeEnum.addLineMode;
        //    else
        //        scheme.frameManager.CurrentMode = ModeEnum.changeValueMode;
        //}

        //public void SwitchToFrame(FrameEnum frame, Grid grid)
        //{
        //    currentFrame = frame;

        //    //Rect rect = grid.GetRect();

        //    menuController.Show();
        //}

        //public void SizeChanged(Rect rect)
        //{
        //    MainGrid.SetRect(rect);

        //    Point menuPoint = MainGrid.GetLeftTop();
        //    Size menuSize = new Size(rect.Width, rect.Height / 20);
        //    Rect menuRect = new Rect(menuPoint, menuSize);

        //    menuController.Update(menuRect);

        //    Point workSpaceRectPoint = MainGrid.GetLeftTop();
        //    workSpaceRectPoint.Y += menuSize.Height;
        //    Size workSpaceSize = new Size(rect.Width, rect.Height);
        //    workSpaceSize.Height -= menuSize.Height;
        //    Rect workSpaceRect = new Rect(workSpaceRectPoint, workSpaceSize);
        //    workspaceController.Update(workSpaceRect);
        //}
    }
}