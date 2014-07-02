using System.Collections.Generic;
using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.IOAutomaton
{
    public class Transition
    {
        public Transition()
        {
            OutMessages = new List<MessageTriple>();
        }

        public State Target { get; set; }
        public MessageTriple InMessageTriple { get; set; }
        public IList<MessageTriple> OutMessages { get; set; }
    }
}