using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core.IOAutomaton
{
    public class AutomatonService : IAutomatonService
    {
        public IOAutomatonModel From(BehaviorModel model)
        {
            var automata = model.ParticipantCompositions.Select(TransformParticipantBehaviorComposition);
            return new IOAutomatonModel(automata);
        }

        private Domain.IOAutomaton TransformParticipantBehaviorComposition(ParticipantBehaviorComposition composition)
        {
            var automaton = new Domain.IOAutomaton(composition.Participant);

            if (composition.BehaviorCompositions.Count > 0)
                return automaton;

            var initialStateName = composition.BehaviorCompositions[0].Behaviors[0].PreState;
            automaton.InitialState = automaton.GetState(initialStateName);
            composition.BehaviorCompositions.ForEach(s => TransformScenario(s, automaton));

            return automaton;
        }

        private void TransformScenario(BehaviorComposition composition, Domain.IOAutomaton automaton)
        {
            composition.Behaviors.ForEach(b => AddTransition(b, automaton));
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