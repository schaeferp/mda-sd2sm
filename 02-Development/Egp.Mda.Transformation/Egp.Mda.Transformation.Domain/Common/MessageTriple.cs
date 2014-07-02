namespace Egp.Mda.Transformation.Domain.Common
{
    public class MessageTriple
    {
        public IParticipant Target { get; set; }

        public string Operation { get; set; }

        public string Return { get; set; }
    }
}