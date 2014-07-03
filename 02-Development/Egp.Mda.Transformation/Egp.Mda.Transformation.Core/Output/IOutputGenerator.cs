﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core.Output
{
    interface IOutputGenerator
    {
        IList<string> GenerateTextDiagrams(UmlStateMachineModel stateMachines);

    }
}