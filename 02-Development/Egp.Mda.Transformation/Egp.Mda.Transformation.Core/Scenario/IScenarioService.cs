using System.Collections.Generic;
using Egp.Mda.Transformation.Domain.Scenario;
using Egp.Mda.Transformation.Domain.Xmi.SequenceDiagram;

namespace Egp.Mda.Transformation.Core
{
    public interface IScenarioService
    {
        IEnumerable<Scenario> From(XmiSequenceDiagramModel xmiModel);
    }
}