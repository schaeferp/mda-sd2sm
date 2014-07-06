using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    /// <summary>
    ///     A service for transforming I/O-Automatons into UML State Machines.
    /// </summary>
    public interface IStateMachineService
    {
        /// <summary>
        ///     Transforms the given I/O-Automatons into UML State Machines.
        /// </summary>
        /// <param name="model">A model containing one or more I/O-Automatons.</param>
        /// <returns>A model containing one state machine for each I/O-Automaton.</returns>
        UmlStateMachineModel From(IOAutomatonModel model);
    }
}