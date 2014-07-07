using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public interface IBehaviorService
    {
        /// <summary>
        ///     Responsible for transforming the given <paramref name="scenarioModel" /> into the appropriate
        ///     <see cref="BehaviorModel" />.
        ///     The ScenarioModel represents the various scenarios which are represented originally via sequence diagrams.
        ///     <seealso cref="BehaviorModel" />
        ///     <seealso cref="ScenarioModel" />
        /// </summary>
        /// <param name="scenarioModel">scenarioModel which should be transformed into a behavior model</param>
        /// <returns>behaviorModel which results from the given <paramref name="scenarioModel"/></returns>
        BehaviorModel From(ScenarioModel scenarioModel);
    }
}