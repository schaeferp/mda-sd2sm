using System.Collections.Generic;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core.IOAutomaton
{
    public interface IAutomatonService
    {
        IEnumerable<Domain.IOAutomaton> From(IEnumerable<ParticipantBehaviorComposition> contexts);
    }
}