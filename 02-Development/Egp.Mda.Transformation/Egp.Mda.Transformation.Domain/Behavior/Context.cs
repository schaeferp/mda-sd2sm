using System.Collections.Generic;
using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Behavior
{
    public class Context
    {
        public IParticipant Participant { get; set; }

        public IList<Scenario> Scenarios { get; set; }
    }
}