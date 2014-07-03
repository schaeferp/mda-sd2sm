using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    internal class StateMachineTransformer
    {
        private readonly Domain.IOAutomaton _automaton;

        public StateMachineTransformer(Domain.IOAutomaton automaton)
        {
            _automaton = automaton;
        }

        public UmlStateMachine Transform()
        {
            return null;
        }
    }
}