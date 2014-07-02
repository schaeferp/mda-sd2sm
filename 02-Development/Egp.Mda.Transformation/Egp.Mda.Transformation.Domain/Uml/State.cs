using System.Collections.Generic;
using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Uml
{
    public class State : Vertex
    {
    }

    public class StableState : State
    {
        public StableState(string name)
        {
            Name = name;
        }

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

        public Region CreateRegion()
        {
            var region = new Region(this);
            Regions.Add(region);
            return region;
        }
    }

    public class ActionState : State
    {
        public ActionState()
        {
            Behavior = new List<MessageTriple>();
        }

        public string Constraint { get; set; }
        public IList<MessageTriple> Behavior { get; private set; }
    }
}