using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.StateMachine
{
    public class StateMachine : IRegionOwner
    {
        public StateMachine()
        {
            Regions = new List<Region>();
        }

        public IList<Region> Regions { get; private set; }
    }
}