using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class UmlState : Vertex
    {
    }

    public class StableState : UmlState
    {
        public StableState(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class ActivityState : UmlState, IUmlRegionOwner
    {
        public ActivityState(string name)
        {
            Name = name;
            Regions = new List<UmlRegion>();
        }

        public string Name { get; set; }
        public IList<UmlRegion> Regions { get; private set; }

        public UmlRegion CreateRegion()
        {
            var region = new UmlRegion(this);
            Regions.Add(region);
            return region;
        }
    }

    public class ActionState : UmlState
    {
        public ActionState()
        {
            Behavior = new List<MessageTriple>();
        }

        public string Constraint { get; set; }
        public IList<MessageTriple> Behavior { get; private set; }
    }
}