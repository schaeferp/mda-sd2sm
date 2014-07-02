using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.StateMachine
{
    public class Transition
    {
        public Transition(Vertex source, Vertex target, string constraint)
        {
            Source = source;
            Target = target;
            Constraint = constraint;
            Behavior = new List<Call>();
        }

        public Vertex Source { get; set; }
        public Vertex Target { get; set; }
        public string Constraint { get; set; }
        public IList<Call> Behavior { get; private set; }
    }
}