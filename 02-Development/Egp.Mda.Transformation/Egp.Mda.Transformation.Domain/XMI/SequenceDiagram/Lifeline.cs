namespace Egp.Mda.Transformation.Domain
{
    public class Lifeline
    {
        /// <summary>
        ///     Represents a Lifeline (XMI) node.
        /// </summary>
        public string XmiId { get; set; }

        /// <summary>
        ///     This Lifeline's XMI-type.
        /// </summary>
        public string XmiType { get; set; }

        /// <summary>
        ///     This Lifeline's Represents id.
        /// </summary>
        public string Represents { get; set; }

        public override string ToString()
        {
            return XmiId;
        }
    }
}