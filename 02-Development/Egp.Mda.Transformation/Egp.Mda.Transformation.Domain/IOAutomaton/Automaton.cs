using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.IOAutomaton
{
    public class Automaton
    {
        public IParticipant Participant { get; set; }
        public State InitialState { get; set; }
    }
}