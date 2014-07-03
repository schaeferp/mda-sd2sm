namespace Egp.Mda.Transformation.Domain
{
    public enum PseudoStateKind
    {
        Initial,
        Terminate,
        Entry,
        Exit
    }

    public class UmlPseudoState : Vertex
    {
        public UmlPseudoState(PseudoStateKind kind)
        {
            Kind = kind;
        }

        public PseudoStateKind Kind { get; set; }
    }
}