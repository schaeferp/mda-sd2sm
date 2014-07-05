using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    internal class StateMachineTransformer
    {
        private readonly IDictionary<UmlVertex, IList<IList<MessageTriple>>> _actions;
        private readonly IOAutomaton _automaton;
        private readonly UmlStateMachine _machine;

        public StateMachineTransformer(IOAutomaton automaton)
        {
            _automaton = automaton;
            _machine = new UmlStateMachine();
            _actions = new Dictionary<UmlVertex, IList<IList<MessageTriple>>>();

            Transform();
        }

        public UmlStateMachine Result
        {
            get { return _machine; }
        }

        private void Transform()
        {
            _machine.Region = new UmlRegion(_automaton.Participant.Name);
            var entry = _machine.Region.EnsurePseudoState("*", UmlPseudoStateKind.Initial);

            if (_automaton.InitialState != null)
            {
                var initial = _machine.Region.EnsureState(_automaton.InitialState.Name);
                entry.Outgoing.Add(new UmlTransition {Target = initial});
            }

            _automaton.States.ForEach(TransformState);
            _actions.ForEach(action => TransformActivity(action.Key, action.Value));
        }

        private void TransformState(IOState state)
        {
            var transformed = _machine.Region.EnsureState(state.Name);
            state.Outgoing.ForEach(t => TransformTransition(t, transformed));
        }

        private void TransformTransition(IOTransition transition, UmlVertex source)
        {
            var activityName = ": " + transition.InMessageTriple.Operation;
            var activity = _machine.Region.EnsureState(activityName,
                created => CreateActivityTransition(source, created, transition));

            activity.Outgoing.Add(new UmlTransition
            {
                Target = _machine.Region.EnsureState(transition.Target.Name),
                Action = "",
                Return = "return " + transition.InMessageTriple.Return
            });
        }

        private void CreateActivityTransition(UmlVertex source, UmlVertex activity, IOTransition transition)
        {
            source.Outgoing.Add(new UmlTransition
            {
                Target = activity,
                Action = transition.InMessageTriple.Operation,
                Return = ""
            });

            RegisterActivityMessages(activity, transition.OutMessages);
        }

        private void RegisterActivityMessages(UmlVertex activity, IList<MessageTriple> behavior)
        {
            IList<IList<MessageTriple>> behaviors;

            _actions.TryGetValue(activity, out behaviors);
            if (behaviors == null)
                behaviors = _actions[activity] = new List<IList<MessageTriple>>();

            behaviors.Add(behavior);
        }

        private void TransformActivity(UmlVertex vertex, IList<IList<MessageTriple>> behaviors)
        {
            var activity = vertex as UmlState;
            if (activity == null)
                return;

            var transformer = new ActivityStateTransformer(activity.Label, behaviors);
            activity.Region = transformer.Result;
        }
    }
}