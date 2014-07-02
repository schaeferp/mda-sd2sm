using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.IOAutomaton
{
    public class Message
    {
        public Message()
        {
        }

        public Message(Behavior.Message other)
        {
            Target = other.Target;
            Operation = other.Operation;
            Return = other.Return;
        }

        public IParticipant Target { get; set; }

        public string Operation { get; set; }

        public string Return { get; set; }
    }
}