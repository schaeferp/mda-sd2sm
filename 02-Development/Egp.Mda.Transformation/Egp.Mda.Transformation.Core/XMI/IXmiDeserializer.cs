﻿using System.IO;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public interface IXmiDeserializer
    {
        XmiSequenceDiagramModel From(Stream xmi);
    }
}