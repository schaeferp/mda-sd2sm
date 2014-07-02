using System.Collections.Generic;
using Egp.Mda.Transformation.Core.IOAutomaton;
using Egp.Mda.Transformation.Domain.Behavior;
using Egp.Mda.Transformation.Domain.Common;
using Egp.Mda.Transformation.Domain.IOAutomaton;
using Egp.Mda.Transformation.Domain.Uml;
using State = Egp.Mda.Transformation.Domain.IOAutomaton.State;
using Transition = Egp.Mda.Transformation.Domain.IOAutomaton.Transition;

namespace Egp.Mda.Transformation.Core.StateMachine
{
    public class StateMachineService : IStateMachineService
    {
        public Domain.Uml.StateMachine From(Automaton automaton)
        {
            var stateMachine = new Domain.Uml.StateMachine();

            Region region = stateMachine.CreateRegion();
            var initial = new PseudoState(PseudoStateKind.Initial);
            region.AddVertex(initial);

            automaton.States.ForEach(s => TransformState(s, region));
            return stateMachine;
        }

        private void TransformState(State state, Region region)
        {
            var stableState = region.GetOrCreateStableState(state.Name);
            state.Outgoing.ForEach(t => TransformTransition(t, stableState, region));
        }

        private void TransformTransition(Transition transition, Vertex source, Region region)
        {
            bool created;
            string activityStateName = ": " + transition.InMessageTriple.Operation;
            var activityState = region.GetOrCreateActivityState(activityStateName, out created);

            if (created)
            {
                source.Outgoing.Add(new Domain.Uml.Transition
                {
                    Target = activityState,
                    Action = transition.InMessageTriple.Operation,
                    Return = ""
                });

                InitializeSubStateMachine(transition.OutMessages, activityState);
            }

            activityState.Outgoing.Add(new Domain.Uml.Transition
            {
                Target = region.GetOrCreateStableState(transition.InMessageTriple.Target.Name),
                Action = "",
                Return = transition.InMessageTriple.Return
            });
        }

        private void InitializeSubStateMachine(IEnumerable<MessageTriple> transition, Vertex parent)
        {
            
        }
    }
}