using System.Collections.Generic;
using Egp.Mda.Transformation.Domain.Common;

namespace Egp.Mda.Transformation.Domain.Behavior
{
    public class Behavior
    {
        public string PreState { get; set; }
        public string PostState { get; set; }

        public MessageTriple InMessageTriple { get; set; }
        public IList<MessageTriple> OutMessages { get; set; }
    }
}