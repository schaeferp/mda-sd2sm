using System;
using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain
{
    /// <summary>
    ///     Represents a single behavior, consisting of an in- and multiple out-messages as well as an pre- and poststate.
    ///     Multiple behaviors are used for one participant in one scenario for representing the resulting actions, based on an
    ///     incoming message.
    /// </summary>
    public class Behavior
    {
        private IList<MessageTriple> _outMessages;
        /// <summary>
        /// Prestate of the behavior, which is ensured to be held before receiving the inmessage.
        /// </summary>
        public string PreState { get; set; }
        /// <summary>
        /// Poststate of the behavior, which is ensured to be held after dealing with all outmessages.
        /// </summary>
        public string PostState { get; set; }
        /// <summary>
        /// Message coming in, triggering this behavior in the prestate.
        /// </summary>
        public MessageTriple InMessageTriple { get; set; }
        /// <summary>
        /// Messages going out after the trigger of the inmessage. After each outmessage is processed the poststate is held.
        /// </summary>
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