using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.Xmi.SequenceDiagram
{
    public class Fragment
    {
        private IList<string> _covered;

        public string XmiId { get; set; }
        public string XmiType { get; set; }
        public string Message { get; set; }
        public string Body { get; set; }

        public IList<string> Covered
        {
            get { return _covered ?? (_covered = new List<string>()); }
        }

        public override string ToString()
        {
            return XmiId;
        }
    }
}