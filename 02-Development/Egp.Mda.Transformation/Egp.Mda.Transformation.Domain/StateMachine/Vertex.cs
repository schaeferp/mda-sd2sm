using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.StateMachine
{
    public abstract class Vertex
    {
        public IEnumerable<Transition> Outgoing { get; protected set; }
    }
}