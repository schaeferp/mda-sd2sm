using System;

namespace Egp.Mda.Transformation.Domain.Scenario
{
    public class StateInvariant
    {
        public string Name { get; set; }

        public static StateInvariant CreateAnonymous()
        {
            return new StateInvariant {Name = Guid.NewGuid().ToString()};
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
