using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     Provides a deserialized XMI diagram model representing any relevant XMI information.
    /// </summary>
    public class XmiSequenceDiagramModel
    {
        /// <summary>
        ///     All <see cref="PackagedElement" />s contained, with their XMI-Id as a key.
        /// </summary>
        public IDictionary<string, PackagedElement> PackagedElements { get; set; }
    }
}