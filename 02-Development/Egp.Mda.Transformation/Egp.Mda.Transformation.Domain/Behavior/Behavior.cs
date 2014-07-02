using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egp.Mda.Transformation.Domain.Behavior
{
    public class Behavior
    {
        public string PreState { get; set; }
        public string PostState { get; set; }

        public Message InMessage { get; set; }
        public IEnumerable<Message> OutMessages { get; set; } 
    }
}
