using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     Represents a PackagedElement (XMI) node.
    /// </summary>
    public class PackagedElement
    {
        private IDictionary<string, Fragment> _fragments;
        private IDictionary<string, Lifeline> _lifelines;
        private IDictionary<string, Message> _messages;
        private IDictionary<string, OwnedAttribute> _ownedAttributes;

        /// <summary>
        ///     This PackagedElement's XMI-Id.
        /// </summary>
        public string XmiId { get; set; }

        /// <summary>
        ///     This PackagedElement's XMI-type.
        /// </summary>
        public string XmiType { get; set; }

        /// <summary>
        ///     This PackagedElement's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     All OwnedAttributes within this PackagedElement's scope, with their XMI-Id as a key.
        /// </summary>
        public IDictionary<string, OwnedAttribute> OwnedAttributes
        {
            get { return _ownedAttributes ?? (_ownedAttributes = new Dictionary<string, OwnedAttribute>()); }
            set { _ownedAttributes = value; }
        }

        /// <summary>
        ///     All Fragments within this PackagedElement's scope, with their XMI-Id as a key.
        /// </summary>
        public IDictionary<string, Fragment> Fragments
        {
            get { return _fragments ?? (_fragments = new Dictionary<string, Fragment>()); }
            set { _fragments = value; }
        }

        /// <summary>
        ///     All Lifelines within this PackagedElement's scope, with their XMI-Id as a key.
        /// </summary>
        public IDictionary<string, Lifeline> Lifelines
        {
            get { return _lifelines ?? (_lifelines = new Dictionary<string, Lifeline>()); }
            set { _lifelines = value; }
        }

        /// <summary>
        ///     All Messages within this PackagedElement's scope, with their XMI-Id as a key.
        /// </summary>
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