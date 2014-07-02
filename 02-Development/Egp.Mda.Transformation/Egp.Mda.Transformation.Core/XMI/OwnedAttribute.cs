using System;

namespace Egp.Mda.Transformation.Core
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
