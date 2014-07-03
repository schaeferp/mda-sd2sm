using Egp.Mda.Transformation.Domain;

namespace Egp.Mda.Transformation.Core
{
    public interface IStateMachineService
    {
        UmlStateMachineModel From(IOAutomatonModel model);
    }
}