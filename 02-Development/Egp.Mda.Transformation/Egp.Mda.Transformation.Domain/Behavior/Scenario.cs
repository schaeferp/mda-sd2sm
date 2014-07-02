using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.Behavior
{
    public class Scenario
    {
        public string Name { get; set; }
        public IList<Behavior> Behaviors { get; set; }
    }
}