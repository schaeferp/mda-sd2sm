using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.IOAutomaton
{
    public class Automaton
    {
        public Automaton(IParticipant participant)
        {
            Participant = participant;
        }

        public IParticipant Participant { get; set; }
        public State InitialState { get; set; }
    }
}