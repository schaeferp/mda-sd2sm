using System.Collections.Generic;
using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Behavior
{
    public class Scenario
    {
        public IParticipant Participant { get; set; }
    
        public IList<Behavior> Behaviors { get; set; }
    }
}