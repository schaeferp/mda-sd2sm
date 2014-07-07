namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    /// An operation which can be invoked on the <see cref="Receiver"/>
    /// </summary>
    public class ScenarioOperation
    {
        private IParticipant _receiver;

        /// <summary>
        /// The name of the operation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The <see cref="IParticipant"/> which provides this operation for invocation (see <see cref="ScenarioOperationInvocation"/>).
        /// </summary>
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