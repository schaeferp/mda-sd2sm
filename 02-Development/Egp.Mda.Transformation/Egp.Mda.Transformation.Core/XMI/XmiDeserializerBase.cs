﻿using System;
using System.IO;
using System.Xml.Linq;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public abstract class XmiDeserializerBase : IXmiDeserializer
    {
        public XmiSequenceDiagramModel SequenceDiagramFrom(Stream xmiStream)
        {
            var document = XDocument.Load(xmiStream);
            return From(document);
        }

        protected abstract XmiSequenceDiagramModel From(XDocument document);

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