using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class ParticipantBehaviorComposition
    {
        public IParticipant Participant { get; set; }

        public IList<BehaviorComposition> BehaviorCompositions { get; set; }
    }
}