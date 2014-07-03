namespace Egp.Mda.Transformation.Domain
{
    public class ScenarioOperation
    {
        public string Name { get; set; }
        public IParticipant Receiver { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}