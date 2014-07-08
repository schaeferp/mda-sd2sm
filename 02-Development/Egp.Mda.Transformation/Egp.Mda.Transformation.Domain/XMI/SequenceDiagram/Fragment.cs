using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     Represents a Fragment (XMI) node.
    /// </summary>
    public class Fragment
    {
        private IList<string> _covered;

        /// <summary>
        ///     This Fragment's XMI-Id.
        /// </summary>
        public string XmiId { get; set; }

        /// <summary>
        ///     This Fragment's XMI-type.
        /// </summary>
        public string XmiType { get; set; }

        /// <summary>
        ///     This Fragment's Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     This Fragment's Body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///     All Covereds of this Fragment.
        /// </summary>
        public IList<string> Covered
        {
            get { return _covered ?? (_covered = new List<string>()); }
        }

        public override string ToString()
        {
            return XmiId;
        }
    }
}