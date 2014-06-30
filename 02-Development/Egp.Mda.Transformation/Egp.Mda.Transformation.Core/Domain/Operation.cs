namespace Egp.Mda.Transformation.Core
{
    public enum OperationSort { Reply , Request };

    public class Operation
    {
        public string Name { get; set; }
        public Actor Receiver { get; set; }
        public OperationSort Sort { get; set; }
    }
}
