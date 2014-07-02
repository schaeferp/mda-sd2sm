using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.Uml
{
    public interface IRegionOwner
    {
        IList<Region> Regions { get; }

        Region CreateRegion();
    }

    public class Region
    {
        private readonly IDictionary<string, Vertex> vertices; 

        public Region(IRegionOwner owner)
        {
            Owner = owner;
            vertices = new Dictionary<string, Vertex>();
        }

        private IRegionOwner Owner { get; set; }

        public ICollection<Vertex> Vertices
        {
            get { return vertices.Values; }
        }

        public Vertex GetOrCreateStableState(string name)
        {
            if (vertices.ContainsKey(name))
                return vertices[name];

            return vertices[name] = new StableState(name);
        }

        public Vertex GetOrCreateActivityState(string name, out bool created)
        {
            created = false;
            if (vertices.ContainsKey(name))
                return vertices[name];

            created = true;
            return vertices[name] = new ActivityState(name);
        }

        public void AddVertex(PseudoState initial)
        {
            throw new System.NotImplementedException();
        }
    }
}