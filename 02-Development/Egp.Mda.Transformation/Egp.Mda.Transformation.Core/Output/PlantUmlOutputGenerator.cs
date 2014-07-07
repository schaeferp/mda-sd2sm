using System;
using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core.Output
{
    /// <summary>
    ///     Generates output for PlantUML, to create diagrams based on this textoutput
    /// </summary>
    public class PlantUmlOutputGenerator : IOutputGenerator
    {
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
            // names a region
            var textDiagram = Environment.NewLine + "state " + region.Name + "{";

            // add entry- and exit-states
            IList<UmlPseudoState> pseudoStates = (from state in region.Vertices.OfType<UmlPseudoState>()
                where (state.Kind.Equals(UmlPseudoStateKind.Entry) || state.Kind.Equals(UmlPseudoStateKind.Exit))
                select state).ToList();

            foreach (var pseudoState in pseudoStates)
            {
                if (pseudoState.Kind.Equals(UmlPseudoStateKind.Entry))
                {
                    textDiagram += "state " + pseudoState.Label + "<<entrypoint>>" + Environment.NewLine;
                }
                else if (pseudoState.Kind.Equals((UmlPseudoStateKind.Exit)))
                {
                    textDiagram += "state " + pseudoState.Label + "<<exitpoint>>" + Environment.NewLine;
                }
            }

            // add initial-states
            IList<UmlPseudoState> initialStates =
                (from state in region.Vertices.OfType<UmlPseudoState>()
                    where state.Kind.Equals(UmlPseudoStateKind.Initial)
                    select state).ToList();

            textDiagram = initialStates.Aggregate(textDiagram,
                (current, initalState) => current + ("[*] --> " + initalState.Label + Environment.NewLine));

            // add initial states
            IList<UmlState> states = (from state in region.Vertices.OfType<UmlState>() select state).ToList();

            foreach (var state in states)
            {
                var origin = state.Label;
                textDiagram = state.Outgoing.Aggregate(textDiagram,
                    (current, transition) =>
                        current +
                        (origin + " --> " + transition.Target.Label + " : " + transition.Label + Environment.NewLine));
            }

            textDiagram += "}" + Environment.NewLine;
            return textDiagram;
        }
    }
}