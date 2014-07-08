using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    /// <summary>
    ///     Provides functionality to transform intermediate models into <see cref="ScenarioModel" />s.
    /// </summary>
    public interface IScenarioService
    {
        /// <summary>
        ///     Transforms the given <see cref="XmiSequenceDiagramModel" /> into a corresponding
        ///     <see cref="ScenarioModel" />.
        /// </summary>
        /// <param name="xmiModel">The input model.</param>
        /// <returns>A corresponding scenario model.</returns>
        ScenarioModel From(XmiSequenceDiagramModel xmiModel);
    }
}