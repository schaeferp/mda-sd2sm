using Egp.Mda.Transformation.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Egp.Mda.Transformation.Core
{
    public abstract class XmiScenarioServiceBase : IScenarioService
    {

        protected abstract IEnumerable<Domain.Scenario> From(XDocument document);

        public IEnumerable<Scenario> From(Stream xmiStream)
        {
            var document = XDocument.Load(xmiStream);
            return From(document);
        }


        /// <summary>
        ///     Resolves the given XMLNS prefix to the full qualified URL and
        ///     returns an <see cref="XName" /> including the required attribute.
        /// </summary>
        /// <param name="prefix">XMLNS prefix.</param>
        /// <param name="attribute">XML attribute name.</param>
        /// <param name="document">XMI document.</param>
        /// <returns>XName including resolved XMLNS.</returns>
        protected XName LookupXName(string prefix, string attribute, XDocument document)
        {
            if (document.Root == null) throw new ArgumentNullException();
            var namespaceOfPrefix = document.Root.GetNamespaceOfPrefix(prefix);
            if (namespaceOfPrefix == null) throw new ArgumentNullException();
            var ns = namespaceOfPrefix.ToString();
            return XName.Get(attribute, ns);
        }
    }
}
