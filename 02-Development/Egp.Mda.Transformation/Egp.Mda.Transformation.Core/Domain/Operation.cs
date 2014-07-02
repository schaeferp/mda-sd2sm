namespace Egp.Mda.Transformation.Core
{
    public enum OperationKind
    {
        Reply,
        Request
    };

    public class Operation
    {
        public string Name { get; set; }
        public IParticipant Receiver { get; set; }
        public OperationKind Kind { get; set; }
    }
}