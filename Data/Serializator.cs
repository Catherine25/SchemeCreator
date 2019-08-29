using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;

namespace SchemeCreator.Data
{
    internal static class Serializer
    {
        //data
        public const string gatePath = "gateData.txt", linePath = "lineData.txt";

        public static List<string> serializeGateData, serializeLineData;

        static Serializer()
        {
            serializeGateData = new List<string>();
            serializeLineData = new List<string>();
        }

        public static void SerializeAll(Scheme scheme)
        {
            int gateCount = scheme.gateController.Gates.Count;

            for (int i = 0; i < gateCount; i++)
                serializeGateData.Add(scheme.gateController.Gates[i].SerializeGate());

            int wireCount = scheme.lineController.Wires.Count;

            for (int i = 0; i < wireCount; i++)
                serializeGateData.Add(scheme.lineController.Wires[i].SerializeLine());
        }

        public static void DeserializeAll(Scheme scheme)
        {
            foreach (string s in serializeGateData)
                scheme.gateController.Gates.Add(s.DeserializeGate());

            foreach (string s in serializeLineData)
                scheme.lineController.Wires.Add(s.DeserializeLine());
        }

        //Gate and line serialization and serialization
        public static string SerializeGate(this Gate gi)
        {
            var ms = new MemoryStream();
            // Write an object to the Stream and leave it opened
            using (var writer = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, ownsStream: false))
            {
                var ser = new DataContractSerializer(typeof(Gate));
                ser.WriteObject(writer, gi);
            }
            // Read serialized string from Stream and close it
            using (var reader = new StreamReader(ms, Encoding.UTF8))
            {
                ms.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static string SerializeLine(this Wire w)
        {
            var ms = new MemoryStream();
            // Write an object to the Stream and leave it opened
            using (var writer = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, ownsStream: false))
            {
                var ser = new DataContractSerializer(typeof(Wire));
                ser.WriteObject(writer, w);
            }
            // Read serialized string from Stream and close it
            using (var reader = new StreamReader(ms, Encoding.UTF8))
            {
                ms.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static Gate DeserializeGate(this string xml)
        {
            var ms = new MemoryStream();
            // Write xml content to the Stream and leave it opened
            using (var writer = new StreamWriter(ms, Encoding.UTF8, 512, leaveOpen: true))
            {
                writer.Write(xml);
                writer.Flush();
                ms.Position = 0;
            }
            // Read Stream to the Serializer and Deserialize and close it
            using (var reader = XmlDictionaryReader.CreateTextReader(ms, Encoding.UTF8,
            new XmlDictionaryReaderQuotas(), null))
            {
                var ser = new DataContractSerializer(typeof(Gate));
                return (Gate)ser.ReadObject(reader);
            }
        }

        public static Wire DeserializeLine(this string xml)
        {
            var ms = new MemoryStream();
            // Write xml content to the Stream and leave it opened
            using (var writer = new StreamWriter(ms, Encoding.UTF8, 512, leaveOpen: true))
            {
                writer.Write(xml);
                writer.Flush();
                ms.Position = 0;
            }
            // Read Stream to the Serializer and Deserialize and close it
            using (var reader = XmlDictionaryReader.CreateTextReader(ms, Encoding.UTF8,
            new XmlDictionaryReaderQuotas(), null))
            {
                var ser = new DataContractSerializer(typeof(Wire));
                return (Wire)ser.ReadObject(reader);
            }
        }

        public static async Task Load(Scheme scheme)
        {
            StorageFolder folder = KnownFolders.PicturesLibrary;

            //serialization of gates
            StorageFile gateFile = await folder.GetFileAsync(gatePath);
            serializeGateData.AddRange(await FileIO.ReadLinesAsync(gateFile));

            //serialization of lines
            StorageFile lineFile = await folder.GetFileAsync(linePath);
            serializeLineData.AddRange(await FileIO.ReadLinesAsync(lineFile));

            DeserializeAll(scheme);
        }

        public static async Task Save()
        {
            StorageFolder folder = KnownFolders.PicturesLibrary;

            //serialization of gates
            StorageFile gateFile = await folder.CreateFileAsync(gatePath,
                                                CreationCollisionOption.OpenIfExists);

            await FileIO.WriteLinesAsync(gateFile, serializeGateData);

            //serialization of lines
            StorageFile lineFile = await folder.CreateFileAsync(linePath,
                                              CreationCollisionOption.OpenIfExists);

            await FileIO.WriteLinesAsync(lineFile, serializeLineData);
        }
    }
}