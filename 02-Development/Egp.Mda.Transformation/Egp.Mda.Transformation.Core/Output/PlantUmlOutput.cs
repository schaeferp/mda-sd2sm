using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core.Output
{
    internal class PlantUmlOutput : IOutputGenerator
    {
        public IList<string> GenerateTextDiagrams(UmlStateMachineModel StateMachines)
        {
            IList<string> TextDiagram =
                StateMachines.Machines.Select(StateMachine => PrintRegion(StateMachine.Region)).ToList();
            return TextDiagram;
        }

        private static string PrintRegion(UmlRegion Region)
        {
            // names a region
            var textDiagram = "state " + Region.Name + "{";

            // add entry- and exit-states
            IList<UmlPseudoState> pseudoStates = (from state in Region.Vertices.OfType<UmlPseudoState>()
                where (state.Kind.Equals(UmlPseudoStateKind.Entry) || state.Kind.Equals(UmlPseudoStateKind.Exit))
                select state).ToList();

            foreach (var pseudoState in pseudoStates)
            {
                if (pseudoState.Kind.Equals(UmlPseudoStateKind.Entry))
                {
                    textDiagram += "state " + pseudoState.Label + "<<entrypoint>>\r\n";
                }
                else if (pseudoState.Kind.Equals((UmlPseudoStateKind.Exit)))
                {
                    textDiagram += "state " + pseudoState.Label + "<<exitpoint>>\r\n";
                }
            }

            // add initial-states
            IList<UmlPseudoState> initialStates =
                (from state in Region.Vertices.OfType<UmlPseudoState>()
                    where state.Kind.Equals(UmlPseudoStateKind.Initial)
                    select state).ToList();

            textDiagram = initialStates.Aggregate(textDiagram,
                (current, initalState) => current + ("[*] --> " + initalState.Label + "\r\n"));

            // add initial states
            IList<UmlTransition> transitions =
                (from transition in Region.Vertices.OfType<UmlTransition>() select transition).ToList();
            textDiagram = transitions.Aggregate(textDiagram,
                (current, transition) =>
                    current +
                    (transition.Origin.Label + " --> " + transition.Target.Label + " : " + transition.Label + "\r\n"));

            textDiagram += "}";
            return textDiagram;
        }
    }
}