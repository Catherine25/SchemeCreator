using System;
using System.Windows;

namespace SchemeSimulator
{
    public static class WireBuilder
    {
        private static Point? End;
        private static Point? Start;

        public static Action<WireView> WireReady;
        
        public static void AddPoint(Point? point, bool isStart)
        {
            if (isStart)
                Start = point;
            else
                End = point;

            TryCreate();
        }

        public static void TryCreate()
        {
            if (Start != null && End != null)
            {
                WireReady(new WireView(Start.Value, End.Value));
                Start = null;
                End = null;
            }
        }
    }
}
