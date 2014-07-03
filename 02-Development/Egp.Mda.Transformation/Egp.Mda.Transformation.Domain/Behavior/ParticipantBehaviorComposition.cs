using System;
using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class ParticipantBehaviorComposition
    {
        private List<BehaviorComposition> _behaviorCompositions;

        public IParticipant Participant { get; set; }

        public List<BehaviorComposition> BehaviorCompositions
        {
            get { return _behaviorCompositions ?? (_behaviorCompositions = new List<BehaviorComposition>()); }
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", Participant, BehaviorCompositions);
        }
    }
}