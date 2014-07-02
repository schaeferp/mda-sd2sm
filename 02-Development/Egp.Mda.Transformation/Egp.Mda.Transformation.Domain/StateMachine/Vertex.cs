using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.StateMachine
{
    public abstract class Vertex
    {
        public IList<Transition> Outgoing { get; protected set; }
    }
}