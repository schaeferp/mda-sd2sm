using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class Scenario
    {
        private IList<ScenarioOperationInvocation> _invocations;

        public string Name { get; set; }

        public IList<ScenarioOperationInvocation> Invocations
        {
            get { return _invocations ?? (_invocations = new List<ScenarioOperationInvocation>()); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}