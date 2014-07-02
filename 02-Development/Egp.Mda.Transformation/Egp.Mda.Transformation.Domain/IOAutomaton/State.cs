using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.IOAutomaton
{
    public class State
    {
        public string Name { get; set; }
        public IEnumerable<Transition> Outgoing { get; set; }
    }
}