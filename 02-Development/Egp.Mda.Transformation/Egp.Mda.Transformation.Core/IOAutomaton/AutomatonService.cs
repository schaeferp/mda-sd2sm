using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain.Behavior;
using Egp.Mda.Transformation.Domain.IOAutomaton;

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
            var automaton = new Automaton(context.Participant);

            if (context.Scenarios.Count > 0)
            {
                var initialStateName = context.Scenarios[0].Behaviors[0].PreState;
                automaton.InitialState = automaton.GetState(initialStateName);
                context.Scenarios.ForEach(s => TransformScenario(s,automaton));
            }

            return automaton;
        }

        private void TransformScenario(Scenario scenario, Automaton automaton)
        {
            scenario.Behaviors.ForEach(b => AddTransition(b, automaton));
        }

        private void AddTransition(Behavior behavior, Automaton automaton)
        {
            State source = automaton.GetState(behavior.PreState);
            State target = automaton.GetState(behavior.PostState);

            source.Outgoing.Add(new Transition
            {
                Target = target,
                InMessageTriple = behavior.InMessageTriple,
                OutMessages = behavior.OutMessages
            });
        }
        }
}