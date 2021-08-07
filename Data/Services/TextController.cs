using System.Collections.Generic;
using System;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Services
{
    static class TextController
    {
        static TextController()
        {
            string[] exInsNotInited = new string[4]
            {
                "It's impossible to visualize scheme",
                "You need to set 'true' or 'false' values to all external inputs at first",
                "",
                "Ok"
            };

            SetText(MessageTypes.ExInsNotInited, exInsNotInited);

            string[] gatesNotConnected = new string[4]
            {
                "It's impossible to visualize scheme",
                "You need to connect all gates",
                "",
                "Ok"
            };

            SetText(MessageTypes.GatesNotConnected, gatesNotConnected);

            string[] functionIsNotSupportedInfo = new string[4]
            {
                "Error",
                "Function is not supported, it has no effects on scheme",
                "",
                "Ok"
            };

            SetText(MessageTypes.FunctionIsNotSupported, functionIsNotSupportedInfo);

            string[] modeChanged = new string[4]
            {
                "Info",
                "Current mode changed",
                "",
                "Ok"
            };

            SetText(MessageTypes.ModeChanged, modeChanged);

            string[] newSchemeButtonClicked = new string[4]
            {
                "Do you want to create new scheme?",
                "Current scheme will be lost",
                "Yes",
                "No"
            };

            SetText(MessageTypes.NewSchemeButtonClicked, newSchemeButtonClicked);

            string[] visualizingFailed = new string[4]
            {
                "It's impossible to visualize scheme",
                "You have some feedbacks in the scheme",
                "",
                "Ok"
            };

            SetText(MessageTypes.VisualizingFailed, visualizingFailed);
        }

        public static string GetText(MessageTypes mt, MessageAttribute ma)
        {
            Tuple<MessageTypes, MessageAttribute> tuple = new Tuple<MessageTypes, MessageAttribute>(mt, ma);

            return messagesText[tuple];
        }

        public static string[] GetText(MessageTypes mt)
        {
            string[] messageInfo = new string[4]
            {
                GetText(mt, MessageAttribute.Title),
                GetText(mt, MessageAttribute.Text),
                GetText(mt, MessageAttribute.Button1),
                GetText(mt, MessageAttribute.Button2)
            };

            return messageInfo;
        }

        private static void SetText(MessageTypes mt, MessageAttribute ma, string text) =>
            messagesText.Add(new Tuple<MessageTypes, MessageAttribute>(mt, ma), text);

        private static void SetText(MessageTypes mt, string[] messageInfo)
        {
            SetText(mt, MessageAttribute.Title, messageInfo[0]);
            SetText(mt, MessageAttribute.Text, messageInfo[1]);
            SetText(mt, MessageAttribute.Button1, messageInfo[2]);
            SetText(mt, MessageAttribute.Button2, messageInfo[3]);
        }

        private static readonly IDictionary<Tuple<MessageTypes, MessageAttribute>, string> messagesText =
            new Dictionary<Tuple<MessageTypes, MessageAttribute>, string>();

        public static string BuildButtonBodyText(GateEnum type, int inputs, int outputs)
        {
            string s = type.ToString();

            if (!SingleOutput.Contains(type))
                s += "\n" + inputs.ToString() + " in " + outputs.ToString();

            return s;
        }
    }
}
