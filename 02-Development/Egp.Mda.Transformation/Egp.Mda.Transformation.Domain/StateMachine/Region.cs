using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.StateMachine
{
    public interface IRegionOwner
    {
        IList<Region> Regions { get; }
    }

    public class Region
    {
        public Region(IRegionOwner owner)
        {
            Owner = owner;
            Vertices = new List<Vertex>();
        }

        private IRegionOwner Owner { get; set; }
        public IList<Vertex> Vertices { get; private set; }
    }
}