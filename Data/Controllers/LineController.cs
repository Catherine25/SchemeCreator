﻿using System.Collections.Generic;

namespace SchemeCreator.Data
{
    public class LineController
    {
        //data
        private List<Wire> wires;

        public List<Wire> Wires
        {
            get => wires;
            set => wires = value;
        }

        //constructor
        public LineController() => wires = new List<Wire>();

        // public void colorLineByValues(Line l, bool? value) {
        //     if (value == true)
        //         l.Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.light1];
        //     else if (value == false)
        //         l.Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.dark1];
        //     l.UpdateLayout();
        // }

        // public void reloadLines() {
        //     foreach (Wire w in Wires)
        //         Wires.Add(new Wire {
        //             start = w.start,
        //             end = w.end });
        // }
    }
}