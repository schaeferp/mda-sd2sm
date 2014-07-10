using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    /// <summary>
    ///     Transforms multiple behaviors of one activity state into one merged sub state machine.
    /// </summary>
    public class ActivityStateTransformer
    {
        private const string StateLabel = "do{0} /";
        private const string VarName = "check";
        private const string CallLabel = "{0}.{1};";
        private const string AssignLabel = "{0} := {1}.{2}";
        private const string CheckLabel = "{0} = {1}";

        private readonly string _activityName;
        private readonly UmlRegion _region;
        private int _exitCount;
        private int _stateCount;

        /// <summary>
        ///     Creates a new transformer instance and computes the transformation.
        ///     NOTE: The behaviors are modified during this transformation process!
        /// </summary>
        /// <param name="activityName">The name of the containing activity state.</param>
        /// <param name="behaviors">A list of behaviors.</param>
        public ActivityStateTransformer(string activityName, IList<IList<MessageTriple>> behaviors)
        {
            _exitCount = 0;
            _stateCount = 0;
            _activityName = activityName;
            _region = new UmlRegion(_activityName);

            Transform(behaviors);
        }

        /// <summary>
        ///     The transformed <see cref="UmlRegion" />.
        /// </summary>
        public UmlRegion Result
        {
            get { return _region; }
        }

        /// <summary>
        ///     Starts the transformation by creating an entry point.
        /// </summary>
        /// <param name="behaviors">The full list of behaviors.</param>
        private void Transform(IList<IList<MessageTriple>> behaviors)
        {
            var entryPoint = _region.EnsurePseudoState("*", UmlPseudoStateKind.Entry);
            entryPoint.Outgoing.Add(new UmlTransition {Target = CreateState(behaviors)});
        }

        /// <summary>
        ///     Creates a state depending on the first message in all behaviors:
        ///     - If all messages are identical, an operation call is added to the states label,
        ///     - If the return values differ, a deterministic branch is created,
        ///     - If the operations differ, an indeterministic branch is created.
        ///     The subsequent states are created recursively using this method.
        /// </summary>
        /// <param name="behaviors">A list of behaviors.</param>
        /// <returns></returns>
        private UmlVertex CreateState(IList<IList<MessageTriple>> behaviors)
        {
            // Directly exit, if there are no messages left.
            if (behaviors.Count == 0)
                return CreateExitPoint();

            // Create a state for the next message(s).
            var state = _region.AddState();
            state.Label = Label(StateLabel, ++_stateCount);

            if (behaviors.Count == 1)
            {
                // There is only one behavior left, so fast forward.
                FinalizeState(state, behaviors.First());
                return state;
            }

            while (true)
            {
                var callGroups = ClassifyBehaviors(behaviors);

                if (callGroups.Count == 0)
                {
                    // All behaviors have finished, so we can exit.
                    state.Outgoing.Add(new UmlTransition {Target = CreateExitPoint()});
                    return state;
                }

                if (callGroups.Count > 1)
                {
                    // There are several possible operations to call next. Therefore we 
                    // have to create an indeterministic branch and continue recursively.
                    CreateIndeterministicBranch(state, callGroups);
                    return state;
                }

                // The next operation call is distinct, so get it first.
                var callGroup = callGroups.First();
                if (callGroup.Returns.Count > 1)
                {
                    // The result value is not distinct. Therefore we have to create a
                    // deterministic branch and continue recursively.
                    CreateDeterministicBranch(state, callGroup);
                    return state;
                }

                // Both operation and return value are distinct. Therefore, simply add the
                // call to this state and continue with the next behavior.
                ApplyMessageCall(state, callGroup);
            }
        }

        /// <summary>
        ///     Finalizes a state by adding all message calls of the given behavior to the state and appending an exit point.
        /// </summary>
        /// <param name="state">The state to finalize.</param>
        /// <param name="behavior">The remaining behavior of the state.</param>
        private void FinalizeState(UmlVertex state, IEnumerable<MessageTriple> behavior)
        {
            foreach (var message in behavior)
                state.Label += Label(CallLabel, message.Target, message.Operation);

            state.Outgoing.Add(new UmlTransition {Target = CreateExitPoint()});
        }

        /// <summary>
        ///     Creates an exit point for this activity. The exit point will have a unique identifier.
        /// </summary>
        /// <returns>An exit pseudo state.</returns>
        private UmlVertex CreateExitPoint()
        {
            var exitLabel = (++_exitCount).ToString(CultureInfo.InvariantCulture);
            return _region.EnsurePseudoState(exitLabel, UmlPseudoStateKind.Exit);
        }

        /// <summary>
        ///     Adds a message call to the given state and removes it from all behaviors in the call group.
        /// </summary>
        /// <param name="state">The corresponding state.</param>
        /// <param name="callGroup">A call group with identical message calls.</param>
        private static void ApplyMessageCall(UmlVertex state, CallGroup callGroup)
        {
            state.Label += Label(CallLabel, callGroup.Target, callGroup.Operation);
            callGroup.Returns.SelectMany(r => r.Behaviors).ForEach(b => b.RemoveAt(0));
        }

        /// <summary>
        ///     Classifies a list of behaviors by grouping them into <see cref="CallGroup" />s, each containing
        ///     <see cref="ReturnGroup" />s.
        /// </summary>
        /// <param name="behaviors">A list of behaviors to classify.</param>
        /// <returns>The classified behavior groups.</returns>
        private static List<CallGroup> ClassifyBehaviors(IEnumerable<IList<MessageTriple>> behaviors)
        {
            return (
                from behavior in behaviors.Where(b => b.Count > 0)
                group behavior by new
                {
                    behavior.First().Target.Name,
                    behavior.First().Operation
                }
                into callGroup
                select new CallGroup
                {
                    Target = callGroup.Key.Name,
                    Operation = callGroup.Key.Operation,
                    Returns = (
                        from cg in callGroup
                        group cg by cg.First().Return
                        into returnGroup
                        select new ReturnGroup
                        {
                            Value = returnGroup.Key,
                            Behaviors = returnGroup.ToList()
                        }).ToList()
                }).ToList();
        }

        /// <summary>
        ///     Branches a call group deterministically.
        /// </summary>
        /// <param name="origin">The source state or pseudo state.</param>
        /// <param name="callGroup">Several call groups.</param>
        private void CreateDeterministicBranch(UmlVertex origin, CallGroup callGroup)
        {
            // Store the result of the operation call in a variable
            origin.Label += Label(AssignLabel, VarName, callGroup.Target, callGroup.Operation);

            foreach (var returnGroup in callGroup.Returns)
            {
                // Remove the first message in the behavior and create a new state for all remaining messages.
                returnGroup.Behaviors.ForEach(b => b.RemoveAt(0));
                var groupState = CreateState(returnGroup.Behaviors);
                // Add a transition to this new state with a guard checking the return value.
                var guard = String.Format(CheckLabel, VarName, returnGroup.Value);
                origin.Outgoing.Add(new UmlTransition {Target = groupState, Guard = guard});
            }
        }

        /// <summary>
        ///     Branches a call group indeterministically.
        /// </summary>
        /// <param name="origin">The source state or pseudo state.</param>
        /// <param name="callGroups">Several call groups.</param>
        private void CreateIndeterministicBranch(UmlVertex origin, IEnumerable<CallGroup> callGroups)
        {
            Console.WriteLine("WARNING: Indeterministic Transition in activity `{0}`!", _activityName);
            foreach (var g in callGroups)
            {
                // Create a new state for all remaining messages and add a transition there.
                var groupBehaviors = g.Returns.SelectMany(r => r.Behaviors).ToList();
                var groupState = CreateState(groupBehaviors);
                origin.Outgoing.Add(new UmlTransition {Target = groupState});
            }
        }

        private static string Label(string format, params object[] args)
        {
            return String.Format(format, args) + Environment.NewLine;
        }

        private class CallGroup
        {
            public string Target { get; set; }
            public string Operation { get; set; }
            public IList<ReturnGroup> Returns { get; set; }
        }

        private class ReturnGroup
        {
            public string Value { get; set; }
            public IList<IList<MessageTriple>> Behaviors { get; set; }
        }
    }
}