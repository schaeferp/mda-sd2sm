namespace Egp.Mda.Transformation.Domain
{
    public class ScenarioOperation
    {
        private IParticipant _receiver;
        public string Name { get; set; }

        public IParticipant Receiver
        {
            get { return _receiver; }
            set
            {
                if (null != _receiver)
                {
                    _receiver.Operations.Remove(this);
                }
                _receiver = value;
                if (null != value)
                {
                    _receiver.Operations.Add(this);
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}