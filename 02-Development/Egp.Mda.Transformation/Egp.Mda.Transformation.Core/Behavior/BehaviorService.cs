using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public class BehaviorService : IBehaviorService
    {
        private Dictionary<IParticipant, ParticipantBehaviorComposition> _participantBehaviorCompositions;

        public BehaviorModel From(ScenarioModel scenarioModel)
        {
            var behaviorModel = new BehaviorModel();
            foreach (Scenario scenario in scenarioModel.Scenarios)
            {
                Init();
                IDictionary<IParticipant, IList<Behavior>> participantBehaviors =
                    CreateBehaviorsFor(scenario.Invocations);

                foreach (IParticipant participant in scenario.ReceiverParticipants)
                {
                    ParticipantBehaviorComposition participantBehavior =
                        CreateOrLookupParticipantBehaviorComposition(participant);
                    behaviorModel.ParticipantCompositions.Add(participantBehavior);

                    var behaviorComposition = new BehaviorComposition
                    {
                        Name = scenario.Name,
                        Behaviors = participantBehaviors[participant]
                    };
                    participantBehavior.BehaviorCompositions.Add(behaviorComposition);
                }
            }
            return behaviorModel;
        }

        /// <summary>
        ///     Creates <see cref="Behavior" />s for the given IEnumerable on <see cref="ScenarioOperationInvocation" />s. These
        ///     <see cref="Behavior" />s are returned as Dictionary, whereas the associated participant serves as the key and a
        ///     list of behaviors as value.
        ///     Participants having the same name will still occur twice in the resulting dictionary. Hence the object reference is
        ///     used as unambigious identification.
        /// </summary>
        /// <param name="invocations">IEnumerable of invocations which should be transformed to a collection of behaviors</param>
        /// <returns>collection of participants, grouped by participants respectively</returns>
        private IDictionary<IParticipant, IList<Behavior>> CreateBehaviorsFor(
            IEnumerable<ScenarioOperationInvocation> invocations)
        {
            var participantBehaviors = new Dictionary<IParticipant, IList<Behavior>>();
            foreach (ScenarioOperationInvocation invocation in invocations)
            {
                MessageTriple inMessage = CreateMessageTripleFrom(invocation);
                Behavior behavior = CreateBehaviorFrom(invocation);
                behavior.InMessageTriple = inMessage;

                IList<Behavior> behaviors = null;
                bool receiverBehaviorExists = participantBehaviors.TryGetValue(invocation.Receiver, out behaviors);
                if (!receiverBehaviorExists)
                {
                    behaviors = new List<Behavior>();
                    participantBehaviors.Add(invocation.Receiver, behaviors);
                }
                behaviors.Add(behavior);


                if (participantBehaviors.ContainsKey(invocation.Sender))
                {
                    MessageTriple outMessage = CreateMessageTripleFrom(invocation);
                    var senderBehavior = participantBehaviors[invocation.Sender].Last();
                    senderBehavior.OutMessages.Add(outMessage);
                }
            }
            return participantBehaviors;
        }

        /// <summary>
        ///     Converts the given <paramref name="invocation" /> into a <see cref="Behavior" />, by setting the pre- and
        ///     poststate-invariant, given by invocation.
        /// </summary>
        /// <param name="invocation">invocation containing pre- and poststate-invariant</param>
        /// <returns>new behavior having the pre- and poststate set</returns>
        private Behavior CreateBehaviorFrom(ScenarioOperationInvocation invocation)
        {
            return new Behavior
            {
                PreState = invocation.PreScenarioStateInvariant.Name,
                PostState = invocation.PostScenarioStateInvariant.Name
            };
        }

        /// <summary>
        ///     Creates a new <see cref="MessageTriple" /> based on the given invocation, which will be used later on as in- or
        ///     outmessage in a newly created behavior.
        /// </summary>
        /// <param name="invocation">invocation containing information about the operation invoked</param>
        /// <returns><see cref="MessageTriple" /> representing the operation invocation of the scenario</returns>
        private MessageTriple CreateMessageTripleFrom(ScenarioOperationInvocation invocation)
        {
            return new MessageTriple
            {
                Operation = invocation.ScenarioOperation.Name,
                Return = invocation.Return,
                Target = invocation.Receiver
            };
        }

        /// <summary>
        ///     Returns the appropriate ParticipantBehaviorComposition for the given participant. These method guarantees that the
        ///     same
        ///     <param name="participant" />
        ///     results to the same <see cref="ParticipantBehaviorComposition" />. Therefore internally a dicitionary is used and
        ///     only a new object created if none exists for the given
        ///     <param name="participant"></param>
        /// </summary>
        /// <param name="participant">participant for which the according <see cref="ParticipantBehaviorComposition"/> is needed</param>
        /// <returns>a newly created or cached <see cref="ParticipantBehaviorComposition"/> for the given <param name="participant"></param></returns>
        private ParticipantBehaviorComposition CreateOrLookupParticipantBehaviorComposition(IParticipant participant)
        {
            ParticipantBehaviorComposition participantBehavior;
            bool participantExists = _participantBehaviorCompositions.TryGetValue(participant, out participantBehavior);
            if (!participantExists)
            {
                participantBehavior = new ParticipantBehaviorComposition {Participant = participant};
                _participantBehaviorCompositions.Add(participant, participantBehavior);
            }
            return participantBehavior;
        }

        /// <summary>
        /// Initializes used caches to ensure to encapsulate each scenario from each, as they are not intended to be merged in any way yet.
        /// </summary>
        private void Init()
        {
            _participantBehaviorCompositions = new Dictionary<IParticipant, ParticipantBehaviorComposition>();
        }
    }
}