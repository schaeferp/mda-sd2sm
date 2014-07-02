using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.Uml
{
    public class StateMachine : IRegionOwner
    {
        public StateMachine()
        {
            Regions = new List<Region>();
        }

        public IList<Region> Regions { get; private set; }

        public Region CreateRegion()
        {
            var region = new Region(this);
            Regions.Add(region);
            return region;
        }
    }
}