using System.Collections.Generic;
using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Behavior
{
    public class Scenario
    {
        public IParticipant Participant { get; set; }
    
        public IEnumerable<Behavior> Behaviors { get; set; }
    }
}