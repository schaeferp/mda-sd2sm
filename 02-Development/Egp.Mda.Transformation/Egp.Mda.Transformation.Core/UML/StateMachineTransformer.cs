using System.Collections.Generic;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    /// <summary>
    ///     Statefully transforms an I/O-Automaton into a state machine. The result is stored in the <see cref="Result" />
    ///     property.
    /// </summary>
    internal class StateMachineTransformer
    {
        private readonly IDictionary<UmlVertex, IList<IList<MessageTriple>>> _actions;
        private readonly IOAutomaton _automaton;
        private readonly UmlStateMachine _machine;

        /// <summary>
        ///     Creates a new transformer instance and computes the transformation.
        /// </summary>
        /// <param name="automaton">An I/O-Automaton to transform.</param>
        public StateMachineTransformer(IOAutomaton automaton)
        {
            _automaton = automaton;
            _machine = new UmlStateMachine();
            _actions = new Dictionary<UmlVertex, IList<IList<MessageTriple>>>();

            Transform();
        }

        /// <summary>
        ///     The transformed <see cref="UmlStateMachine" />.
        /// </summary>
        public UmlStateMachine Result
        {
            get { return _machine; }
        }

        /// <summary>
        ///     Transforms the contents of the automaton.
        /// </summary>
        private void Transform()
        {
            // Create the root region and the entry point of the state machine
            _machine.Region = new UmlRegion(_automaton.Participant.Name);
            var entry = _machine.Region.EnsurePseudoState("*", UmlPseudoStateKind.Initial);

            // Add a transition from the entry point to the initial state.
            if (_automaton.InitialState != null)
            {
                var initial = _machine.Region.EnsureState(_automaton.InitialState.Name);
                entry.Outgoing.Add(new UmlTransition {Target = initial});
            }

            // Create all stable and activity states.
            _automaton.States.ForEach(TransformState);
            // Create submachines for all activity states.
            _actions.ForEach(action => TransformActivity(action.Key, action.Value));
        }

        /// <summary>
        ///     Create a new stable state and transform all outgoing transitions.
        /// </summary>
        /// <param name="state">The corresponding state in the I/O-Automaton.</param>
        private void TransformState(IOState state)
        {
            var transformed = _machine.Region.EnsureState(state.Name);
            state.Outgoing.ForEach(t => TransformTransition(t, transformed));
        }

        /// <summary>
        ///     Transforms a transition of the I/O-Automation into an activity state.
        /// </summary>
        /// <param name="transition">The original transition in the I/O-Automaton.</param>
        /// <param name="source">The source state in the transformed state machine.</param>
        private void TransformTransition(IOTransition transition, UmlVertex source)
        {
            // Create the activity state unless it already exists.
            var activityName = ": " + transition.InMessageTriple.Operation;
            var activity = _machine.Region.EnsureState(activityName,
                created => CreateActivityTransition(source, created, transition));

            // Add a transition from the activity state to the target stable state.
            activity.Outgoing.Add(new UmlTransition
            {
                Target = _machine.Region.EnsureState(transition.Target.Name),
                Action = "",
                Return = "return " + transition.InMessageTriple.Return
            });
        }

        /// <summary>
        ///     Creates a transition from a stable state to an activity state. The behavior of the transition (i.e. the list of out
        ///     messages) is first stored and then merged into a submachine.
        ///     NOTE: This method should only be called once for each activity state!
        /// </summary>
        /// <param name="source">The source state in the transformed state machine.</param>
        /// <param name="activity">The activity state in the transformed state machine.</param>
        /// <param name="transition">The original transition in the I/O-Automaton.</param>
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

        /// <summary>
        ///     Stores the given behavior (i.e. list of out messages) of a transition to transform it into a submachine later.
        /// </summary>
        /// <param name="activity">The associated activity state.</param>
        /// <param name="behavior">One possible behavior of the activity.</param>
        private void RegisterActivityMessages(UmlVertex activity, IList<MessageTriple> behavior)
        {
            IList<IList<MessageTriple>> behaviors;

            _actions.TryGetValue(activity, out behaviors);
            if (behaviors == null)
                behaviors = _actions[activity] = new List<IList<MessageTriple>>();

            behaviors.Add(behavior);
        }

        /// <summary>
        ///     Merges all recorded behaviors of the specified activity into one submachine.
        /// </summary>
        /// <param name="vertex">An activity state.</param>
        /// <param name="behaviors">All recorded behaviors of the activity.</param>
        private static void TransformActivity(UmlVertex vertex, IList<IList<MessageTriple>> behaviors)
        {
            // Check if the given vertex even is an activity.
            var activity = vertex as UmlState;
            if (activity == null)
                return;

            // Merge all behaviors into one region and attach it to the activity.
            var transformer = new ActivityStateTransformer(activity.Label, behaviors);
            activity.Region = transformer.Result;
        }
    }
}