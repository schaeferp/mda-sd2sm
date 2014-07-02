using System;

namespace Egp.Mda.Transformation.Domain.Xmi.SequenceDiagram
{
    public class Message
    {
        public string XmiId { get; set; }
        public string XmiType { get; set; }
        public string Name { get; set; }
        public string ReceiveEvent { get; set; }
        public string SendEvent { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", XmiId, Name);
        }
    }
}
