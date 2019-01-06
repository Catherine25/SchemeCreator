namespace SchemeCreator.Data
{
    static class NewElementData
    {
        public static int? inputs, id;
        public static string name;

        public static void SetAll(int _in, int _id)
        {
            inputs = _in;
            id = _id;
            name = Scheme.GateNames[_id];
        }
    }
}
