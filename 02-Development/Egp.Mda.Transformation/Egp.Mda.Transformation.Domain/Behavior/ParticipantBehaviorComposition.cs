using System;
using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    /// Stores all <see cref="BehaviorComposition"/>s for a single participant in all scenarios.
    /// </summary>
    public class ParticipantBehaviorComposition
    {
        private List<BehaviorComposition> _behaviorCompositions;
        /// <summary>
        /// The associated participant to the <see cref="BehaviorComposition"/>s, receiving the inmessages.
        /// </summary>
        public IParticipant Participant { get; set; }

        /// <summary>
        /// The <see cref="BehaviorComposition"/>s for the participant in all scenarios.
        /// </summary>
        public List<BehaviorComposition> BehaviorCompositions
        {
            get { return _behaviorCompositions ?? (_behaviorCompositions = new List<BehaviorComposition>()); }
            set { _behaviorCompositions = value; }
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", Participant, String.Concat(BehaviorCompositions));
        }
    }
}