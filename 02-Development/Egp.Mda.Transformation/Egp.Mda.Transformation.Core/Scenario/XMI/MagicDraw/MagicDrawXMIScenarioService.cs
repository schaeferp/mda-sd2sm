//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Xml.Linq;
//using Egp.Mda.Transformation.Core.Domain;

//namespace Egp.Mda.Transformation.Core
//{
//    //internal class Message
//    //{
//    //    public string XmiId { get; set; }
//    //    public string Name { get; set; }
//    //    public string ReceiveEvent { get; set; }
//    //    public string SendEvent { get; set; }
//    //    public string Sort { get; set; }
//    //}

//    public class MagicDrawXmiScenarioService : XmiScenarioServiceBase
//    {
//        private const string XmiPrefix = "xmi";
//        private const string IdAttributeName = "id";
//        private const string TypeAttributeName = "type";
//        private const string NameAttributeName = "name";
//        private const string OwnedAttributeTagName = "ownedAttribute";
//        private const string OwnedBehaviorTagName = "ownedBehavior";
//        private const string UmlPropertyAttributeValue = "uml:Property";
//        private const string UmlInteractionAttributeValue = "uml:Interaction";
//        private const string UmlActorAttributeValue = "uml:Actor";
//        private const string PackagedElementTagName = "packagedElement";
//        private const string LifelineTagName = "lifeline";
//        private const string MessageTagName = "message";
//        private const string MessageSortAttributeName = "messageSort";
//        private const string FragmentTagName = "fragment";
//        private const string MessageAttributeName = "message";
//        private const string CoveredTagName = "covered";
//        private const string RequestSortValue = "request";
//        private const string ReplySortValue = "reply";

//        private const string UmlMessageOccurenceSpecificationAttributeValue =
//            "uml:MessageOccurrenceSpecification";

//        private const string UmlStateInvariantAttributeValue = "uml:StateInvariant";
//        private const string IdRefAttributeName = "idref";
//        private const string BodyTagName = "body";
//        private const string ReceiveEventAttributeName = "receiveEvent";
//        private const string SendEventAttributeName = "sendEvent";
//        private XName _xmiTypeAttribute;
//        private XName _xmiIdAttribute;
//        private XName _xmiIdRefAttribute;

//        /// <summary>
//        ///     Contains a mapping from id to participant. The id matches the
//        ///     global actor identifiers as specified in XMI all the time (invariant, bro!).
//        /// </summary>
//        private Dictionary<string, IParticipant> _actors;

//        protected override IEnumerable<Scenario> From(XDocument document)
//        {
//            _xmiIdAttribute = LookupXName(XmiPrefix, IdAttributeName, document);
//            _xmiTypeAttribute = LookupXName(XmiPrefix, TypeAttributeName, document);
//            _xmiIdRefAttribute = LookupXName(XmiPrefix, IdRefAttributeName, document);
//            _actors = ReadActors(document);
//            return ReadInteractionNodes(document).Select(ReadScenario).ToList();
//        }

//        private Scenario ReadScenario(XElement node)
//        {
//            if (null == node) throw new ArgumentNullException("node");
//            var name = node.Attribute(NameAttributeName).Value;
//            var participants = FetchParticipantsForDiagram(node);
//            var messages = ReadMessages(node, participants);
//            var operationInvocations = FetchOperationInvocations(node, participants, messages);
//            return new Scenario() {Name = name, Invocations = operationInvocations};
//        }

//        private IEnumerable<OperationInvocation> FetchOperationInvocations(XElement node,
//            Dictionary<string, IParticipant> participants, IDictionary<string, Message> messages)
//        {
//            var results = new List<OperationInvocation>();
//            var lastInvocationPerParticipant = new Dictionary<string, OperationInvocation>();
//            var fragments = node.Descendants(FragmentTagName);
//            foreach (var fragment in fragments)
//            {
//                var fragmentType = fragment.Attribute(_xmiTypeAttribute).Value;
//                var coveredParticipantIds =
//                    fragment.Descendants(CoveredTagName)
//                        .Select(covered => covered.Attribute(_xmiIdRefAttribute).Value);
//                switch (fragmentType)
//                {
//                    case UmlMessageOccurenceSpecificationAttributeValue:
//                    {
//                        var messageId = fragment.Attribute(MessageAttributeName).Value;
//                        var message = messages[messageId];
//                        var participantId = coveredParticipantIds.First();

//                        OperationInvocation invocation;
//                        var invocationExists = lastInvocationPerParticipant.TryGetValue(participantId, out invocation);

