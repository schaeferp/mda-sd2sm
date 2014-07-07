using System.Collections.Generic;
using System.Linq;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     One scenario which may for instance represent a sequence diagram or another dynamic behavior.
    /// </summary>
    public class Scenario
    {
        private IList<ScenarioOperationInvocation> _invocations;

        /// <summary>
        ///     The name of the scenario.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     A ordered collection of invocations. The order specifies the order of operation invocations.
        /// </summary>
        public IList<ScenarioOperationInvocation> Invocations
        {
            get { return _invocations ?? (_invocations = new List<ScenarioOperationInvocation>()); }
        }

        /// <summary>
        ///     Distinct collection of all <see cref="IParticipant" />s which receives an operation invocation in this scenario.
        ///     Distinction is based on object reference.
        /// </summary>
        public IEnumerable<IParticipant> ReceiverParticipants
        {
            get { return Invocations.Select(i => i.ScenarioOperation.Receiver).Distinct(); }
        }

        /// <summary>
        ///     Distinct collection of all <see cref="IParticipant" />s which sends an operation invocation in this scenario.
        ///     Distinction is based on object reference.
        /// </summary>
        public IEnumerable<IParticipant> SenderParticipants
        {
            get { return Invocations.Select(i => i.Sender).Distinct(); }
        }

        /// <summary>
        ///     Distinct collection of all <see cref="IParticipant" />s which receives or sends an operation invocation in this
        ///     scenario. Distinction is based on object reference.
        /// </summary>
        public IEnumerable<IParticipant> Participants
        {
            get { return ReceiverParticipants.Union(SenderParticipants).Distinct(); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}