using System;
using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    public class Behavior
    {
        private IList<MessageTriple> _outMessages;
        public string PreState { get; set; }
        public string PostState { get; set; }

        public MessageTriple InMessageTriple { get; set; }

        public IList<MessageTriple> OutMessages
        {
            get { return _outMessages ?? (_outMessages = new List<MessageTriple>()); }
            set { _outMessages = value; }
        }

        public override string ToString()
        {
            return String.Format("{0}|{1}|{2}|{3}", PreState, InMessageTriple, String.Concat(OutMessages), PostState);
        }
    }
}