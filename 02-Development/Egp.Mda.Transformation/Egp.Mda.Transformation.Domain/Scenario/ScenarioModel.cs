using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class ScenarioModel
    {
        private List<Scenario> _scenarios;

        public List<Scenario> Scenarios
        {
            get { return _scenarios ?? (_scenarios = new List<Scenario>()); }
        }
    }
}