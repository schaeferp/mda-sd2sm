using System.Collections.Generic;
using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.IOAutomaton
{
    public class Automaton
    {
        private readonly IDictionary<string, State> states;

        public Automaton(IParticipant participant)
        {
            Participant = participant;
            states = new Dictionary<string, State>();
        }

        public IParticipant Participant { get; set; }
        public State InitialState { get; set; }

        public ICollection<State> States
        {
            get { return states.Values; }
        }

        public State GetState(string name)
        {
            if (states.ContainsKey(name))
                return states[name];

            return states[name] = new State {Name = name};
        }
    }
}