using System;
using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public interface IUmlRegionOwner
    {
        IList<UmlRegion> Regions { get; }

        UmlRegion CreateRegion();
    }

    public class UmlRegion
    {
        private readonly IDictionary<string, Vertex> _vertices;

        public UmlRegion(IUmlRegionOwner owner)
        {
            Owner = owner;
            _vertices = new Dictionary<string, Vertex>();
        }

        private IUmlRegionOwner Owner { get; set; }

        public ICollection<Vertex> Vertices
        {
            get { return _vertices.Values; }
        }

        public Vertex GetOrCreateStableState(string name)
        {
            if (_vertices.ContainsKey(name))
                return _vertices[name];

            return _vertices[name] = new StableState(name);
        }

        public Vertex GetOrCreateActivityState(string name, out bool created)
        {
            created = false;
            if (_vertices.ContainsKey(name))
                return _vertices[name];

            created = true;
            return _vertices[name] = new ActivityState(name);
        }

        public void AddVertex(UmlPseudoState initial)
        {
            throw new NotImplementedException();
        }
    }
}