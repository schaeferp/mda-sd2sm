using System.IO;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    /// <summary>
    ///     Provides functionality to deserialize XMI data into an intermediate
    ///     format which may be passed to further processing measures.
    ///     The expected XMI version/relevant data may vary by implementation.
    /// </summary>
    public interface IXmiDeserializer
    {
        /// <summary>
        ///     Transforms XMI data (given as an argument) into an <see cref="XmiSequenceDiagramModel" />.
        /// </summary>
        /// <param name="xmi">The XMI input stream.</param>
        /// <returns>A model reflecting the information contained in the input XMI.</returns>
        XmiSequenceDiagramModel SequenceDiagramFrom(Stream xmi);
    }
}