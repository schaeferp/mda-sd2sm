using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public interface IScenarioService
    {
        ScenarioModel From(XmiSequenceDiagramModel xmiModel);
    }
}