using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.Uml
{
    public abstract class Vertex
    {
        public IList<Transition> Outgoing { get; protected set; }
    }
}