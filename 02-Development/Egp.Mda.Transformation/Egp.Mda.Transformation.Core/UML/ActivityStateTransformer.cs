using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public class ActivityStateTransformer
    {
        private const string StateLabel = "do /";
        private const string VarName = "check";
        private const string CallLabel = "{0}.{1};";
        private const string AssignLabel = "{0} := {1}.{2}";
        private const string CheckLabel = "{0} = {1}";

        private readonly string _activityName;
        private readonly UmlRegion _region;
        private int _exitCount;

        public ActivityStateTransformer(string activityName, IList<IList<MessageTriple>> behaviors)
        {
            _exitCount = 0;
            _activityName = activityName;
            _region = new UmlRegion(_activityName);

            Transform(behaviors);
        }

        public UmlRegion Result
        {
            get { return _region; }
        }

        private void Transform(IList<IList<MessageTriple>> behaviors)
        {
            var entryPoint = _region.EnsurePseudoState("*", UmlPseudoStateKind.Entry);
            entryPoint.Outgoing.Add(new UmlTransition {Target = CreateState(behaviors)});
        }

        private UmlVertex CreateState(IList<IList<MessageTriple>> behaviors)
        {
            if (behaviors.Count == 0)
                return CreateExitPoint();

            var state = _region.AddState();
            state.Label = Label(StateLabel);

            if (behaviors.Count == 1)
            {
                FinalizeState(state, behaviors.First());
                return state;
            }

            while (true)
            {
                var callGroups = ClassifyBehaviors(behaviors);

                if (callGroups.Count == 0)
                {
                    state.Outgoing.Add(new UmlTransition {Target = CreateExitPoint()});
                    return state;
                }

                if (callGroups.Count > 1)
                {
                    CreateIndeterministicBranch(state, callGroups);
                    return state;
                }

                var callGroup = callGroups.First();
                if (callGroup.Returns.Count > 1)
                {
                    CreateDeterministicBranch(state, callGroup);
                    return state;
                }

                ApplyMessageCall(state, callGroup);
            }
        }

        private void FinalizeState(UmlVertex state, IEnumerable<MessageTriple> behavior)
        {
            foreach (var message in behavior)
                state.Label += Label(CallLabel, message.Target, message.Operation);

            state.Outgoing.Add(new UmlTransition {Target = CreateExitPoint()});
        }

        private UmlVertex CreateExitPoint()
        {
            var exitLabel = (++_exitCount).ToString(CultureInfo.InvariantCulture);
            return _region.EnsurePseudoState(exitLabel, UmlPseudoStateKind.Exit);
        }

        private static void ApplyMessageCall(UmlVertex state, CallGroup callGroup)
        {
            state.Label += Label(CallLabel, callGroup.Target, callGroup.Operation);
            callGroup.Returns.SelectMany(r => r.Behaviors).ForEach(b => b.RemoveAt(0));
        }

        private static List<CallGroup> ClassifyBehaviors(IEnumerable<IList<MessageTriple>> behaviors)
        {
            return (
                from behavior in behaviors
                group behavior by new
                {
                    behavior.First().Target,
                    behavior.First().Operation
                }
                into callGroup
                select new CallGroup
                {
                    Target = callGroup.Key.Target,
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

        private void CreateDeterministicBranch(UmlVertex origin, CallGroup callGroup)
        {
            origin.Label += Label(AssignLabel, VarName, callGroup.Target, callGroup.Operation);

            foreach (var returnGroup in callGroup.Returns)
            {
                returnGroup.Behaviors.ForEach(b => b.RemoveAt(0));
                var groupState = CreateState(returnGroup.Behaviors);
                var guard = String.Format(CheckLabel, VarName, returnGroup.Value);
                origin.Outgoing.Add(new UmlTransition {Target = groupState, Guard = guard});
            }
        }

        private void CreateIndeterministicBranch(UmlVertex origin, IEnumerable<CallGroup> callGroups)
        {
            Console.WriteLine("WARNING: Indeterministic Transition in activity `{0}`!", _activityName);
            foreach (var g in callGroups)
            {
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
            public IParticipant Target { get; set; }
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