using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.IOAutomaton
{
    public class Transition
    {
        public Message InMessage { get; set; }
        public IEnumerable<Message> OutMessages { get; set; }
    }
}