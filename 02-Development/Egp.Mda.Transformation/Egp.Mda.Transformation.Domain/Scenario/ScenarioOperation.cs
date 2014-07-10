namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     An operation which can be invoked on the <see cref="Receiver" />
    /// </summary>
    public class ScenarioOperation
    {
        /// <summary>
        ///     The name of the operation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The <see cref="IParticipant" /> which provides this operation for invocation (see
        ///     <see cref="ScenarioOperationInvocation" />).
        /// </summary>
        public IParticipant Receiver { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}