//                        if (null != invocation && message.SendEvent == messageId)
//                        {
//                            invocation.Sender = participants[participantId];
//                        }
//                        else
//                        {
//                            StateInvariant stateInvariant = null;
//                            if (invocationExists && invocation.Sender != null && invocation.Operation.Receiver != null)
//                            {
//                                stateInvariant = new StateInvariant { Name = Guid.NewGuid().ToString() };
//                                invocation.PostStateInvariant = stateInvariant;
//                                lastInvocationPerParticipant.Remove(participantId);
//                                invocationExists = false;
//                            }
//                            if (!invocationExists)
//                            {
//                                var preState = stateInvariant ?? new StateInvariant { Name = Guid.NewGuid().ToString() };
//                                invocation = new OperationInvocation { PreStateInvariant = preState };
//                                lastInvocationPerParticipant.Add(participantId, invocation);
//                                results.Add(invocation);
//                            }

//                            invocation.Operation = invocation.Operation ?? new Operation()
//                            {
//                                Name = message.Name,
//                                Kind = (OperationKind)Enum.Parse(typeof(OperationKind), message.Sort, true)
//                            };
//                            invocation.Operation.Receiver = participants[participantId];
//                        }
//                        break;
//                    }
//                    case UmlStateInvariantAttributeValue:
//                    {
//                        var stateName = fragment.Descendants(BodyTagName).First().Value;
//                        var stateInvariant = new StateInvariant {Name = stateName};
//                        foreach (var id in coveredParticipantIds)
//                        {
//                            OperationInvocation lastInvocation = null;
//                            lastInvocationPerParticipant.TryGetValue(id, out lastInvocation);
//                            if (null != lastInvocation)
//                            {
//                                lastInvocation.PostStateInvariant = stateInvariant;
//                                lastInvocationPerParticipant.Remove(id);
//                            }
//                            var invocation = new OperationInvocation {PreStateInvariant = stateInvariant};
//                            lastInvocationPerParticipant.Add(id, invocation);
//                            results.Add(invocation);
//                        }
//                        break;
//                    }
//                }
//            }

//            return results;
//        }

//        private IDictionary<string, Message> ReadMessages(XElement node, Dictionary<string, IParticipant> participants)
//        {
//            if (null == node) throw new ArgumentNullException("node");
//            var messageNodes = node.Descendants(MessageTagName);
//            return messageNodes
//                .Select(
//                    messageNode => new
//                    {
//                        XmiId = messageNode.Attribute(_xmiIdAttribute).Value,
//                        Message = new Message
//                        {
//                            Name = messageNode.Attribute(NameAttributeName).Value,
//                            ReceiveEvent = messageNode.Attribute(ReceiveEventAttributeName).Value,
//                            SendEvent = messageNode.Attribute(SendEventAttributeName).Value,
//                            Sort = MessageSortFromXAttribute(messageNode.Attribute(MessageSortAttributeName))
//                        }
//                    })
//                .ToDictionary(key => key.XmiId, value => value.Message);
//        }

//        private static string MessageSortFromXAttribute(XAttribute attribute)
//        {
//            return null == attribute
//                ? RequestSortValue
//                : ReplySortValue;
//        }

//        private static OperationKind OperationSortFromXAttribute(XAttribute attribute)
//        {
//            if (null == attribute) return OperationKind.Request;
//            return attribute.Value == "reply"
//                ? OperationKind.Reply
//                : OperationKind.Request;
//        }

//        /// <summary>
//        ///     Reads the participants which are stored in the sequence diagram identified by <paramref name="scenarioNode" />.
//        ///     All participants included in the result contain their lifeline identifieres as ids.
//        /// </summary>
//        /// <param name="scenarioNode">The XMI representing the sequence diagram to fetch participants from.</param>
//        /// <returns>All participants for the given sequence diagram.</returns>
//        private Dictionary<string, IParticipant> FetchParticipantsForDiagram(XElement scenarioNode)
//        {
//            var ownedAttributeNodes = scenarioNode.Descendants(OwnedAttributeTagName);
//            var umlPropertyAttributeNodes = ownedAttributeNodes
//                .Where(
//                    ownedAttribute =>
//                        ownedAttribute.Attribute(_xmiTypeAttribute) != null &&
//                        ownedAttribute.Attribute(_xmiTypeAttribute).Value == UmlPropertyAttributeValue);
//            var participants = new Dictionary<string, IParticipant>(_actors);

