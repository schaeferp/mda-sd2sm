using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class IOTransition
    {
        public IOTransition()
        {
            OutMessages = new List<MessageTriple>();
        }

        public IOState Target { get; set; }
        public MessageTriple InMessageTriple { get; set; }
        public IList<MessageTriple> OutMessages { get; set; }
    }
}