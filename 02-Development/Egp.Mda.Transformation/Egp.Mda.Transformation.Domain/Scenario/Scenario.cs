using System.Collections.Generic;
using System.Linq;

namespace Egp.Mda.Transformation.Domain
{
    public class Scenario
    {
        private IList<ScenarioOperationInvocation> _invocations;

        public string Name { get; set; }

        public IList<ScenarioOperationInvocation> Invocations
        {
            get { return _invocations ?? (_invocations = new List<ScenarioOperationInvocation>()); }
        }

        public IEnumerable<IParticipant> ReceiverParticipants
        {
            get { return Invocations.Select(i => i.ScenarioOperation.Receiver); }
        }

        public IEnumerable<IParticipant> SenderParticipants
        {
            get { return Invocations.Select(i => i.Sender); }
        }

        public IEnumerable<IParticipant> Participants
        {
            get { return ReceiverParticipants.Union(SenderParticipants); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}