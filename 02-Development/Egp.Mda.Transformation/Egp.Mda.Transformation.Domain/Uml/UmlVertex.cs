using System;
using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public abstract class UmlVertex
    {
        private IList<UmlTransition> _outgoing;

        public string Label { get; set; }

        public IList<UmlTransition> Outgoing
        {
            get { return _outgoing ?? (_outgoing = new List<UmlTransition>()); }
        }

        public string GetName()
        {
            if (!Label.Contains(Environment.NewLine)) return Label;
            var index = Label.IndexOf(Environment.NewLine, StringComparison.Ordinal);
            return Label.Substring(0, index);
        }
    }

    public class UmlState : UmlVertex, IUmlRegionOwner
    {
        public bool IsCompositional
        {
            get { return Region != null; }
        }

        public UmlRegion Region { get; set; }
    }

    public class UmlPseudoState : UmlVertex
    {
        public UmlPseudoState(UmlPseudoStateKind kind)
        {
            Kind = kind;
        }

        public UmlPseudoStateKind Kind { get; set; }
    }

    public enum UmlPseudoStateKind
    {
        Initial,
        Terminate,
        Entry,
        Exit
    }
}