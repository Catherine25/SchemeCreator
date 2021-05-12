using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using SchemeCreator.Data.Models;
using SchemeCreator.UI.Dynamic;
using SchemeCreator.UI;
using Windows.Foundation;
using static SchemeCreator.Data.Constants;
using System.Numerics;
using System.Linq;

namespace SchemeCreator.Data.Services.Serialization
{
    static partial class Serializer
    {
        public const string gatePath = "gateData.txt";
        public const string linePath = "lineData.txt";

        public static List<string> serializeGateData;
        public static List<string> serializeLineData;

        static Serializer()
        {
            serializeGateData = new List<string>();
            serializeLineData = new List<string>();
        }

        public static async Task Load(SchemeView scheme)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            scheme.Recreate();

            //serialization of gates
            StorageFile gateFile = await folder.GetFileAsync(gatePath);

            if (gateFile == null)
                await folder.CreateFileAsync(gatePath);

            var gates = JsonConvert.DeserializeObject<List<Gate>>(await FileIO.ReadTextAsync(gateFile));
            var gatesToAdd = new List<GateView>(gates.Select(g => new GateView(g.Type, g.Location, g.Inputs, g.Outputs)));
            gatesToAdd.ForEach(g => scheme.AddToView(g));

            //serialization of wires
            StorageFile lineFile = await folder.GetFileAsync(linePath);

            if (lineFile == null)
                await folder.CreateFileAsync(linePath);

            var wires = JsonConvert.DeserializeObject<List<Wire>>(await FileIO.ReadTextAsync(lineFile));
            var wiresToAdd = new List<WireView>(wires.Select(w => new WireView(w.Start, w.End)));
            wiresToAdd.ForEach(w => scheme.AddToView(w));
        }

        public static async Task Save(SchemeView scheme)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            //serialization of gates
            StorageFile gateFile = await folder.CreateFileAsync(gatePath, CreationCollisionOption.OpenIfExists);

            await FileIO.WriteTextAsync(gateFile, JsonConvert.SerializeObject(scheme.Gates.Select(g => new Gate(g))));

            //serialization of wires
            StorageFile lineFile = await folder.CreateFileAsync(linePath, CreationCollisionOption.OpenIfExists);

            await FileIO.WriteTextAsync(lineFile, JsonConvert.SerializeObject(scheme.Wires.Select(w => new Wire(w))));
        }
    }
}