using Egp.Mda.Transformation.Domain.IOAutomaton;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core.StateMachine
{
    public interface IStateMachineService
    {
        Domain.Uml.StateMachine From(Automaton automaton);
    }
}