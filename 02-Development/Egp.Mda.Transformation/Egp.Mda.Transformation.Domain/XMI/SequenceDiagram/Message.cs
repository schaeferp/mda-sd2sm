using System;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     Represents a Message (XMI) node.
    /// </summary>
    public class Message
    {
        /// <summary>
        ///     This Message's XMI-Id.
        /// </summary>
        public string XmiId { get; set; }

        /// <summary>
        ///     This Message's XMI-type.
        /// </summary>
        public string XmiType { get; set; }

        /// <summary>
        ///     This Message's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This Message's name in pretty style, which means that a colon as prefix is removed.
        /// </summary>
        public string PrettyName
        {
            get
            {
                if (Name.StartsWith(":"))
                {
                    return Name.Substring(1).Trim();
                }
                return Name;
            }
        }

        /// <summary>
        ///     This Message's receive event.
        /// </summary>
        public string ReceiveEvent { get; set; }

        /// <summary>
        ///     This Message's send event.
        /// </summary>
        public string SendEvent { get; set; }

        /// <summary>
        ///     This Message's sort.
        /// </summary>
        public string Sort { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", XmiId, Name);
        }
    }
}