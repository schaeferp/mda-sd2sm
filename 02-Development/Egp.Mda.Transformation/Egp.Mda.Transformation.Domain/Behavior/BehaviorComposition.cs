using System;
using System.Collections.Generic;
using System.Linq;

namespace Egp.Mda.Transformation.Domain
{
    public class BehaviorComposition
    {
        public string Name { get; set; }
        public IList<Behavior> Behaviors { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}", Name, Behaviors);
        }
    }
}