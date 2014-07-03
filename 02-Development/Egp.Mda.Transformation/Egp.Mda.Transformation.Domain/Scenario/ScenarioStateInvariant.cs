using System;

namespace Egp.Mda.Transformation.Domain
{
    public class ScenarioStateInvariant
    {
        public string Name { get; set; }

        public static ScenarioStateInvariant CreateAnonymous()
        {
            return new ScenarioStateInvariant {Name = Guid.NewGuid().ToString()};
        }

        public override string ToString()
        {
            return Name;
        }
    }
}