using System.Collections.Generic;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core.Output
{
    /// <summary>
    ///     Provides an interface for output generators
    /// </summary>
    public interface IOutputGenerator
    {
        /// <summary>
        ///     Renders diagrams for all state machines supplied by the model.
        /// </summary>
        /// <param name="stateMachines">A model containing one or more state machines.</param>
        /// <returns>A list of text based diagrams.</returns>
        IList<string> GenerateTextDiagrams(UmlStateMachineModel stateMachines);
    }
}