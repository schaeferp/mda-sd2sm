using System;
using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain.Common;
using Egp.Mda.Transformation.Domain.Scenario;
using Egp.Mda.Transformation.Domain.Xmi.SequenceDiagram;

namespace Egp.Mda.Transformation.Core
{
    public class ScenarioService : IScenarioService
    {
        private const string UmlCollaborationAttributeValue = "uml:Collaboration";
        private const string ReplySortValue = "reply";

        private const string UmlMessageOccurenceSpecificationAttributeValue =
            "uml:MessageOccurrenceSpecification";

        private const string UmlStateInvariantAttributeValue = "uml:StateInvariant";

        private XmiSequenceDiagramModel _xmiModel;
        private IDictionary<Lifeline, IParticipant> _participants;

        public IEnumerable<Scenario> From(XmiSequenceDiagramModel xmiModel)
        {
            Init(xmiModel);
            var collaborations =
                xmiModel.PackagedElements.Values.Where(e => e.XmiType == UmlCollaborationAttributeValue).ToList();
            var result = new List<Scenario>(collaborations.Count);
            result.AddRange(collaborations.Select(CreateScenarioFrom));
            return result;
        }

        private Scenario CreateScenarioFrom(PackagedElement sequenceDiagram)
        {
            var scenario = new Scenario
            {
                Name = sequenceDiagram.Name
            };
            var lastInvocationPerParticipant = new Dictionary<IParticipant, OperationInvocation>();
            foreach (var fragment in sequenceDiagram.Fragments.Values)
            {
                switch (fragment.XmiType)
                {
                    case UmlStateInvariantAttributeValue: AddStateInvariantToInvocations(fragment, sequenceDiagram, lastInvocationPerParticipant);
                        break;
                    case UmlMessageOccurenceSpecificationAttributeValue:
                    {
                        OperationInvocation lastInvocation;
                        AddMessageToInvocations(fragment, sequenceDiagram, lastInvocationPerParticipant, scenario);
                        break;
                    }
                }
            }
            AddPostStatesToLastInvocations(lastInvocationPerParticipant);
            return scenario;
        }

        private void AddMessageToInvocations(Fragment fragment, PackagedElement sequenceDiagram, Dictionary<IParticipant, OperationInvocation> lastInvocationPerParticipant, Scenario scenario)
        {
            var message = LookupMessageFor(sequenceDiagram, fragment.Message);
            var participantIsSender = message.SendEvent == fragment.XmiId;
            if (participantIsSender) return;

            foreach (var lifelineId in fragment.Covered)
            {
                var lifeline = LookupLifelineFor(sequenceDiagram, lifelineId);
                var participant = LookupParticipantFor(sequenceDiagram, lifeline);
                OperationInvocation lastInvocation;
                var lastInvocationExists = lastInvocationPerParticipant.TryGetValue(participant, out lastInvocation);
                if (lastInvocationExists)
                {
                    UpdateCurrentParticipantInvocation(message, lastInvocationPerParticipant, participant,lastInvocation, scenario);
                }
                else
                {
                    CreateCurrentParticipantInvocation(message, lastInvocationPerParticipant, participant, scenario);
                }
            }
        }

        private void UpdateCurrentParticipantInvocation(Message message, Dictionary<IParticipant, OperationInvocation> lastInvocationPerParticipant, IParticipant participant, OperationInvocation lastInvocation, Scenario scenario)
        {
            var state = StateInvariant.CreateAnonymous();
            var lastInvocationHasOperation = lastInvocation.Operation != null;
            var messageIsReply = message.Sort == ReplySortValue;
            if (lastInvocationHasOperation && !messageIsReply)
            {
                lastInvocation.PostStateInvariant = state;
                var newInvocation = new OperationInvocation()
                {
                    PreStateInvariant = state
                };
                lastInvocationPerParticipant[participant] = newInvocation;
                lastInvocation = newInvocation;
            }

            if (messageIsReply)
            {
                lastInvocation.Return = message.Name;
            }
            else
            {
                lastInvocation.Operation = new Operation()
                {
                    Name = message.Name,
                    Receiver = participant,
                };
                scenario.Invocations.Add(lastInvocation);
            }
        }

        private void CreateCurrentParticipantInvocation(Message message, Dictionary<IParticipant, OperationInvocation> lastInvocationPerParticipant, IParticipant participant, Scenario scenario)
        {
            var preState = StateInvariant.CreateAnonymous();
            var operation = new Operation()
            {
                Name = message.Name,
                Receiver = participant,
            };
            var invocation = new OperationInvocation()
            {
                PreStateInvariant = preState,
                Operation = operation
            };
            lastInvocationPerParticipant.Add(participant, invocation);
            scenario.Invocations.Add(invocation);
        }

        private void AddStateInvariantToInvocations(Fragment fragment, PackagedElement sequenceDiagram, Dictionary<IParticipant, OperationInvocation> lastInvocationPerParticipant)
        {
            var state = new StateInvariant() { Name = fragment.Body };
            foreach (var lifelineId in fragment.Covered)
            {
                var lifeline = LookupLifelineFor(sequenceDiagram, lifelineId);
                var participant = LookupParticipantFor(sequenceDiagram, lifeline);
                OperationInvocation activeInvocation;
                var activeInvocationExists = lastInvocationPerParticipant.TryGetValue(participant,
                    out activeInvocation);
                if (activeInvocationExists) activeInvocation.PostStateInvariant = state;
                var newInvocation = new OperationInvocation() { PreStateInvariant = state };
                lastInvocationPerParticipant.Remove(participant);
                lastInvocationPerParticipant.Add(participant, newInvocation);
            }
        }

        private void AddPostStatesToLastInvocations(
            Dictionary<IParticipant, OperationInvocation> lastInvocationPerParticipant)
        {
            var invocationsWithoutPostState = lastInvocationPerParticipant.Values.Where(
                lastInvocation => null == lastInvocation.PostStateInvariant);
            foreach (var lastInvocation in invocationsWithoutPostState)
            {
                lastInvocation.PostStateInvariant = StateInvariant.CreateAnonymous();
            }
        }

        private void Init(XmiSequenceDiagramModel xmiModel)
        {
            _xmiModel = xmiModel;
            _participants = new Dictionary<Lifeline, IParticipant>();
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
                participant = new SystemObject() {Name = ownedAttribute.Name};
            }
            else
            {
                var actorId = ownedAttribute.Type;
                var packagedElementActor = _xmiModel.PackagedElements[actorId];
                participant = new Actor() {Name = packagedElementActor.Name};
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