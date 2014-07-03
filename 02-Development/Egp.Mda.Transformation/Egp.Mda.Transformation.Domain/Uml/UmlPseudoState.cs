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
        public PseudoStateKind Kind { get; set; }

        public UmlPseudoState(PseudoStateKind kind)
        {
            Kind = kind;
        }
    }
}