using Egp.Mda.Transformation.Core.Domain;
using System.Collections.Generic;
using System.IO;

namespace Egp.Mda.Transformation.Core
{
    public interface IScenarioService
    {
        IEnumerable<Scenario> From(Stream xmiStream);
    }
}
