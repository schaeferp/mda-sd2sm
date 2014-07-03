using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public abstract class UmlVertex
    {
        public string Label { get; set; }
        public IList<UmlTransition> Outgoing { get; protected set; }
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
        public UmlPseudoState(PseudoStateKind kind)
        {
            Kind = kind;
        }

        public PseudoStateKind Kind { get; set; }
    }

    public enum PseudoStateKind
    {
        Initial,
        Terminate,
        Entry,
        Exit
    }
}