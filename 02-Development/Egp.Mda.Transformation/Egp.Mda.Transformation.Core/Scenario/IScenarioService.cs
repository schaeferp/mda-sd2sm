using System.Collections.Generic;
using System.IO;
using Egp.Mda.Transformation.Domain.Behavior;

namespace Egp.Mda.Transformation.Core
{
    public interface IScenarioService
    {
        IEnumerable<Scenario> From(Stream xmiStream);
    }
}
