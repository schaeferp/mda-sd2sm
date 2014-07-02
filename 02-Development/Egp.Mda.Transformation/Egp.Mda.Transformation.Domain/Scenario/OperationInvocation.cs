using System;
using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Scenario
{
    public class OperationInvocation
    {
        public StateInvariant PreStateInvariant { get; set; }
        public StateInvariant PostStateInvariant { get; set; }
        public IParticipant Sender { get; set; }
        public Operation Operation { get; set; }
        public string Return { get; set; }

        public override string ToString()
        {
            return String.Format("{0} -> {1} -> {2}", PreStateInvariant, Operation, PostStateInvariant);
        }
    }
}