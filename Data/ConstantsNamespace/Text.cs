using System.Collections.Generic;
using System;
using static SchemeCreator.Data.ConstantsNamespace.Constants;

namespace SchemeCreator.Data.ConstantsNamespace
{
    static class Text
    {
        static Text()
        {
            string[] exInsNotInited = new string[4]
            {
                "It's impossible to visualize scheme",
                "You need to set 'true' or 'false' values to all external inputs at first",
                "",
                "Ok"
            };

            SetText(MessageTypes.exInsNotInited, exInsNotInited);

            string[] gatesNotConnected = new string[4]
            {
                "It's impossible to visualize scheme",
                "You need to connect all gates",
                "",
                "Ok"
            };

            SetText(MessageTypes.gatesNotConnected, gatesNotConnected);

            string[] functionIsNotSupportedInfo = new string[4]
            {
                "Error",
                "Function is not supported, it has no effects on scheme",
                "",
                "Ok"
            };

            SetText(MessageTypes.functionIsNotSupported, functionIsNotSupportedInfo);

            string[] modeChanged = new string[4] //TODO: ADD MODE NAMES IN THE MESSAGE
            {
                "Info",
                "Current mode changed",
                "",
                "Ok"
            };

            SetText(MessageTypes.modeChanged, modeChanged);

            string[] newSchemeButtonClicked = new string[4]
            {
                "Do you want to create new scheme?",
                "Current scheme will be lost",
                "Yes",
                "No"
            };

            SetText(MessageTypes.newSchemeButtonClicked, newSchemeButtonClicked);
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
                GetText(mt, MessageAttribute.title),
                GetText(mt, MessageAttribute.text),
                GetText(mt, MessageAttribute.button1),
                GetText(mt, MessageAttribute.button2)
            };

            return messageInfo;
        }

        private static void SetText(MessageTypes mt, MessageAttribute ma, string text) =>
            messagesText.Add(new Tuple<MessageTypes, MessageAttribute>(mt, ma), text);

        private static void SetText(MessageTypes mt, string[] messageInfo)
        {
            SetText(mt, MessageAttribute.title, messageInfo[0]);
            SetText(mt, MessageAttribute.text, messageInfo[1]);
            SetText(mt, MessageAttribute.button1, messageInfo[2]);
            SetText(mt, MessageAttribute.button2, messageInfo[3]);
        }


        private static IDictionary<Tuple<MessageTypes, MessageAttribute>, string> messagesText =
            new Dictionary<Tuple<MessageTypes, MessageAttribute>, string>();
    }
}
