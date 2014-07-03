using System.Collections.Generic;
using System.Linq;

namespace Egp.Mda.Transformation.Domain
{
    public class UmlStateMachineModel
    {
        private List<UmlStateMachine> _machines;

        public UmlStateMachineModel(IEnumerable<UmlStateMachine> machines)
        {
            _machines = machines.ToList();
        }

        public List<UmlStateMachine> Machines
        {
            get { return _machines ?? (_machines = new List<UmlStateMachine>()); }
        }
    }
}