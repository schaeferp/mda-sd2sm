using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Behavior
{
    public class Message
    {
        public IParticipant Target { get; set; }

        public string Operation { get; set; }

        public string Return { get; set; }
    }
}