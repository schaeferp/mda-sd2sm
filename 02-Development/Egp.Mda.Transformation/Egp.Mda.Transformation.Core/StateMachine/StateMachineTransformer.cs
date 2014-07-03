using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egp.Mda.Transformation.Domain.IOAutomaton;

namespace Egp.Mda.Transformation.Core.StateMachine
{
    class StateMachineTransformer
    {
        private readonly Automaton _automaton;

        public StateMachineTransformer(Automaton automaton)
        {
            _automaton = automaton;
        }

        public StateMachine Do();
    }
}
