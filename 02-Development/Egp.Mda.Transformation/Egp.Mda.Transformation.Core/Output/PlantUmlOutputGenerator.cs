using System;
using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    /// <summary>
    ///     Generates output for PlantUML, to create diagrams based on this textoutput
    /// </summary>
    public class PlantUmlOutputGenerator : IOutputGenerator
    {
        private static string _textDiagram;
        private static readonly List<UmlRegion> _subRegions = new List<UmlRegion>();

        // Interface implementation
        public IList<string> GenerateTextDiagrams(UmlStateMachineModel stateMachines)
        {
            var tempDiagramList = stateMachines.Machines
                .Select(stateMachine => PrintRegion(stateMachine.Region))
                .ToList();
            tempDiagramList.Insert(0, "@startuml");
            tempDiagramList.Add("@enduml");
            return tempDiagramList;
        }

        private static string PrintRegion(UmlRegion region)
        {
            _textDiagram = Environment.NewLine + "state " + region.Name + "{" + Environment.NewLine;

            AddEntryExitStates(region);

            AddInitialStates(region);

            AddStates(region);

            _textDiagram += "}" + Environment.NewLine;

            //AddSubregions(); // stackoverflow exception? not sure why

            return _textDiagram;
        }

        private static void AddStates(UmlRegion region)
        {
            IList<UmlState> states = (from state in region.Vertices.OfType<UmlState>() select state).ToList();

            foreach (var state in states)
            {
                foreach (var transition in state.Outgoing)
                {
                    _textDiagram += EscapeString(state.Label) + " --> " + EscapeString(transition.Target.Label) + " : " + EscapeString(transition.Label) +
                                    Environment.NewLine;
                }

                if (state.IsCompositional)
                {
                    _subRegions.Add(state.Region);
                }
            }
        }

        private static void AddInitialStates(UmlRegion region)
        {
            IList<UmlPseudoState> initialStates =
                (from state in region.Vertices.OfType<UmlPseudoState>()
                    where state.Kind.Equals(UmlPseudoStateKind.Initial)
                    select state).ToList();

            foreach (var transition in initialStates.SelectMany(initialState => initialState.Outgoing))
            {
                _textDiagram += "[*] --> " + EscapeString(transition.Target.Label) + Environment.NewLine;
            }
        }

        private static void AddEntryExitStates(UmlRegion region)
        {
            IList<UmlPseudoState> pseudoStates = (from state in region.Vertices.OfType<UmlPseudoState>()
                where (state.Kind.Equals(UmlPseudoStateKind.Entry) || state.Kind.Equals(UmlPseudoStateKind.Exit))
                select state).ToList();

            foreach (var pseudoState in pseudoStates)
            {
                if (pseudoState.Kind.Equals(UmlPseudoStateKind.Entry))
                {
                    _textDiagram += "state " + EscapeString(pseudoState.Label) + "<<entrypoint>>" + Environment.NewLine;
                }
                else if (pseudoState.Kind.Equals((UmlPseudoStateKind.Exit)))
                {
                    _textDiagram += "state " + EscapeString(pseudoState.Label) + "<<exitpoint>>" + Environment.NewLine;
                }
            }
            foreach (var pseudoState in pseudoStates)
            {
                if (!pseudoState.Kind.Equals(UmlPseudoStateKind.Entry)) continue;
                foreach (var transition in pseudoState.Outgoing)
                {
                    _textDiagram += EscapeString(pseudoState.Label) + " --> " + EscapeString(transition.Target.Label) + " : " +
                                    EscapeString(transition.Label) +
                                    Environment.NewLine;
                }
            }
        }

        private static void AddSubregions()
        {
            foreach (var subRegion in _subRegions)
            {
                _textDiagram += PrintRegion(subRegion);
            }
        }

        private static string EscapeString(string temp)
        {
            temp = temp.Replace(":", "");
            temp = temp.Replace("-", "");
            return temp;
        }
    }
}