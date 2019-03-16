using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Windows.Storage;
using System;
using System.Threading.Tasks;

namespace SchemeCreator.Data {
    static class Serializer {
        //data
        public const string gatePath = "gateData.txt", linePath = "lineData.txt";
        public static List<string> serializeGateData, serializeLineData;

        static Serializer() {
            serializeGateData = new List<string>();
            serializeLineData = new List<string>();
        }

        public static void SerializeAll(Scheme scheme) {
            foreach (GateInfo gi in scheme.gateController.gateInfo)
                serializeGateData.Add(gi.SerializeGate());

            foreach (LineInfo li in scheme.lineController.lineInfo)
                serializeLineData.Add(li.SerializeLine());
        }

        public static void DeserializeAll(Scheme scheme) {
            //scheme.gateController.gateInfo.Clear();
            //scheme.lineController.lineInfo.Clear();

            foreach (string s in serializeGateData)
                scheme.gateController.gateInfo.Add(s.DeserializeGate());

            foreach (string s in serializeLineData)
                scheme.lineController.lineInfo.Add(s.DeserializeLine());
        }

        //Gate and line serialization and deserialization
        public static string SerializeGate(this GateInfo gi) {
            var ms = new MemoryStream();
            // Write an object to the Stream and leave it opened
            using (var writer = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, ownsStream: false)) {
                var ser = new DataContractSerializer(typeof(GateInfo));
                ser.WriteObject(writer, gi);
            }
            // Read serialized string from Stream and close it
            using (var reader = new StreamReader(ms, Encoding.UTF8)) {
                ms.Position = 0;
                return reader.ReadToEnd();
            }
        }
        public static string SerializeLine(this LineInfo li) {
            var ms = new MemoryStream();
            // Write an object to the Stream and leave it opened
            using (var writer = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, ownsStream: false)) {
                var ser = new DataContractSerializer(typeof(LineInfo));
                ser.WriteObject(writer, li);
            }
            // Read serialized string from Stream and close it
            using (var reader = new StreamReader(ms, Encoding.UTF8)) {
                ms.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static GateInfo DeserializeGate(this string xml) {
            var ms = new MemoryStream();
            // Write xml content to the Stream and leave it opened
            using (var writer = new StreamWriter(ms, Encoding.UTF8, 512, leaveOpen: true)) {
                writer.Write(xml);
                writer.Flush();
                ms.Position = 0;
            }
            // Read Stream to the Serializer and Deserialize and close it
            using (var reader = XmlDictionaryReader.CreateTextReader(ms, Encoding.UTF8,
            new XmlDictionaryReaderQuotas(), null)) {
                var ser = new DataContractSerializer(typeof(GateInfo));
                return (GateInfo)ser.ReadObject(reader);
            }
        }

        public static LineInfo DeserializeLine(this string xml) {
            var ms = new MemoryStream();
            // Write xml content to the Stream and leave it opened
            using (var writer = new StreamWriter(ms, Encoding.UTF8, 512, leaveOpen: true)) {
                writer.Write(xml);
                writer.Flush();
                ms.Position = 0;
            }
            // Read Stream to the Serializer and Deserialize and close it
            using (var reader = XmlDictionaryReader.CreateTextReader(ms, Encoding.UTF8,
            new XmlDictionaryReaderQuotas(), null)) {
                var ser = new DataContractSerializer(typeof(LineInfo));
                return (LineInfo)ser.ReadObject(reader);
            }
        }

        public static async Task Load(Scheme scheme) {
            StorageFolder folder = KnownFolders.PicturesLibrary;

            //deserialization of gates
            StorageFile gateFile = await folder.GetFileAsync(gatePath);
            serializeGateData.AddRange(await FileIO.ReadLinesAsync(gateFile));

            //deserialization of lines
            StorageFile lineFile = await folder.GetFileAsync(linePath);
            serializeLineData.AddRange(await FileIO.ReadLinesAsync(lineFile));

            DeserializeAll(scheme);
        }

        public static async Task Save() {
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
