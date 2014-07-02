using System.Collections.Generic;
using Egp.Mda.Transformation.Domain.Behavior;
using Egp.Mda.Transformation.Domain.IOAutomaton;

namespace Egp.Mda.Transformation.Core.IOAutomaton
{
    public interface IAutomatonService
    {
        IEnumerable<Automaton> From(IEnumerable<Context> contexts);
    }
}