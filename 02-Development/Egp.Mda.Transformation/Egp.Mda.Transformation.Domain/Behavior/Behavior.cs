using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class Behavior
    {
        public string PreState { get; set; }
        public string PostState { get; set; }

        public MessageTriple InMessageTriple { get; set; }
        public IList<MessageTriple> OutMessages { get; set; }
    }
}