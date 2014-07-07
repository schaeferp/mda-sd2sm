using System;
using System.Collections.Generic;
using System.Linq;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     Model representing the behaviors for multiple scenarios as well as multiple participants.
    /// </summary>
    public class BehaviorModel
    {
        private IList<ParticipantBehaviorComposition> _participantCompositions;

        /// <summary>
        ///     Collection of <see cref="ParticipantBehaviorComposition" />s, storing all <see cref="BehaviorComposition" />s for
        ///     one participant. One participant is identified by it's object reference and hence may be duplicated in this
        ///     collection.
        /// </summary>
        public IList<ParticipantBehaviorComposition> ParticipantCompositions
        {
            get
            {
                return _participantCompositions ??
                       (_participantCompositions = new List<ParticipantBehaviorComposition>());
            }
        }

        /// <summary>
        ///     Collection of <see cref="ParticipantBehaviorComposition" />s, storing all <see cref="BehaviorComposition" />s for
        ///     one participant. One participant is identified by it's name and hence no duplicates occure in this
        ///     collection. Therefore the first participant found is returned and associated to the appropriate
        ///     <see cref="BehaviorComposition" />s. All other <see cref="IParticipant" />'s information from other participants having the same name are discarded.
        /// </summary>
        public IList<ParticipantBehaviorComposition> DistinctParticipantCompositions
        {
            get
            {
                IEnumerable<IGrouping<string, ParticipantBehaviorComposition>> grouped =
                    ParticipantCompositions.GroupBy(compo => compo.Participant.Name);
                return grouped.Select(
                    g =>
                        new ParticipantBehaviorComposition
                        {
                            Participant = g.ToList().First().Participant,
                            BehaviorCompositions = g.ToList().SelectMany(e => e.BehaviorCompositions).ToList()
                        }).ToList();
            }
        }

        /// <summary>
        /// Accessing the <see cref="ParticipantBehaviorComposition"/> for the given particpant. Resulting in alle <see cref="BehaviorComposition"/>s for the given <param name="participant"></param>.
        /// </summary>
        /// <param name="participant">participant whose <see cref="ParticipantBehaviorComposition"/> are required</param>
        /// <returns>Collection of <see cref="ParticipantBehaviorComposition"/>, as these are identified by object reference, this collection contains usually only one or zero elements</returns>
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