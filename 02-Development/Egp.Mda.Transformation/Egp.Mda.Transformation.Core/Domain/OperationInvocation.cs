namespace Egp.Mda.Transformation.Core
{
    public class OperationInvocation
    {
        public StateInvariant PreStateInvariant { get; set; }
        public StateInvariant PostStateInvariant { get; set; }
        public Actor Sender { get; set; }
        public Actor Receiver { get; set; }
        public Operation Operation { get; set; }
        public ReturnType Return { get; set; }
    }
}
