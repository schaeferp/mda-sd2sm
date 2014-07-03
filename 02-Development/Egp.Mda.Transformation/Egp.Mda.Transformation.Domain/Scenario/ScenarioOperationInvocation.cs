using System;

namespace Egp.Mda.Transformation.Domain
{
    public class ScenarioOperationInvocation
    {
        public ScenarioStateInvariant PreScenarioStateInvariant { get; set; }
        public ScenarioStateInvariant PostScenarioStateInvariant { get; set; }
        public IParticipant Sender { get; set; }
        public IParticipant Receiver { get { return ScenarioOperation.Receiver; } }
        public ScenarioOperation ScenarioOperation { get; set; }
        public string Return { get; set; }

        public override string ToString()
        {
            return String.Format("{0} -> {1} -> {2}", PreScenarioStateInvariant, ScenarioOperation,
                PostScenarioStateInvariant);
        }
    }
}