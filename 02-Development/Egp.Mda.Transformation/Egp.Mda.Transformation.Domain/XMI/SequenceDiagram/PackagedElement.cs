using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class PackagedElement
    {
        private IDictionary<string, Fragment> _fragments;
        private IDictionary<string, Lifeline> _lifelines;
        private IDictionary<string, Message> _messages;
        private IDictionary<string, OwnedAttribute> _ownedAttributes;

        public string XmiId { get; set; }
        public string XmiType { get; set; }
        public string Name { get; set; }

        public IDictionary<string, OwnedAttribute> OwnedAttributes
        {
            get { return _ownedAttributes ?? (_ownedAttributes = new Dictionary<string, OwnedAttribute>()); }
            set { _ownedAttributes = value; }
        }

        public IDictionary<string, Fragment> Fragments
        {
            get { return _fragments ?? (_fragments = new Dictionary<string, Fragment>()); }
            set { _fragments = value; }
        }

        public IDictionary<string, Lifeline> Lifelines
        {
            get { return _lifelines ?? (_lifelines = new Dictionary<string, Lifeline>()); }
            set { _lifelines = value; }
        }

        public IDictionary<string, Message> Messages
        {
            get { return _messages ?? (_messages = new Dictionary<string, Message>()); }
            set { _messages = value; }
        }

        public override string ToString()
        {
            return XmiId;
        }
    }
}