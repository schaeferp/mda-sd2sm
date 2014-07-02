using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.Behavior
{
    public class Behavior
    {
        public string PreState { get; set; }
        public string PostState { get; set; }

        public Message InMessage { get; set; }
        public IList<Message> OutMessages { get; set; }
    }
}