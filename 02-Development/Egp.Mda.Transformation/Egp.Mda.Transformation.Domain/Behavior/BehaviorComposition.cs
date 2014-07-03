using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class BehaviorComposition
    {
        public string Name { get; set; }
        public IList<Behavior> Behaviors { get; set; }
    }
}