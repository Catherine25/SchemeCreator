using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using SchemeCreator.UI.Dynamic;
using SchemeCreator.UI;
using System.Linq;

namespace SchemeCreator.Data.Services.Serialization
{
    static partial class Serializer
    {
        private const string GatePath = "gateData.txt";
        private const string WirePath = "wireData.txt";
        private const string PortPath = "portData.txt";
        
        public static async Task Load(SchemeView scheme)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            scheme.Recreate();

            //serialization of gates
            StorageFile gateFile = await folder.CreateFileAsync(GatePath, CreationCollisionOption.OpenIfExists);

            var gates = JsonConvert.DeserializeObject<List<GateDto>>(await FileIO.ReadTextAsync(gateFile));
            var gatesToAdd = new List<GateView>(gates.Select(g => new GateView(g.Type, g.Location, g.Inputs, g.Outputs)));
            gatesToAdd.ForEach(scheme.AddToView);

            // serialization of ports
            StorageFile portFile = await folder.CreateFileAsync(PortPath, CreationCollisionOption.OpenIfExists);

            var ports = JsonConvert.DeserializeObject<List<PortDto>>(await FileIO.ReadTextAsync(portFile));
            var portsToAdd = new List<ExternalPortView>(ports.Select(p => new ExternalPortView(p.Type, p.Location)));
            portsToAdd.ForEach(scheme.AddToView);

            //serialization of wires
            StorageFile wireFile = await folder.CreateFileAsync(WirePath, CreationCollisionOption.OpenIfExists);

            var wires = JsonConvert.DeserializeObject<List<WireDto>>(await FileIO.ReadTextAsync(wireFile));
            var wiresToAdd = new List<WireView>(wires.Select(w =>
            {
                WireView wireView = new();
                wireView.SetConnection(w.Connection);
                return wireView;
            }));
            wiresToAdd.ForEach(scheme.AddToView);
        }

        public static async Task Save(SchemeView scheme)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            var serialized = PrepareForSerialization(scheme);

            await Save(folder, GatePath, JsonConvert.SerializeObject(serialized.gates));
            await Save(folder, PortPath, JsonConvert.SerializeObject(serialized.ports));
            await Save(folder, WirePath, JsonConvert.SerializeObject(serialized.wires));
        }

        private static async Task Save(StorageFolder folder, string path, string serializedData)
        {
            StorageFile file = await folder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, serializedData);
        }

        /// <summary>
        /// Prepares Scheme components for serialization.
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        private static (IEnumerable<GateDto> gates, IEnumerable<PortDto> ports, IEnumerable<WireDto> wires) PrepareForSerialization(SchemeView scheme)
        {
            var gates = scheme.Gates.Select(gate => new GateDto(gate));
            var ports = scheme.ExternalPorts.Select(port => new PortDto(port));
            var wires = scheme.Wires.Select(wire => new WireDto(wire));

            return (gates, ports, wires);
        }
    }
}