using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public class BehaviorService : IBehaviorService
    {
        private Dictionary<IParticipant, ParticipantBehaviorComposition> _participantBehaviorCompositions;
        private Dictionary<IParticipant, IList<Behavior>> _participantBehaviors;

        public BehaviorModel From(ScenarioModel scenarioModel)
        {
            Init();
            var behaviorModel = new BehaviorModel();
            foreach (var scenario in scenarioModel.Scenarios)
            {
                CreateBehaviorsFor(scenario.Invocations);

                foreach (var participant in scenario.ReceiverParticipants)
                {
                    var participantBehavior = CreateOrLookupParticipantBehaviorComposition(participant);
                    behaviorModel.ParticipantCompositions.Add(participantBehavior);

                    var behaviorComposition = new BehaviorComposition
                    {
                        Name = scenario.Name,
                        Behaviors = _participantBehaviors[participant]
                    };
                    participantBehavior.BehaviorCompositions.Add(behaviorComposition);
                }
            }
            return behaviorModel;
        }

        private void CreateBehaviorsFor(IEnumerable<ScenarioOperationInvocation> invocations)
        {
            foreach (var invocation in invocations)
            {
                var inMessage = CreateMessageTripleFrom(invocation);
                var behavior = CreateBehaviorFrom(invocation);
                behavior.InMessageTriple = inMessage;

                IList<Behavior> behaviors = null;
                var receiverBehaviorExists = _participantBehaviors.TryGetValue(invocation.Receiver, out behaviors);
                if (!receiverBehaviorExists)
                {
                    behaviors = new List<Behavior>();
                    _participantBehaviors.Add(invocation.Receiver, behaviors);
                }
                behaviors.Add(behavior);


                if (_participantBehaviors.ContainsKey(invocation.Sender))
                {
                    var outMessage = CreateMessageTripleFrom(invocation);
                    var senderBehavior = _participantBehaviors[invocation.Sender].Last();
                    senderBehavior.OutMessages.Add(outMessage);
                }
            }
        }

        private Behavior CreateBehaviorFrom(ScenarioOperationInvocation invocation)
        {
            return new Behavior
            {
                PreState = invocation.PreScenarioStateInvariant.Name,
                PostState = invocation.PostScenarioStateInvariant.Name
            };
        }

        private MessageTriple CreateMessageTripleFrom(ScenarioOperationInvocation invocation)
        {
            return new MessageTriple
            {
                Operation = invocation.ScenarioOperation.Name,
                Return = invocation.Return,
                Target = invocation.Receiver
            };
        }

        private ParticipantBehaviorComposition CreateOrLookupParticipantBehaviorComposition(IParticipant participant)
        {
            ParticipantBehaviorComposition participantBehavior;
            var participantExists = _participantBehaviorCompositions.TryGetValue(participant, out participantBehavior);
            if (!participantExists)
            {
                participantBehavior = new ParticipantBehaviorComposition {Participant = participant};
                _participantBehaviorCompositions.Add(participant, participantBehavior);
            }
            return participantBehavior;
        }

        private void Init()
        {
            _participantBehaviorCompositions = new Dictionary<IParticipant, ParticipantBehaviorComposition>();
            _participantBehaviors = new Dictionary<IParticipant, IList<Behavior>>();
        }
    }
}