using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public interface IBehaviorService
    {
        BehaviorModel From(ScenarioModel scenarioModel);
    }
}