using System.Text;

namespace Egp.Mda.Transformation.Domain
{
    public class UmlTransition
    {
        public UmlVertex Target { get; set; }
        public string Guard { get; set; }
        public string Action { get; set; }
        public string Return { get; set; }

        public string Label
        {
            get
            {
                var sb = new StringBuilder();

                if (Guard != null)
                    sb.Append('[').Append(Guard).Append("] ");
                if (Action != null)
                    sb.Append(Action).Append(' ');
                if (Return != null)
                    sb.Append("/ ").Append(Return);

                return sb.ToString();
            }
        }
    }
}