namespace Egp.Mda.Transformation.Domain.Uml
{
    public enum PseudoStateKind
    {
        Initial,
        Terminate,
        Entry,
        Exit
    }

    public class PseudoState : Vertex
    {
        public PseudoStateKind Kind { get; set; }

        public PseudoState(PseudoStateKind kind)
        {
            Kind = kind;
        }
    }
}