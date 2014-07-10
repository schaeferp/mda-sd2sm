using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public interface IParticipant
    {
        string Name { get; set; }
    }

    public class Actor : IParticipant
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class SystemObject : IParticipant
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}