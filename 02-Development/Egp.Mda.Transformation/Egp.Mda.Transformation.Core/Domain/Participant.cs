namespace Egp.Mda.Transformation.Core
{
    public interface IParticipant
    {
        string Name { get; set; }
    }

    public class Actor : IParticipant
    {
        public string Name { get; set; }
    }

    public class SystemObject : IParticipant
    {
        public string Name { get; set; }
    }
}
