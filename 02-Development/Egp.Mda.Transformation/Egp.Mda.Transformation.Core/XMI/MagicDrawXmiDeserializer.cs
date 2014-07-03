using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public class MagicDrawXmiDeserializer : XmiDeserializerBase
    {
        private const string XmiPrefix = "xmi";
        private const string UmlPrefix = "uml";
        private const string IdAttributeName = "id";
        private const string TypeAttributeName = "type";
        private const string NameAttributeName = "name";
        private const string OwnedAttributeTagName = "ownedAttribute";
        private const string PackagedElementTagName = "packagedElement";
        private const string LifelineTagName = "lifeline";
        private const string MessageTagName = "message";
        private const string MessageSortAttributeName = "messageSort";
        private const string FragmentTagName = "fragment";
        private const string MessageAttributeName = "message";
        private const string CoveredTagName = "covered";
        private const string RepresentsAttributeName = "represents";
        private const string ModelTagName = "Model";
        private const string IdRefAttributeName = "idref";
        private const string BodyTagName = "body";
        private const string ReceiveEventAttributeName = "receiveEvent";
        private const string SendEventAttributeName = "sendEvent";
        private XName _xmiTypeAttribute;
        private XName _xmiIdAttribute;
        private XName _xmiIdRefAttribute;
        private XName _umlModel;

        protected override XmiSequenceDiagramModel From(XDocument document)
        {
            Init(document);
            return new XmiSequenceDiagramModel {PackagedElements = FetchPackagedElements(document)};
        }

        private IDictionary<string, PackagedElement> FetchPackagedElements(XDocument document)
        {
            var umlModel = document.Descendants(_umlModel);
            var xPackagedElements = umlModel.Descendants(PackagedElementTagName);
            return xPackagedElements.Select(xElement =>
                new PackagedElement
                {
                    XmiId = ValueOrDefault(xElement.Attribute(_xmiIdAttribute)),
                    Name = ValueOrDefault(xElement.Attribute(NameAttributeName)),
                    XmiType = ValueOrDefault(xElement.Attribute(_xmiTypeAttribute)),
                    OwnedAttributes = FetchOwnedAttributesFor(xElement),
                    Fragments = FetchFragmentsFor(xElement),
                    Lifelines = FetchLifelinesFor(xElement),
                    Messages = FetchMessagesFor(xElement)
                }
                ).ToDictionary(key => key.XmiId);
        }

        private IDictionary<string, Message> FetchMessagesFor(XElement xElement)
        {
            var xMessages = xElement.Descendants(MessageTagName);
            return xMessages.Select(xMessage => new Message
            {
                XmiId = xMessage.Attribute(_xmiIdAttribute).Value,
                Name = ValueOrDefault(xMessage.Attribute(NameAttributeName)),
                XmiType = xMessage.Attribute(_xmiTypeAttribute).Value,
                ReceiveEvent = xMessage.Attribute(ReceiveEventAttributeName).Value,
                SendEvent = xMessage.Attribute(SendEventAttributeName).Value,
                Sort = ValueOrDefault(xMessage.Attribute(MessageSortAttributeName))
            }).ToDictionary(ownedAttribute => ownedAttribute.XmiId);
        }

        private IDictionary<string, Fragment> FetchFragmentsFor(XElement xElement)
        {
            var result = new Dictionary<string, Fragment>();
            var xFragments = xElement.Descendants(FragmentTagName);
            foreach (var xFragment in xFragments)
            {
                var fragment = new Fragment
                {
                    XmiId = xFragment.Attribute(_xmiIdAttribute).Value,
                    XmiType = xFragment.Attribute(_xmiTypeAttribute).Value,
                    Body = ValueOrDefault(xFragment.Descendants(BodyTagName).FirstOrDefault()),
                    Message = ValueOrDefault(xFragment.Attribute(MessageAttributeName))
                };
                var xCovereds = xFragment.Descendants(CoveredTagName);
                foreach (var xCovered in xCovereds) fragment.Covered.Add(xCovered.Attribute(_xmiIdRefAttribute).Value);
                result.Add(fragment.XmiId, fragment);
            }
            return result;
        }

        private IDictionary<string, OwnedAttribute> FetchOwnedAttributesFor(XElement xElement)
        {
            var xOwnedAttributes = xElement.Descendants(OwnedAttributeTagName);
            return xOwnedAttributes.Select(xOwnedAttribute => new OwnedAttribute
            {
                XmiId = ValueOrDefault(xOwnedAttribute.Attribute(_xmiIdAttribute)),
                Name = ValueOrDefault(xOwnedAttribute.Attribute(NameAttributeName)),
                XmiType = ValueOrDefault(xOwnedAttribute.Attribute(_xmiTypeAttribute)),
                Type = ValueOrDefault(xOwnedAttribute.Attribute(TypeAttributeName))
            }).ToDictionary(ownedAttribute => ownedAttribute.XmiId);
        }

        private IDictionary<string, Lifeline> FetchLifelinesFor(XElement xElement)
        {
            var xLifelines = xElement.Descendants(LifelineTagName);
            return xLifelines.Select(xLifeline => new Lifeline
            {
                XmiId = xLifeline.Attribute(_xmiIdAttribute).Value,
                XmiType = xLifeline.Attribute(_xmiTypeAttribute).Value,
                Represents = xLifeline.Attribute(RepresentsAttributeName).Value
            }).ToDictionary(ownedAttribute => ownedAttribute.XmiId);
        }

        private void Init(XDocument document)
        {
            _xmiIdAttribute = LookupXName(XmiPrefix, IdAttributeName, document);
            _xmiTypeAttribute = LookupXName(XmiPrefix, TypeAttributeName, document);
            _xmiIdRefAttribute = LookupXName(XmiPrefix, IdRefAttributeName, document);
            _umlModel = LookupXName(UmlPrefix, ModelTagName, document);
        }

        private static string ValueOrDefault(XAttribute attribute, string defaultVal = "")
        {
            return null == attribute
                ? defaultVal
                : attribute.Value;
        }

        private static string ValueOrDefault(XElement element, string defaultVal = "")
        {
            return null == element
                ? defaultVal
                : element.Value;
        }
    }
}