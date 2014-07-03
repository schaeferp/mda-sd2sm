using System.Collections.Generic;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core.Output
{
    /**
     * Provides an interface for output generators
     */

    internal interface IOutputGenerator
    {
        IList<string> GenerateTextDiagrams(UmlStateMachineModel stateMachines);
    }
}