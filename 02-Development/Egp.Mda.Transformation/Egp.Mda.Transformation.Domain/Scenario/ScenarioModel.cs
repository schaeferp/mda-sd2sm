using System.Collections.Generic;
using System.Linq;

namespace Egp.Mda.Transformation.Domain
{
    public class ScenarioModel
    {
        private List<Scenario> _scenarios;

        public List<Scenario> Scenarios
        {
            get { return _scenarios ?? (_scenarios = new List<Scenario>()); }
        }

        public IEnumerable<IParticipant> SenderParticipants
        {
            get
            {
                return Scenarios.SelectMany(
                    scn => scn.Invocations.Select(inv => inv.Sender));
            }
        }

        public IEnumerable<IParticipant> ReceiverParticipants
        {
            get
            {
                return Scenarios.SelectMany(
                    scn => scn.Invocations.Select(inv => inv.ScenarioOperation.Receiver));
            }
        }

        public IEnumerable<IParticipant> Participants
        {
            get { return ReceiverParticipants.Union(SenderParticipants); }
        }
    }
}