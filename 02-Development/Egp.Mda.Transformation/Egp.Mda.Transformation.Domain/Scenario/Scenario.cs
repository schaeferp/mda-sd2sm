﻿using System.Collections.Generic;

namespace Egp.Mda.Transformation.Domain.Scenario
{
    public class Scenario
    {
        public string Name { get; set; }

        public IList<OperationInvocation> Invocations { get; set; }
    }
}
