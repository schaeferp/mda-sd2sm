using System;
using System.Collections.Generic;
using System.Linq;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    /// Containing the <see cref="Behavior"/>s for a given scenario-name.
    /// </summary>
    public class BehaviorComposition
    {
        /// <summary>
        /// The name of the scenario.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The collection of behaviors, associated to the <see cref="Name"/> of the scenario.
        /// </summary>
        public IList<Behavior> Behaviors { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}", Name, String.Concat(Behaviors));
        }
    }
}