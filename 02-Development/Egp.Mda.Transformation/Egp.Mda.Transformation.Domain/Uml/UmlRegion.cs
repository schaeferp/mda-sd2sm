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

        public UmlVertex EnsureState(string name, Action<UmlVertex> onCreate = null)
        {
            if (_vertices.ContainsKey(name))
                return _vertices[name];

            var vertex = new UmlState {Label = name};
            if (onCreate != null) onCreate(vertex);
            return _vertices[name] = vertex;
        }

        public UmlVertex EnsurePseudoState(string name, UmlPseudoStateKind kind, Action<UmlVertex> onCreate = null)
        {
            if (_vertices.ContainsKey(name))
                return _vertices[name];

            var vertex = new UmlPseudoState(kind) { Label = name };
            if (onCreate != null) onCreate(vertex);
            return _vertices[name] = vertex;
        }
    }
}