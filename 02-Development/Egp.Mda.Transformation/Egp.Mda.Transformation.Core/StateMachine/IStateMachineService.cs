using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public interface IStateMachineService
    {
        UmlStateMachine From(Domain.IOAutomaton ioAutomaton);
    }
}