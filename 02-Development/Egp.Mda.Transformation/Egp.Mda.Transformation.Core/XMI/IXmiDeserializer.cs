using System.IO;
using Egp.Mda.Transformation.Domain.Xmi.SequenceDiagram;

namespace Egp.Mda.Transformation.Core
{
    public interface IXmiDeserializer
    {
        XmiSequenceDiagramModel From(Stream xmi);
    }
}
