using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core.IOAutomaton
{
    public class AutomatonService : IAutomatonService
    {
        public IEnumerable<Domain.IOAutomaton> From(IEnumerable<ParticipantBehaviorComposition> contexts)
        {
            return contexts.Select(TransformContext);
        }

        private Domain.IOAutomaton TransformContext(ParticipantBehaviorComposition participantBehaviorComposition)
        {
            var automaton = new Domain.IOAutomaton(participantBehaviorComposition.Participant);

            if (participantBehaviorComposition.BehaviorCompositions.Count > 0)
            {
                var initialStateName = participantBehaviorComposition.BehaviorCompositions[0].Behaviors[0].PreState;
                automaton.InitialState = automaton.GetState(initialStateName);
                participantBehaviorComposition.BehaviorCompositions.ForEach(s => TransformScenario(s,automaton));
            }

            return automaton;
        }

        private void TransformScenario(BehaviorComposition behaviorComposition, Domain.IOAutomaton ioAutomaton)
        {
            behaviorComposition.Behaviors.ForEach(b => AddTransition(b, ioAutomaton));
        }

        private void AddTransition(Behavior behavior, Domain.IOAutomaton ioAutomaton)
        {
            var source = ioAutomaton.GetState(behavior.PreState);
            var target = ioAutomaton.GetState(behavior.PostState);

            source.Outgoing.Add(new IOTransition
            {
                Target = target,
                InMessageTriple = behavior.InMessageTriple,
                OutMessages = behavior.OutMessages
            });
        }
    }
}