using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Scenario
{
    public class OperationInvocation
    {
        public StateInvariant PreStateInvariant { get; set; }
        public StateInvariant PostStateInvariant { get; set; }
        public IParticipant Sender { get; set; }
        public Operation Operation { get; set; }
        public ReturnType Return { get; set; }
    }
}
