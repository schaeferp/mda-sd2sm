using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.IOAutomaton
{
    public class Transition
    {
        public Transition()
        {
            OutMessages = new List<Message>();
        }

        public State Target { get; set; }
        public Message InMessage { get; set; }
        public IList<Message> OutMessages { get; set; }
    }
}