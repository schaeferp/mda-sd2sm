using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Egp.Mda.Transformation.Core
{
    public class ScenarioService : IScenarioService
    {
        private const string XmiPrefix = "xmi";
        private const string IdAttributeName = "id";
        private const string TypeAttributeName = "type";
        private const string NameAttributeName = "name";
        private const string OwnedAttributeTagName = "ownedAttribute";
        private const string OwnedBehaviorTagName = "ownedBehaviorName";
        private const string UmlPropertyAttributeValue = "uml:Property";
        private const string UmlInteractionAttributeValue = "uml:Interaction";
        private const string UmlActorAttributeValue = "uml:Actor";
        private const string PackagedElementTagName = "packagedElement";

        private XName _xmiTypeAttribute;
        private XName _xmiIdAttribute;

        /// <summary>
        ///     Contains a mapping from id to participant. The id matches the
        ///     global actor identifiers as specified in XMI all the time (invariant, bro!).
        /// </summary>
        private Dictionary<string, IParticipant> _actors;

        public IEnumerable<Scenario> From(Stream xmiStream)
        {
            var document = XDocument.Load(xmiStream);
            _xmiIdAttribute = LookupXName(XmiPrefix, IdAttributeName, document);
            _xmiTypeAttribute = LookupXName(XmiPrefix, TypeAttributeName, document);
            _actors = ReadActors(document);
            var scenarios = ReadInteractionNodes(document).Select(node => ReadScenario(node)).ToList();

            return null;
        }

        private Scenario ReadScenario(XElement node)
        {
            if (null == node) throw new ArgumentNullException("node");
            var name = node.Attribute(NameAttributeName).Value;
            var participants = ReadParticipants(node);
            return null;
        }

        /// <summary>
        ///     Reads the participants which are stored in the sequence diagram identified by <paramref name="scenarioNode" />.
        /// </summary>
        /// <param name="scenarioNode">The XMI representing the sequence diagram.</param>
        /// <returns>All participants for the given sequence diagram.</returns>
        private Dictionary<string, IParticipant> ReadParticipants(XElement scenarioNode)
        {
            var ownedAttributeNodes = scenarioNode.Descendants(OwnedAttributeTagName);
            var lifelineAttributeNodes = ownedAttributeNodes
                .Where(
                    ownedAttribute =>
                        ownedAttribute.Attribute(_xmiTypeAttribute) != null &&
                        ownedAttribute.Attribute(_xmiTypeAttribute).Value == UmlPropertyAttributeValue);
            var participants = new Dictionary<string, IParticipant>(_actors);

            foreach (var node in lifelineAttributeNodes)
            {
                var nodeType = node.Attribute(TypeAttributeName);
                if (null == nodeType)
                {
                    string systemObjectId;
                    var systemObject = CreateSystemObjectFrom(node, out systemObjectId);
                    participants.Add(systemObjectId, systemObject);
                }
                else
                {
                    UpdateParticipant(participants, node, nodeType.Value);
                }
            }
            return participants;
        }

        /// <summary>
        ///     Creates a system object which is a participant by using the given <paramref name="participantNode" /> and
        ///     stores the used identifier in <paramref name="systemObjectId" />.
        /// </summary>
        /// <param name="participantNode">The XMI node representing the participant.</param>
        /// <param name="systemObjectId">The created system object's identifier.</param>
        /// <returns>The created object.</returns>
        private IParticipant CreateSystemObjectFrom(XElement participantNode, out string systemObjectId)
        {
            var xmiId = participantNode.Attribute(_xmiIdAttribute);
            var xmiName = participantNode.Attribute(NameAttributeName);
            if (null == xmiId) throw new ArgumentNullException();
            if (null == xmiName) throw new ArgumentNullException();
            systemObjectId = xmiId.Value;
            return new SystemObject {Name = xmiName.Value};
        }

        /// <summary>
        ///     Determines the actual id of the actor identified by the given <paramref name="oldParticipantId" />, which is valid
        ///     in the current
        ///     sequence diagram. This id will be set in the given <paramref name="participants" /> dict.
        ///     uml:Model
        ///     \_packagedElement xmi:type='uml:Actor' xmi:id name
        ///     \_packagedElement xmi:type='uml:Collaboration'
        ///     \_ownedBehavior name
        ///     \_ownedAttribute xmi:id name
        /// </summary>
        /// <param name="participants">The participants to update.</param>
        /// <param name="ownedAttributeNode">The ownedAttribute participantNode of the current sequence diagram.</param>
        /// <param name="oldParticipantId">The id of the participant to update.</param>
        private void UpdateParticipant(Dictionary<string, IParticipant> participants, XElement ownedAttributeNode,
            string oldParticipantId)
        {
            IParticipant assumedParticipant;
            participants.TryGetValue(oldParticipantId, out assumedParticipant);
            if (null == assumedParticipant) return;
            var nodeXmiId = ownedAttributeNode.Attribute(_xmiIdAttribute);
            if (null == nodeXmiId) return;
            var newParticipantId = nodeXmiId.Value;
            participants.Remove(oldParticipantId);
            participants.Add(newParticipantId, assumedParticipant);
        }

        /// <summary>
        ///     Reads in all uml:Interaction nodes (which represent a single sequence diagram each).
        /// </summary>
        /// <param name="document">XMI document.</param>
        /// <returns>All uml:Interaction nodes.</returns>
        private IEnumerable<XElement> ReadInteractionNodes(XDocument document)
        {
            if (document.Root == null) throw new ArgumentNullException();
            var ownedBehaviorNodes = document.Descendants(OwnedBehaviorTagName);
            return ownedBehaviorNodes
                .Where(
                    elem =>
                        null != elem.Attribute(_xmiTypeAttribute) &&
                        elem.Attribute(_xmiTypeAttribute).Value == UmlInteractionAttributeValue);
        }


        /// <summary>
        ///     Reads in all actors (which are independent of sequence diagram).
        ///     uml:Model
        ///     \_packagedElement xmi:type='uml:Actor' xmi:id name
        /// </summary>
        /// <param name="document">The XMI document.</param>
        /// <returns>A mapping from XMI-IDs to actors.</returns>
        private Dictionary<string, IParticipant> ReadActors(XDocument document)
        {
            if (document.Root == null) throw new ArgumentNullException();
            var packagedElementNodes = document.Descendants(PackagedElementTagName);
            var actorNodes = packagedElementNodes
                .Where(
                    elem =>
                        null != elem.Attribute(_xmiTypeAttribute) &&
                        elem.Attribute(_xmiTypeAttribute).Value == UmlActorAttributeValue);
            var actors = actorNodes
                .Select(
                    elem =>
                        new
                        {
                            Id = elem.Attribute(_xmiIdAttribute).Value,
                            Actor = new Actor {Name = elem.Attribute(NameAttributeName).Value} as IParticipant
                        });
            return actors
                .ToDictionary(key => key.Id, val => val.Actor);
        }

        /// <summary>
        ///     Resolves the given XMLNS prefix to the full qualified URL and
        ///     returns an <see cref="XName" /> including the required attribute.
        /// </summary>
        /// <param name="prefix">XMLNS prefix.</param>
        /// <param name="attribute">XML attribute name.</param>
        /// <param name="document">XMI document.</param>
        /// <returns>XName including resolved XMLNS.</returns>
        private XName LookupXName(string prefix, string attribute, XDocument document)
        {
            if (document.Root == null) throw new ArgumentNullException();
            var namespaceOfPrefix = document.Root.GetNamespaceOfPrefix(prefix);
            if (namespaceOfPrefix == null) throw new ArgumentNullException();
            var ns = namespaceOfPrefix.ToString();
            return XName.Get(attribute, ns);
        }
    }
}