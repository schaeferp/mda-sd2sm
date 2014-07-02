using System.IO;

namespace Egp.Mda.Transformation.Core
{
    public interface IXmiDeserializer
    {
        XmiSequenceDiagramModel From(Stream xmi);
    }
}
