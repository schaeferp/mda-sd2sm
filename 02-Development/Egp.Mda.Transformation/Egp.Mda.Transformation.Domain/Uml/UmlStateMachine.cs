using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class UmlStateMachine : IUmlRegionOwner
    {
        public UmlStateMachine()
        {
            Regions = new List<UmlRegion>();
        }

        public IList<UmlRegion> Regions { get; private set; }

        public UmlRegion CreateRegion()
        {
            var region = new UmlRegion(this);
            Regions.Add(region);
            return region;
        }
    }
}