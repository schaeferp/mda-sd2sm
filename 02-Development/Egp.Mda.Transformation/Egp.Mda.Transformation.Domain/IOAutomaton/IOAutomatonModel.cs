using System.Collections.Generic;
using System.Linq;

namespace Egp.Mda.Transformation.Domain
{
    public class IOAutomatonModel
    {
        private List<IOAutomaton> _automata;

        public IOAutomatonModel(IEnumerable<IOAutomaton> automata)
        {
            _automata = automata.ToList();
        }

        public List<IOAutomaton> Automata
        {
            get { return _automata ?? (_automata = new List<IOAutomaton>()); }
        }
    }
}