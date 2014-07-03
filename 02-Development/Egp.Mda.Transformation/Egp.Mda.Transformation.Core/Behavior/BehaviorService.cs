using System.Collections.Generic;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public class BehaviorService : IBehaviorService
    {
        private Dictionary<IParticipant, Behavior> _participantActualBehavior;
        private Dictionary<IParticipant, ParticipantBehaviorComposition> _participantBehaviorCompositions;

        public BehaviorModel From(ScenarioModel scenarioModel)
        {
            Init();
            var behaviorModel = new BehaviorModel();
            foreach (var scenario in scenarioModel.Scenarios)
            {
                foreach (var participant in scenario.ReceiverParticipants)
                {
                    var participantBehavior = CreateOrLookupParticipantBehaviorComposition(participant);
                    behaviorModel.ParticipantCompositions.Add(participantBehavior);

                    var behaviorComposition = new BehaviorComposition
                    {
                        Name = scenario.Name,
                        Behaviors = CreateBehaviorsFor(participant, scenario.Invocations)
                    };
                    participantBehavior.BehaviorCompositions.Add(behaviorComposition);
                }
            }
            return behaviorModel;
        }

        private List<Behavior> CreateBehaviorsFor(IParticipant participant, IEnumerable<ScenarioOperationInvocation> invocations)
        {
            var results = new List<Behavior>();
            foreach (var invocation in invocations)
            {
                var inMessage = CreateMessageTripleFrom(invocation);
                var behavior = CreateBehaviorFrom(invocation);
                behavior.InMessageTriple = inMessage;
                _participantActualBehavior.Replace(participant, behavior);
                if(invocation.Receiver == participant) results.Add(behavior);


                if (_participantActualBehavior.ContainsKey(invocation.Sender))
                {
                    var outMessage = CreateMessageTripleFrom(invocation);
                    var senderBehavior = _participantActualBehavior[invocation.Sender];
                    senderBehavior.OutMessages.Add(outMessage);
                }
            }
            return results;
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
            _participantActualBehavior = new Dictionary<IParticipant, Behavior>();
        }
    }
}