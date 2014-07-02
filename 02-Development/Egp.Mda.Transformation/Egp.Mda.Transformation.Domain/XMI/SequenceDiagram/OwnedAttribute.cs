using System;

namespace Egp.Mda.Transformation.Domain.Xmi.SequenceDiagram
{
    public class OwnedAttribute
    {
        public string XmiId { get; set; }
        public string XmiType { get; set; }
        public string Name { get; set; }
        public String Type { get; set; }

        public override string ToString()
        {
            return XmiId;
        }
    }
}