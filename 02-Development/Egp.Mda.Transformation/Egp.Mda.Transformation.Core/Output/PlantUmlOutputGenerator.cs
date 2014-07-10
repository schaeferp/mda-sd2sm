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
            return
                tempDiagramList.Select(textDiagram => "@startuml" + Environment.NewLine + textDiagram + "@enduml")
                    .ToList();
        }

        /// <summary>
        ///     Prints a given region
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private static string PrintRegion(UmlRegion region)
        {
            _textDiagram = Environment.NewLine + "state stateMachine" + EscapeLabel(region.Name) + "{" +
                           Environment.NewLine;

            AddEntryExitStates(region);

            AddInitialStates(region);

            AddStates(region);

            _textDiagram += "}" + Environment.NewLine;

            AddSubregions();

            return _textDiagram;
        }

        /// <summary>
        ///     Adds all states to the given region
        /// </summary>
        /// <param name="region"></param>
        private static void AddStates(UmlRegion region)
        {
            IList<UmlState> states = (from state in region.Vertices.OfType<UmlState>() select state).ToList();
            foreach (var state in states)
            {
                foreach (var transition in state.Outgoing)
                {
                    _textDiagram += EscapeState(state.GetName()) + " --> " + EscapeState(transition.Target.GetName());

                    if (EscapeLabel(transition.Label) != "")
                    {
                        _textDiagram += " : " + EscapeLabel(transition.Label);
                    }
                    _textDiagram += Environment.NewLine;
                }
                if (state.Label.Contains(Environment.NewLine))
                {
                    var i = 0;
                    foreach (var line in state.Label.Split(Environment.NewLine.ToCharArray()))
                    {
                        if (i == 0 || line.Equals(""))
                        {
                            i++;
                            continue;
                        }
                        _textDiagram += EscapeState(state.GetName()) + " : " + line + Environment.NewLine;
                        i++;
                    }
                }
                _textDiagram += Environment.NewLine;

                if (!state.IsCompositional) continue;
                if (!region.Equals(state.Region))
                {
                    _subRegions.Add(state.Region);
                }
            }
        }

        /// <summary>
        ///     adds initial states to the current region
        /// </summary>
        /// <param name="region"></param>
        private static void AddInitialStates(UmlRegion region)
        {
            IList<UmlPseudoState> initialStates =
                (from state in region.Vertices.OfType<UmlPseudoState>()
                    where state.Kind.Equals(UmlPseudoStateKind.Initial)
                    select state).ToList();

            foreach (var transition in initialStates.SelectMany(initialState => initialState.Outgoing))
            {
                _textDiagram += "[*] --> " + EscapeState(transition.Target.GetName()) + Environment.NewLine;
            }
        }

        /// <summary>
        ///     adds entry and exit states to the current region
        /// </summary>
        /// <param name="region"></param>
        private static void AddEntryExitStates(UmlRegion region)
        {
            IList<UmlPseudoState> pseudoStates = (from state in region.Vertices.OfType<UmlPseudoState>()
                where (state.Kind.Equals(UmlPseudoStateKind.Entry) || state.Kind.Equals(UmlPseudoStateKind.Exit))
                select state).ToList();

            foreach (var pseudoState in pseudoStates)
            {
                var diagramPart = "";
                if (pseudoState.Kind.Equals(UmlPseudoStateKind.Entry))
                {
                    diagramPart = "state " + pseudoState.GetHashCode() + " <<entrypoint>>" + Environment.NewLine;
                }
                else if (pseudoState.Kind.Equals((UmlPseudoStateKind.Exit)))
                {
                    diagramPart += "state " + EscapeState(pseudoState.Label) + " <<exitpoint>>" + Environment.NewLine;
                }
                _textDiagram += diagramPart;
            }
            foreach (var pseudoState in pseudoStates)
            {
                if (!pseudoState.Kind.Equals(UmlPseudoStateKind.Entry)) continue;
                foreach (var transition in pseudoState.Outgoing)
                {
                    _textDiagram += pseudoState.GetHashCode() + " --> " + EscapeState(transition.Target.GetName());
                    if (EscapeLabel(transition.Label) != "")
                    {
                        _textDiagram += " : " + EscapeLabel(transition.Label);
                    }
                    _textDiagram += Environment.NewLine;
                }
            }
        }

        /// <summary>
        ///     adds a subregion to the current region
        /// </summary>
        private static void AddSubregions()
        {
            while (_subRegions.Any())
            {
                var tempRegion = _subRegions.First();
                _subRegions.Remove(tempRegion);
                _textDiagram += PrintRegion(tempRegion);
            }
        }

        /// <summary>
        ///     removes forbidden chars
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private static string EscapeLabel(string temp)
        {
            temp = temp.Replace(":", "");
            temp = temp.Replace("-", "");
            temp = temp.Replace(" ", "");
            return temp;
        }

        /// <summary>
        ///     removes forbidden chars
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private static string EscapeState(string temp)
        {
            temp = EscapeLabel(temp);
            temp = temp.Replace("/", "");
            temp = temp.Replace(".", "");
            temp = temp.Replace(";", "");
            temp = temp.Replace(Environment.NewLine, "");

            return temp;
        }
    }
}