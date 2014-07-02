using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain.Behavior;
using Egp.Mda.Transformation.Domain.IOAutomaton;
using Message = Egp.Mda.Transformation.Domain.IOAutomaton.Message;

namespace Egp.Mda.Transformation.Core.IOAutomaton
{
    public class AutomatonService : IAutomatonService
    {
        public IEnumerable<Automaton> From(IEnumerable<Context> contexts)
        {
            return contexts.Select(TransformContext);
        }

        private Automaton TransformContext(Context context)
        {
            var states = new Dictionary<string, State>();
            var automaton = new Automaton(context.Participant);

            if (context.Scenarios.Count > 0)
            {
                string initialStateName = context.Scenarios[0].Behaviors[0].PreState;
                automaton.InitialState = getOrCreateState(initialStateName, states);
                context.Scenarios.ForEach(s => TransformScenario(s, states));
            }

            return automaton;
        }

        private void TransformScenario(Scenario scenario, IDictionary<string, State> states)
        {
            scenario.Behaviors.ForEach(b => AddTransition(b, states));
        }

        private void AddTransition(Behavior behavior, IDictionary<string, State> states)
        {
            State source = getOrCreateState(behavior.PreState, states);
            State target = getOrCreateState(behavior.PostState, states);

            source.Outgoing.Add(new Transition
            {
                Target = target,
                InMessage = new Message(behavior.InMessage),
                OutMessages = behavior.OutMessages.Select(m => new Message(m)).ToList()
            });
        }

        private State getOrCreateState(string name, IDictionary<string, State> states)
        {
            if (states.ContainsKey(name))
                return states[name];

            return states[name] = new State {Name = name};
        }
    }
}