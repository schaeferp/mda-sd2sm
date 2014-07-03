using System.Collections.Generic;
using System.Linq;

namespace Egp.Mda.Transformation.Domain
{
    public class BehaviorModel
    {
        private IList<ParticipantBehaviorComposition> _participantCompositions;

        public IList<ParticipantBehaviorComposition> ParticipantCompositions
        {
            get
            {
                return _participantCompositions ??
                       (_participantCompositions = new List<ParticipantBehaviorComposition>());
            }
        }

        public IEnumerable<ParticipantBehaviorComposition> this[IParticipant participant]
        {
            get { return ParticipantCompositions.Where(p => p.Participant == participant); }
        }

        public override string ToString()
        {
            return ParticipantCompositions.ToString();
        }
    }
}