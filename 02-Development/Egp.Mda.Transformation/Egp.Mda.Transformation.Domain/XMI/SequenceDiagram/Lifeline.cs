﻿namespace Egp.Mda.Transformation.Domain.Xmi.SequenceDiagram
{
    public class Lifeline
    {
        public string XmiId { get; set; }
        public string XmiType { get; set; }
        public string Represents { get; set; }

        public override string ToString()
        {
            return XmiId;
        }
    }
}