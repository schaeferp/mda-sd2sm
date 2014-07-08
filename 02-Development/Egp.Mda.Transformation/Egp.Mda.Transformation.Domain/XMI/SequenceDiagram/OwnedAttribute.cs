using System;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     Represents an OwnedAttribute (XMI) node.
    /// </summary>
    public class OwnedAttribute
    {
        /// <summary>
        ///     This OwnedAttribute's XMI-Id.
        /// </summary>
        public string XmiId { get; set; }

        /// <summary>
        ///     This OwnedAttribute's XMI-type.
        /// </summary>
        public string XmiType { get; set; }

        /// <summary>
        ///     This OwnedAttribute's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     This OwnedAttribute's type.
        /// </summary>
        public String Type { get; set; }

        public override string ToString()
        {
            return XmiId;
        }
    }
}