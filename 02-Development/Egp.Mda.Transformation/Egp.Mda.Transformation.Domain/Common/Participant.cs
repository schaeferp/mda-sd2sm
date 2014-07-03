using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public interface IParticipant
    {
        string Name { get; set; }
        IList<ScenarioOperation> Operations { get; }
    }

    public class Actor : IParticipant
    {
        private IList<ScenarioOperation> _operations;
        public string Name { get; set; }

        public IList<ScenarioOperation> Operations
        {
            get { return _operations ?? (_operations = new List<ScenarioOperation>()); }
        }
    }

    public class SystemObject : IParticipant
    {
        private IList<ScenarioOperation> _operations;
        public string Name { get; set; }

        public IList<ScenarioOperation> Operations
        {
            get { return _operations ?? (_operations = new List<ScenarioOperation>()); }
        }
    }
}