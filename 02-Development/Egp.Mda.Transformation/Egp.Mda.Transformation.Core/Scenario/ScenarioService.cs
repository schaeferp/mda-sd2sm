using System;
using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public class ScenarioService : IScenarioService
    {
        private const string UmlCollaborationAttributeValue = "uml:Collaboration";
        private const string ReplySortValue = "reply";

        private const string UmlMessageOccurenceSpecificationAttributeValue =
            "uml:MessageOccurrenceSpecification";

        private const string UmlStateInvariantAttributeValue = "uml:StateInvariant";
        private IParticipant _lastSender;

        private IDictionary<IParticipant, IDictionary<string, ScenarioOperation>> _participantOperations;
        private IDictionary<Lifeline, IParticipant> _participants;

        private XmiSequenceDiagramModel _xmiModel;

        public ScenarioModel From(XmiSequenceDiagramModel xmiModel)
        {
            Init(xmiModel);
            var collaborations =
                xmiModel.PackagedElements.Values.Where(e => e.XmiType == UmlCollaborationAttributeValue).ToList();
            var result = new ScenarioModel();
            result.Scenarios.AddRange(collaborations.Select(CreateScenarioFrom));
            return result;
        }

        private Scenario CreateScenarioFrom(PackagedElement sequenceDiagram)
        {
            var scenario = new Scenario
            {
                Name = sequenceDiagram.Name
            };
            var lastInvocationPerParticipant = new Dictionary<IParticipant, ScenarioOperationInvocation>();
            foreach (var fragment in sequenceDiagram.Fragments.Values)
            {
                switch (fragment.XmiType)
                {
                    case UmlStateInvariantAttributeValue:
                        AddStateInvariantToInvocations(fragment, sequenceDiagram, lastInvocationPerParticipant);
                        break;
                    case UmlMessageOccurenceSpecificationAttributeValue:
                    {
                        AddMessageToInvocations(fragment, sequenceDiagram, lastInvocationPerParticipant, scenario);
                        break;
                    }
                }
            }
            AddPostStatesToLastInvocations(lastInvocationPerParticipant);
            return scenario;
        }

        private void AddMessageToInvocations(Fragment fragment, PackagedElement sequenceDiagram,
            Dictionary<IParticipant, ScenarioOperationInvocation> lastInvocationPerParticipant, Scenario scenario)
        {
            var message = LookupMessageFor(sequenceDiagram, fragment.Message);
            var participantIsSender = message.SendEvent == fragment.XmiId;
            var messageIsReply = message.Sort == ReplySortValue;

            foreach (var lifelineId in fragment.Covered)
            {
                var lifeline = LookupLifelineFor(sequenceDiagram, lifelineId);
                var participant = LookupParticipantFor(sequenceDiagram, lifeline);
                ScenarioOperationInvocation lastInvocation;
                var lastInvocationExists = lastInvocationPerParticipant.TryGetValue(participant, out lastInvocation);

                if (lastInvocationExists && messageIsReply && participantIsSender) lastInvocation.Return = message.Name;

                if (participantIsSender)
                {
                    _lastSender = participant;
                    continue;
                }
                
                if (messageIsReply) continue;

                ScenarioOperationInvocation newInvocation;
                if (lastInvocationExists)
                {
                    newInvocation = UpdateCurrentParticipantInvocation(message, lastInvocationPerParticipant, participant,
                        lastInvocation, scenario);
                }
                else
                {
                    newInvocation = CreateCurrentParticipantInvocation(message, lastInvocationPerParticipant,
                        participant, scenario);
                }
                newInvocation.Sender = _lastSender;
            }
        }

        private ScenarioOperationInvocation UpdateCurrentParticipantInvocation(Message message,
            Dictionary<IParticipant, ScenarioOperationInvocation> lastInvocationPerParticipant, IParticipant participant,
            ScenarioOperationInvocation lastInvocation, Scenario scenario)
        {
            var state = ScenarioStateInvariant.CreateAnonymous();
            var lastInvocationHasOperation = lastInvocation.ScenarioOperation != null;

            if (lastInvocationHasOperation)
            {
                lastInvocation.PostScenarioStateInvariant = state;
                var newInvocation = new ScenarioOperationInvocation
                {
                    PreScenarioStateInvariant = state
                };
                lastInvocationPerParticipant[participant] = newInvocation;
                lastInvocation = newInvocation;
            }

            lastInvocation.ScenarioOperation = CreateOrLookOperation(participant, message.Name);
            scenario.Invocations.Add(lastInvocation);
            return lastInvocation;
        }

        private ScenarioOperationInvocation CreateCurrentParticipantInvocation(Message message,
            Dictionary<IParticipant, ScenarioOperationInvocation> lastInvocationPerParticipant, IParticipant participant,
            Scenario scenario)
        {
            var preState = ScenarioStateInvariant.CreateAnonymous();
            var operation = CreateOrLookOperation(participant, message.Name);
            var invocation = new ScenarioOperationInvocation
            {
                PreScenarioStateInvariant = preState,
                ScenarioOperation = operation
            };
            lastInvocationPerParticipant.Add(participant, invocation);
            scenario.Invocations.Add(invocation);
            return invocation;
        }

        private void AddStateInvariantToInvocations(Fragment fragment, PackagedElement sequenceDiagram,
            Dictionary<IParticipant, ScenarioOperationInvocation> lastInvocationPerParticipant)
        {
            var state = new ScenarioStateInvariant {Name = fragment.Body};
            foreach (var lifelineId in fragment.Covered)
            {
                var lifeline = LookupLifelineFor(sequenceDiagram, lifelineId);
                var participant = LookupParticipantFor(sequenceDiagram, lifeline);
                ScenarioOperationInvocation activeInvocation;
                var activeInvocationExists = lastInvocationPerParticipant.TryGetValue(participant,
                    out activeInvocation);
                if (activeInvocationExists) activeInvocation.PostScenarioStateInvariant = state;
                var newInvocation = new ScenarioOperationInvocation {PreScenarioStateInvariant = state};
                lastInvocationPerParticipant.Remove(participant);
                lastInvocationPerParticipant.Add(participant, newInvocation);
            }
        }

        private void AddPostStatesToLastInvocations(
            Dictionary<IParticipant, ScenarioOperationInvocation> lastInvocationPerParticipant)
        {
            var invocationsWithoutPostState = lastInvocationPerParticipant.Values.Where(
                lastInvocation => null == lastInvocation.PostScenarioStateInvariant);
            foreach (var lastInvocation in invocationsWithoutPostState)
            {
                lastInvocation.PostScenarioStateInvariant = ScenarioStateInvariant.CreateAnonymous();
            }
        }


        private ScenarioOperation CreateOrLookOperation(IParticipant participant, String operationName)
        {
            IDictionary<string, ScenarioOperation> participantOperations;
            var dictionaryExists = _participantOperations.TryGetValue(participant, out participantOperations);
            if (!dictionaryExists)
            {
                participantOperations = new Dictionary<string, ScenarioOperation>();
                _participantOperations.Add(participant, participantOperations);
            }
            ScenarioOperation operation;
            var operationExists = participantOperations.TryGetValue(participant.Name, out operation);
            if (!operationExists)
            {
                operation = new ScenarioOperation
                {
                    Name = operationName,
                    Receiver = participant
                };
            }
            return operation;
        }

        private void Init(XmiSequenceDiagramModel xmiModel)
        {
            _xmiModel = xmiModel;
            _participants = new Dictionary<Lifeline, IParticipant>();
            _participantOperations = new Dictionary<IParticipant, IDictionary<string, ScenarioOperation>>();
        }

        private Lifeline LookupLifelineFor(PackagedElement sequenceDiagram, string lifelineId)
        {
            Lifeline lifeline;
            var exists = sequenceDiagram.Lifelines.TryGetValue(lifelineId, out lifeline);
            if (!exists) throw new ArgumentException();
            return lifeline;
        }

        private IParticipant LookupParticipantFor(PackagedElement sequenceDiagram, Lifeline lifeline)
        {
            IParticipant participant;
            var exists = _participants.TryGetValue(lifeline, out participant);
            if (exists) return participant;
            var ownedAttributeId = lifeline.Represents;
            var ownedAttribute = sequenceDiagram.OwnedAttributes[ownedAttributeId];
            if (String.IsNullOrEmpty(ownedAttribute.Type))
            {
                participant = new SystemObject {Name = ownedAttribute.Name};
            }
            else
            {
                var actorId = ownedAttribute.Type;
                var packagedElementActor = _xmiModel.PackagedElements[actorId];
                participant = new Actor {Name = packagedElementActor.Name};
            }
            _participants.Add(lifeline, participant);
            return participant;
        }

        private Message LookupMessageFor(PackagedElement sequenceDiagram, string messageId)
        {
            Message message;
            var exists = sequenceDiagram.Messages.TryGetValue(messageId, out message);
            if (!exists) throw new ArgumentException();
            return message;
        }
    }
}