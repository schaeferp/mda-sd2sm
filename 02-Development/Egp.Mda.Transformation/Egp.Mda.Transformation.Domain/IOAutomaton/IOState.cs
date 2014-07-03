using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class IOState
    {
        private IList<IOTransition> _outgoing;
        public string Name { get; set; }

        public IList<IOTransition> Outgoing
        {
            get { return _outgoing ?? (_outgoing = new List<IOTransition>()); }
        }
    }
}