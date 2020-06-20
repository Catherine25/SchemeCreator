using Newtonsoft.Json;
using SchemeCreator.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace SchemeCreator.Data.Services
{
    static class Serializer
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

        public static async Task Load(Scheme scheme)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            //serialization of gates
            StorageFile gateFile = await folder.GetFileAsync(gatePath);

            if (gateFile == null)
                await folder.CreateFileAsync(gatePath);

            scheme.gateController.Gates.Clear();
            scheme.gateController.Gates = JsonConvert.DeserializeObject<List<Gate>>(await FileIO.ReadTextAsync(gateFile));

            //serialization of wires
            StorageFile lineFile = await folder.GetFileAsync(linePath);

            if (lineFile == null)
                await folder.CreateFileAsync(linePath);

            scheme.lineController.Wires.Clear();
            scheme.lineController.Wires = JsonConvert.DeserializeObject<List<Wire>>(await FileIO.ReadTextAsync(lineFile));
        }

        public static async Task Save(Scheme scheme)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            //serialization of gates
            StorageFile gateFile = await folder.CreateFileAsync(gatePath,
                                                CreationCollisionOption.OpenIfExists);

            await FileIO.WriteTextAsync(gateFile, JsonConvert.SerializeObject(scheme.gateController.Gates));

            //serialization of wires
            StorageFile lineFile = await folder.CreateFileAsync(linePath,
                                              CreationCollisionOption.OpenIfExists);

            await FileIO.WriteTextAsync(lineFile, JsonConvert.SerializeObject(scheme.lineController.Wires));
        }
    }
}