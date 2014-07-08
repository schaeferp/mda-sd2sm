using System;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     Represents an invocation of an operation on a <see cref="Receiver" />. This operation is invoked by the
    ///     <see cref="Sender" />.
    ///     Before the operation invocation the <see cref="PreScenarioStateInvariant" /> is held at the <see cref="Receiver" />
    ///     side. After invocation and processing the <see cref="PostScenarioStateInvariant" /> is held.
    /// </summary>
    public class ScenarioOperationInvocation
    {
        /// <summary>
        ///     Default value, used in case no return value is specified.
        /// </summary>
        public const string DefaultReturnValue = "void";

        public ScenarioOperationInvocation()
        {
            Return = DefaultReturnValue;
        }

        /// <summary>
        ///     StateInvariant which is held by the <see cref="Receiver" />, before the invocation of the operation.
        /// </summary>
        public ScenarioStateInvariant PreScenarioStateInvariant { get; set; }

        /// <summary>
        ///     StateInvariant which is held by the <see cref="Receiver" />, after the invocation of the operation.
        /// </summary>
        public ScenarioStateInvariant PostScenarioStateInvariant { get; set; }

        /// <summary>
        ///     The <see cref="IParticipant" /> which invokes the operation (opeartion invoker) on the <see cref="Receiver" />.
        /// </summary>
        public IParticipant Sender { get; set; }

        /// <summary>
        ///     Returns the <see cref="ScenarioOperation.Receiver" /> for convenience, which is the <see cref="IParticipant" />
        ///     providing the operation for invocation.
        /// </summary>
        public IParticipant Receiver
        {
            get { return ScenarioOperation.Receiver; }
        }

        /// <summary>
        /// The operation which is invoked, see <see cref="ScenarioOperation"/>.
        /// </summary>
        public ScenarioOperation ScenarioOperation { get; set; }

        /// <summary>
        /// The value which is returned by this invocation of the <see cref="ScenarioOperation"/>.
        /// </summary>
        public string Return { get; set; }

        public override string ToString()
        {
            return String.Format("{0} -> {1} -> {2}", PreScenarioStateInvariant, ScenarioOperation,
                PostScenarioStateInvariant);
        }
    }
}