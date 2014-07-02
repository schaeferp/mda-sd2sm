using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.Scenario
{
    public class Scenario
    {
        private IList<OperationInvocation> _invocations;

        public string Name { get; set; }

        public IList<OperationInvocation> Invocations
        {
            get { return _invocations ?? (_invocations = new List<OperationInvocation>()); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}