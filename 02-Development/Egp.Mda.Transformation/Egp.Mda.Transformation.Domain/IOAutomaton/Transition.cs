using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.IOAutomaton
{
    public class Transition
    {
        public Message InMessage { get; set; }
        public IList<Message> OutMessages { get; set; }
    }
}