using System;
using System.Collections.Generic;
using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public class ScenarioService : IScenarioService
    {
        #region XML Attribute Value Constants

        private const string UmlCollaborationAttributeValue = "uml:Collaboration";
        private const string ReplySortAttributeValue = "reply";

        private const string UmlMessageOccurenceSpecificationAttributeValue =
            "uml:MessageOccurrenceSpecification";

        private const string UmlStateInvariantAttributeValue = "uml:StateInvariant";

        #endregion

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

        /// <summary>
        ///     Creates a <see cref="Scenario" /> based on the given <see cref="PackagedElement" /> object representing
        ///     an entire, single sequence diagram.
        /// </summary>
        /// <param name="sequenceDiagram">The sequence diagram's root node.</param>
        /// <returns>A corresponing scenario.</returns>
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
            AddAnonymousPostStatesToPostStateLessInvocations(lastInvocationPerParticipant);
            return scenario;
        }

        /// <summary>
        ///     Evaluates the Message being described by the given Fragment and enriches both the given invocation dictionary
        ///     and scenario appropriately.
        /// </summary>
        /// <param name="fragment">The Fragment describing the Message to add.</param>
        /// <param name="sequenceDiagram">The PackagedElement describing the containing sequence diagram.</param>
        /// <param name="lastInvocationPerParticipant">The invocation tracker.</param>
        /// <param name="scenario">The scenario to enrich.</param>
        private void AddMessageToInvocations(Fragment fragment, PackagedElement sequenceDiagram,
            IDictionary<IParticipant, ScenarioOperationInvocation> lastInvocationPerParticipant, Scenario scenario)
        {
            var message = LookupMessageFor(sequenceDiagram, fragment.Message);
            var participantIsSender = message.SendEvent == fragment.XmiId;
            var messageIsReply = message.Sort == ReplySortAttributeValue;

            foreach (var lifelineId in fragment.Covered)
            {
                var lifeline = LookupLifelineFor(sequenceDiagram, lifelineId);
                var participant = LookupParticipantFor(sequenceDiagram, lifeline);
                ScenarioOperationInvocation lastInvocation;
                var lastInvocationExists = lastInvocationPerParticipant.TryGetValue(participant, out lastInvocation);

                if (lastInvocationExists && messageIsReply && participantIsSender) lastInvocation.Return = message.PrettyName;

                if (participantIsSender)
                {
                    _lastSender = participant;
                    continue;
                }

                if (messageIsReply) continue;

                ScenarioOperationInvocation newInvocation;
                if (lastInvocationExists)
                {
                    newInvocation = UpdateCurrentParticipantInvocation(message, lastInvocationPerParticipant,
                        participant,
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

        /// <summary>
        ///     Updates the Invocation of the given Participant being currently evaluated.
        /// </summary>
        /// <param name="message">The Message to derive an Operation for the current Invocation from.</param>
        /// <param name="lastInvocationPerParticipant">The Invocation tracker.</param>
        /// <param name="participant">The corresponding Participant.</param>
        /// <param name="lastInvocation">The last Invocation that occurred in here.</param>
        /// <param name="scenario">The containing Scenario.</param>
        /// <returns></returns>
        private ScenarioOperationInvocation UpdateCurrentParticipantInvocation(Message message,
            IDictionary<IParticipant, ScenarioOperationInvocation> lastInvocationPerParticipant,
            IParticipant participant,
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

            lastInvocation.ScenarioOperation = CreateOrLookOperation(participant, message.PrettyName);
            scenario.Invocations.Add(lastInvocation);
            return lastInvocation;
        }

        /// <summary>
        ///     Creates a new Invocation for the given Participant sending the given Message.
        /// </summary>
        /// <param name="message">The Message to derive an Operation from.</param>
        /// <param name="lastInvocationPerParticipant">The invocation tracker to update.</param>
        /// <param name="participant">The corresponding Participant.</param>
        /// <param name="scenario">The scenario to add the invocation to.</param>
        /// <returns>The created Invocation.</returns>
        private ScenarioOperationInvocation CreateCurrentParticipantInvocation(Message message,
            IDictionary<IParticipant, ScenarioOperationInvocation> lastInvocationPerParticipant,
            IParticipant participant,
            Scenario scenario)
        {
            var preState = ScenarioStateInvariant.CreateAnonymous();
            var operation = CreateOrLookOperation(participant, message.PrettyName);
            var invocation = new ScenarioOperationInvocation
            {
                PreScenarioStateInvariant = preState,
                ScenarioOperation = operation
            };
            lastInvocationPerParticipant.Add(participant, invocation);
            scenario.Invocations.Add(invocation);
            return invocation;
        }

        /// <summary>
        ///     Adds the state invariant decribed by the given Fragment, within the sequence diagram described by the
        ///     given PackagedElement, to the Invocation dictionary.
        /// </summary>
        /// <param name="fragment">The Fragment describing the state invariant.</param>
        /// <param name="sequenceDiagram">The PackagedElement describing the sequence diagram.</param>
        /// <param name="lastInvocationPerParticipant">The invocations to update.</param>
        private void AddStateInvariantToInvocations(Fragment fragment, PackagedElement sequenceDiagram,
            IDictionary<IParticipant, ScenarioOperationInvocation> lastInvocationPerParticipant)
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

        /// <summary>
        ///     Adds an anonymous post state to each invocation within the given dictionary, that does not
        ///     yet have any post state.
        /// </summary>
        /// <param name="lastInvocationPerParticipant">The invocations to update.</param>
        private static void AddAnonymousPostStatesToPostStateLessInvocations(
            Dictionary<IParticipant, ScenarioOperationInvocation> lastInvocationPerParticipant)
        {
            var invocationsWithoutPostState = lastInvocationPerParticipant.Values.Where(
                lastInvocation => null == lastInvocation.PostScenarioStateInvariant);
            foreach (var lastInvocation in invocationsWithoutPostState)
            {
                lastInvocation.PostScenarioStateInvariant = ScenarioStateInvariant.CreateAnonymous();
            }
        }

        /// <summary>
        ///     Creates or looks up the Operation identified by the given operation name and the given Participant.
        ///     If the given Operation already exists for the given Particpant, it will be returned.
        ///     If it does not yet exist, it will be created, stored and returned.
        /// </summary>
        /// <param name="participant">The Participant executing the Operation.</param>
        /// <param name="operationName">The required Operation's name.</param>
        /// <returns>A corresponding Scenario Operation.</returns>
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

        /// <summary>
        ///     Field initialization.
        /// </summary>
        /// <param name="xmiModel">The input sequence diagram model.</param>
        private void Init(XmiSequenceDiagramModel xmiModel)
        {
            _xmiModel = xmiModel;
            _participants = new Dictionary<Lifeline, IParticipant>();
            _participantOperations = new Dictionary<IParticipant, IDictionary<string, ScenarioOperation>>();
        }

        /// <summary>
        ///     Looks up and returns a Lifeline within a given <see cref="PackagedElement" /> describing a sequence diagram
        ///     and identified by the given lifeline id.
        /// </summary>
        /// <param name="sequenceDiagram">The PackagedElement describing the containing sequence diagram.</param>
        /// <param name="lifelineId">The required Lifeline's id.</param>
        /// <returns>A corresponding lifeline.</returns>
        private static Lifeline LookupLifelineFor(PackagedElement sequenceDiagram, string lifelineId)
        {
            Lifeline lifeline;
            var exists = sequenceDiagram.Lifelines.TryGetValue(lifelineId, out lifeline);
            if (!exists) throw new ArgumentException();
            return lifeline;
        }

        /// <summary>
        ///     Looksup and returns a Participant within a given <see cref="PackagedElement" /> describing a sequence diagram
        ///     and identified by the given lifeline.
        /// </summary>
        /// <param name="sequenceDiagram">The PackagedElement describing the containing sequence diagram.</param>
        /// <param name="lifeline">The lifeline identifing the required Participant.</param>
        /// <returns>A correponding Participant.</returns>
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

        /// <summary>
        ///     Looks up and returns a Message within a given <see cref="PackagedElement" /> describing a sequence diagram
        ///     and identified by the given message id.
        /// </summary>
        /// <param name="sequenceDiagram">The PackagedElement describing the containing sequence diagram.</param>
        /// <param name="messageId">The required message's id.</param>
        /// <returns>A corresponding Message.</returns>
        private static Message LookupMessageFor(PackagedElement sequenceDiagram, string messageId)
        {
            Message message;
            var exists = sequenceDiagram.Messages.TryGetValue(messageId, out message);
            if (!exists) throw new ArgumentException();
            return message;
        }
    }
}