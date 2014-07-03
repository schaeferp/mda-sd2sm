using System;
using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public interface IUmlRegionOwner
    {
        UmlRegion Region { get; set; }
    }

    public class UmlRegion
    {
        private readonly IDictionary<string, UmlVertex> _vertices;

        public UmlRegion(string name = "")
        {
            Name = name;
            _vertices = new Dictionary<string, UmlVertex>();
        }

        public string Name { get; set; }

        public IEnumerable<UmlVertex> Vertices
        {
            get { return _vertices.Values; }
        }

        public UmlVertex GetOrCreateStableState(string name)
        {
            if (_vertices.ContainsKey(name))
                return _vertices[name];

            return _vertices[name] = new UmlState {Label = name};
        }

        public UmlVertex GetOrCreateActivityState(string name, out bool created)
        {
            created = false;
            if (_vertices.ContainsKey(name))
                return _vertices[name];

            created = true;
            return _vertices[name] = new UmlState {Label = name};
        }

        public UmlVertex GetOrAddPseudoState(string name, UmlPseudoStateKind kind)
        {
            if (_vertices.ContainsKey(name))
                return _vertices[name];

            return _vertices[name] = new UmlPseudoState(kind) { Label = name };
        }
    }
}