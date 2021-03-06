// using System.Collections.Generic;
// using System.Runtime.Serialization;
// using Windows.UI.Xaml;
// using SchemeCreator.Data.Interfaces;
// using static SchemeCreator.Data.Constants;
// using System.Linq;
// using System;
// using SchemeCreator.Data.Models.Enums;
// using SchemeCreator.UI.Dynamic;
// using System.Numerics;

// namespace SchemeCreator.Data.Models
// {
//     [DataContract]
//     public class Gate : IGridChild
//     {
//         [DataMember]
//         public GateEnum Type;

//         [DataMember]
//         public int Inputs;

//         [DataMember]
//         public int Outputs;

//         [DataMember]
//         public Vector2 MatrixIndex;

//         [DataMember]
//         public List<bool?> Values;

//         public Gate(GateEnum type, int inputs, int outputs, Vector2 point)
//         {
//             Type = type;
//             Inputs = inputs;
//             Outputs = outputs;
//             MatrixIndex = point;

//             Values = new List<bool?>(inputs);

//             for (int i = 0; i < inputs; i++)
//                 Values.Add(null);

//             ProcessData = Services.GateWorkPatterns.ActionByType[type];
//         }

//         public Vector3 GetLeftTop()
//         {
//             if(external.Contains(Type))
//             {
//                 return new Vector3
//                 {
//                     X = (float)(MatrixIndex.X - externalGateSize.Width / 2),
//                     Y = (float)(MatrixIndex.Y - externalGateSize.Height / 2)
//                 };
//             }
//             else
//             {
//                 return new Vector3
//                 {
//                     X = (float)(MatrixIndex.X - logicGateSize.Width / 2),
//                     Y = (float)(MatrixIndex.Y - logicGateSize.Height / 2)
//                 };
//             }
            
//         }

//         public void Work() { ProcessData(this); }
//         private readonly Action<Gate> ProcessData;

//         public GateBody DrawBody() => new GateBody(this);

//         public List<GatePortView> DrawPorts(ConnectionTypeEnum type)
//         {
//             if(type == ConnectionTypeEnum.Both)
//             {
//                 var items = DrawPorts(ConnectionTypeEnum.Input);
//                 items.AddRange(DrawPorts(ConnectionTypeEnum.Output));
//                 return items;
//             }
//             else
//             {
//                 List<GatePortView> ports = new List<GatePortView>();
//                 int length = type == ConnectionTypeEnum.Input ? Inputs : Outputs;

//                 for (int i = 0; i < length; i++)
//                 {
//                     GatePortView port = new GatePortView(type);

//                     Vector3 center =
//                         type == ConnectionTypeEnum.Input
//                         ? new Vector3(GetLeftTop().X, (float)(GetLeftTop().Y + (logicGateSize.Height / (length + 1) * (i + 1))), 0)
//                         : new Vector3((float)(GetLeftTop().X + logicGateSize.Width), (float)(GetLeftTop().Y + (logicGateSize.Height / (length + 1) * (i + 1))), 0);

//                     port.CenterPoint = center;

//                     port.Value = Values[i];

//                     ports.Add(port);
//                 }

//                 return ports;
//             }            
//         }

//         public bool ContainsInOutByCenter(Vector3 center, ConnectionTypeEnum type) =>
//             DrawPorts(type).Exists(x => x.CenterPoint == center);

//         public bool ContainsBodyByMargin(Thickness t) =>
//             DrawBody().ContainsBodyByMargin(t);

//         public int GetIndexOfInOutByCenter(Vector3 center, ConnectionTypeEnum type) =>
//             DrawPorts(type).FindIndex(x => x.CenterPoint == center);

//         public GateBody GetBodyByWirePart(Vector3 p)
//         {
//             if (Type == GateEnum.IN || Type == GateEnum.OUT)
//                 if (p == MatrixIndex)
//                     return DrawBody();

//             return null;
//         }

//         public GatePortView GetInOutByWirePart(Vector3 p)
//         {
//             var items = DrawPorts(ConnectionTypeEnum.Both);
//             return items.FirstOrDefault(i => i.CenterPoint == p);
//         }

//         public bool WireConnects(Vector3 point)
//         {
//             if (external.Contains(Type))
//                 return MatrixIndex == point;
//             else
//             {
//                 var items = DrawPorts(ConnectionTypeEnum.Both);
//                 return items.Any(i => i.CenterPoint == point);
//             }
//         }

//         public int FirstFreeValueBoxIndex()
//         {
//             for (int i = 0; i < Inputs; i++)
//                 if (Values[i] == null)
//                     return i;

//             return -1;
//         }

//         public void AddToParent(SmartGrid parent) =>
//             DrawBody().AddToParent(parent);
//     }
// }