﻿namespace Egp.Mda.Transformation.Domain
{
    public class MessageTriple
    {
        public IParticipant Target { get; set; }

        public string Operation { get; set; }

        public string Return { get; set; }

        public override string ToString()
        {
            return Operation;
        }
    }
}