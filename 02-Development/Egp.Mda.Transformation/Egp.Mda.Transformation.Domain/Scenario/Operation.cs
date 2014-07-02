using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Scenario
{
    public class Operation
    {
        public string Name { get; set; }
        public IParticipant Receiver { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}