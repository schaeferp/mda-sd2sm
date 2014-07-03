using System;
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

        public IList<ParticipantBehaviorComposition> DistinctParticipantCompositions
        {
            get
            {
                var grouped = ParticipantCompositions.GroupBy(compo => compo.Participant.Name);
                return grouped.Select(
                    g =>
                        new ParticipantBehaviorComposition()
                        {
                            Participant = g.ToList().First().Participant,
                            BehaviorCompositions = g.ToList().SelectMany(e => e.BehaviorCompositions).ToList()
                        }).ToList();
            }
        }

        public IEnumerable<ParticipantBehaviorComposition> this[IParticipant participant]
        {
            get { return ParticipantCompositions.Where(p => p.Participant == participant); }
        }

        public override string ToString()
        {
            return String.Concat(ParticipantCompositions);
        }
    }
}