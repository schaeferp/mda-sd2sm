using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public abstract class Vertex
    {
        public IList<Uml> Outgoing { get; protected set; }
    }
}