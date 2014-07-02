using System.Collections.Generic;
using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Uml
{
    public class Transition
    {
        public Vertex Target { get; set; }
        public string Action { get; set; }
        public string Return { get; set; }
    }
}