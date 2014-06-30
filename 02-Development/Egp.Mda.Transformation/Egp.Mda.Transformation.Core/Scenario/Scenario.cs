using System.Collections.Generic;

namespace Egp.Mda.Transformation.Core
{
    public class Scenario
    {
        public string Name { get; set; }

        public IEnumerable<OperationInvocation> Invocations { get; set; }
    }
}
