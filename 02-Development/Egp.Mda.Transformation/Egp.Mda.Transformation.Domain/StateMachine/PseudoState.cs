namespace Egp.Mda.Transformation.Domain.StateMachine
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
    }
}