//            foreach (var node in umlPropertyAttributeNodes)
//            {
//                var nodeType = node.Attribute(TypeAttributeName);
//                if (null == nodeType)
//                {
//                    string systemObjectId;
//                    var systemObject = CreateSystemObjectFrom(node, out systemObjectId);
//                    participants.Add(systemObjectId, systemObject);
//                }
//                else
//                {
//                    UpdateParticipant(participants, node, nodeType.Value);
//                }
//            }

//            var lifelineNodes = scenarioNode.Descendants(LifelineTagName);
//            lifelineNodes.ToList()
//                .ForEach(node => UpdateParticipant(participants, node, node.Attribute("represents").Value));
//            return participants;
//        }

//        /// <summary>
//        ///     Creates a system object which is a participant by using the given <paramref name="participantNode" /> and
//        ///     stores the used identifier in <paramref name="systemObjectId" />.
//        /// </summary>
//        /// <param name="participantNode">The XMI node representing the participant.</param>
//        /// <param name="systemObjectId">The created system object's identifier.</param>
//        /// <returns>The created object.</returns>
//        private IParticipant CreateSystemObjectFrom(XElement participantNode, out string systemObjectId)
//        {
//            var xmiId = participantNode.Attribute(_xmiIdAttribute);
//            var xmiName = participantNode.Attribute(NameAttributeName);
//            if (null == xmiId) throw new ArgumentNullException();
//            if (null == xmiName) throw new ArgumentNullException();
//            systemObjectId = xmiId.Value;
//            return new SystemObject {Name = xmiName.Value};
//        }

//        /// <summary>
//        ///     Determines the actual id of the actor identified by the given <paramref name="oldParticipantId" />, which is valid
//        ///     in the current
//        ///     sequence diagram. This id will be set in the given <paramref name="participants" /> dict.
//        /// </summary>
//        /// <param name="participants">The participants to update.</param>
//        /// <param name="node">The node to retrieve the new id from.</param>
//        /// <param name="oldParticipantId">The id of the participant to update.</param>
//        private void UpdateParticipant(Dictionary<string, IParticipant> participants, XElement node,
//            string oldParticipantId)
//        {
//            IParticipant assumedParticipant;
//            participants.TryGetValue(oldParticipantId, out assumedParticipant);
//            if (null == assumedParticipant) return;
//            var nodeXmiId = node.Attribute(_xmiIdAttribute);
//            if (null == nodeXmiId) return;
//            var newParticipantId = nodeXmiId.Value;
//            participants.Remove(oldParticipantId);
//            participants.Add(newParticipantId, assumedParticipant);
//        }

//        /// <summary>
//        ///     Reads in all uml:Interaction nodes (which represent a single sequence diagram each).
//        /// </summary>
//        /// <param name="document">XMI document.</param>
//        /// <returns>All uml:Interaction nodes.</returns>
//        private IEnumerable<XElement> ReadInteractionNodes(XDocument document)
//        {
//            if (document.Root == null) throw new ArgumentNullException();
//            var ownedBehaviorNodes = document.Descendants(OwnedBehaviorTagName);
//            return ownedBehaviorNodes
//                .Where(
//                    elem =>
//                        null != elem.Attribute(_xmiTypeAttribute) &&
//                        elem.Attribute(_xmiTypeAttribute).Value == UmlInteractionAttributeValue);
//        }


//        /// <summary>
//        ///     Reads in all actors (which are independent of sequence diagram).
//        ///     uml:XmiSequenceDiagramModel
//        ///     \_packagedElement xmi:type='uml:Actor' xmi:id name
//        /// </summary>
//        /// <param name="document">The XMI document.</param>
//        /// <returns>A mapping from XMI-IDs to actors.</returns>
//        private Dictionary<string, IParticipant> ReadActors(XDocument document)
//        {
//            if (document.Root == null) throw new ArgumentNullException();
//            var packagedElementNodes = document.Descendants(PackagedElementTagName);
//            var actorNodes = packagedElementNodes
//                .Where(
//                    elem =>
//                        null != elem.Attribute(_xmiTypeAttribute) &&
//                        elem.Attribute(_xmiTypeAttribute).Value == UmlActorAttributeValue);
//            var actors = actorNodes
//                .Select(
//                    elem =>
//                        new
//                        {
//                            XmiId = elem.Attribute(_xmiIdAttribute).Value,
//                            Actor = new Actor {Name = elem.Attribute(NameAttributeName).Value} as IParticipant
//                        });
//            return actors
//                .ToDictionary(key => key.XmiId, val => val.Actor);
//        }
//    }
//}