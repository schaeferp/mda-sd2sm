namespace Egp.Mda.Transformation.Domain
{
    public class UmlStateMachine : IUmlRegionOwner
    {
        public UmlStateMachine()
        {
            Region = new UmlRegion();
        }

        public UmlRegion Region { get; set; }
    }
}