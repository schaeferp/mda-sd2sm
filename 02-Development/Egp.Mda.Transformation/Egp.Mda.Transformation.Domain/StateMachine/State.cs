using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.StateMachine
{
    public class State : Vertex
    {
    }

    public class StableState : State
    {
        public string Name { get; set; }
    }

    public class ActivityState : State, IRegionOwner
    {
        public ActivityState(string name)
        {
            Name = name;
            Regions = new List<Region>();
        }

        public string Name { get; set; }
        public IList<Region> Regions { get; private set; }
    }

    public class ActionState : State
    {
        public ActionState()
        {
            Behavior = new List<Call>();
        }

        public string Constraint { get; set; }
        public IList<Call> Behavior { get; private set; }
    }
}