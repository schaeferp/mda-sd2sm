using System.Collections.Generic;

namespace Egp.Mda.Transformation.Core
{
    public class Scenario
    {
        private IEnumerable<OperationInvocation> _invocations;

        public string Name { get; set; }

        public IEnumerable<OperationInvocation> Invocations
        {
            get { return _invocations ?? (_invocations = new List<OperationInvocation>()); }
        }
    }
}
