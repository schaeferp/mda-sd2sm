using System.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public class StateMachineService : IStateMachineService
    {
        public UmlStateMachineModel From(IOAutomatonModel model)
        {
            var stateMachines = model.Automata.Select(a => new StateMachineTransformer(a).Result);
            return new UmlStateMachineModel(stateMachines);
        }
    }
}