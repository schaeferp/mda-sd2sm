using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     A scenariomodel which contains a collection of scenarios. Each scenario may describe for example a sequence
    ///     diagram.
    /// </summary>
    public class ScenarioModel
    {
        private List<Scenario> _scenarios;

        /// <summary>
        /// Collection of <see cref="Scenario"/>s, representing for instance a sequence diagram each.
        /// </summary>
        public List<Scenario> Scenarios
        {
            get { return _scenarios ?? (_scenarios = new List<Scenario>()); }
        }
    }
}