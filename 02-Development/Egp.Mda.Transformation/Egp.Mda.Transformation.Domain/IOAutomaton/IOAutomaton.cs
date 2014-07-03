using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class IOAutomaton
    {
        private readonly IDictionary<string, IOState> _states;

        public IOAutomaton(IParticipant participant)
        {
            Participant = participant;
            _states = new Dictionary<string, IOState>();
        }

        public IParticipant Participant { get; set; }
        public IOState InitialState { get; set; }

        public ICollection<IOState> States
        {
            get { return _states.Values; }
        }

        public IOState GetState(string name)
        {
            if (_states.ContainsKey(name))
                return _states[name];

            return _states[name] = new IOState {Name = name};
        }
    }
}