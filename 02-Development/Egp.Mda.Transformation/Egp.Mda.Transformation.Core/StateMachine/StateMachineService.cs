﻿using System.Collections.Generic;
using Egp.Mda.Transformation.Core.IOAutomaton;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public class StateMachineService : IStateMachineService
    {
        public UmlStateMachine From(Domain.IOAutomaton ioAutomaton)
        {
            var stateMachine = new UmlStateMachine();

            UmlRegion umlRegion = stateMachine.CreateRegion();
            var initial = new UmlPseudoState(PseudoStateKind.Initial);
            umlRegion.AddVertex(initial);

            ioAutomaton.States.ForEach(s => TransformState(s, umlRegion));
            return stateMachine;
        }

        private void TransformState(IOState state, UmlRegion umlRegion)
        {
            var stableState = umlRegion.GetOrCreateStableState(state.Name);
            state.Outgoing.ForEach(t => TransformTransition(t, stableState, umlRegion));
        }

        private void TransformTransition(IOTransition transition, Vertex source, UmlRegion umlRegion)
        {
            bool created;
            string activityStateName = ": " + transition.InMessageTriple.Operation;
            var activityState = umlRegion.GetOrCreateActivityState(activityStateName, out created);

            if (created)
            {
                source.Outgoing.Add(new Uml
                {
                    Target = activityState,
                    Action = transition.InMessageTriple.Operation,
                    Return = ""
                });

                InitializeSubStateMachine(transition.OutMessages, activityState);
            }

            activityState.Outgoing.Add(new Uml
            {
                Target = umlRegion.GetOrCreateStableState(transition.InMessageTriple.Target.Name),
                Action = "",
                Return = transition.InMessageTriple.Return
            });
        }

        private void InitializeSubStateMachine(IEnumerable<MessageTriple> transition, Vertex parent)
        {
            
        }
    }
}