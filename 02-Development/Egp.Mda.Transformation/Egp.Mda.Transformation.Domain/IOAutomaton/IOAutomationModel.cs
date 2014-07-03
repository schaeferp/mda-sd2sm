using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class IOAutomationModel
    {
        private List<IOAutomaton> _automations;

        public List<IOAutomaton> Automations
        {
            get { return _automations ?? (_automations = new List<IOAutomaton>()); }
        }
    }
}