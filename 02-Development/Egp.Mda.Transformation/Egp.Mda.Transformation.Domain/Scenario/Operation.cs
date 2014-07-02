using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Scenario
{
    public enum OperationSort { Reply , Request };

    public class Operation
    {
        public string Name { get; set; }
        public Actor Receiver { get; set; }
        public OperationSort Sort { get; set; }
    }
}
