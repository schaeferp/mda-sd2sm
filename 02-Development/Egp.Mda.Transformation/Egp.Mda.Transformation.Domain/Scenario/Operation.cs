using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Scenario
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
