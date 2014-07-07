using System;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    /// A stateinvariant which is held within a scenario, expressing an invariant state of a receiver.
    /// </summary>
    public class ScenarioStateInvariant
    {
        /// <summary>
        /// The name of the state invariant.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates a new state invariant with an anonymous name (new GUID). Used if no useful name is associated to the current state of a operation receiver.
        /// </summary>
        /// <returns>a new state invariant having a non meaningful name</returns>
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