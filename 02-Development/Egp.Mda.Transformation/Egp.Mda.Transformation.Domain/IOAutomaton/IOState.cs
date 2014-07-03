using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class IOState
    {
        public string Name { get; set; }
        public IList<IOTransition> Outgoing { get; set; }
    }
}