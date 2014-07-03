using System;
using System.Collections.Generic;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public class BehaviorService : IBehaviorService
    {
        private Dictionary<IParticipant, ParticipantBehaviorComposition> _participantBehaviorCompositions;
        private Dictionary<IParticipant, Behavior> _participantActualBehavior;

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
                        Behaviors = CreateBehaviorsFor(scenario.Invocations)
                    };
                    participantBehavior.BehaviorCompositions.Add(behaviorComposition);
                }
            }
            return behaviorModel;
        }

        private List<Behavior> CreateBehaviorsFor(IEnumerable<ScenarioOperationInvocation> invocations)
        {
            var results = new List<Behavior>();
            foreach (var invocation in invocations)
            {
                var inMessage = CreateMessageTripleFrom(invocation);
                var behavior = CreateBehaviorFrom(invocation);
                behavior.InMessageTriple = inMessage;
                _participantActualBehavior.Replace(invocation.Receiver, behavior);
                results.Add(behavior);


                var outMessage = CreateMessageTripleFrom(invocation);
                var senderBehavior = _participantActualBehavior[invocation.Sender];
                senderBehavior.OutMessages.Add(outMessage);
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