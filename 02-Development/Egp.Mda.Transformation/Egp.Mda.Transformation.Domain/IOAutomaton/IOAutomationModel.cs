using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egp.Mda.Transformation.Domain
{
    public class IOAutomationModel
    {
        private List<IOAutomaton> _automations;

        public List<IOAutomaton> Automations
        {
            get
            {
                return _automations ?? (_automations = new List<IOAutomaton>());
            }
        }
    }
}
