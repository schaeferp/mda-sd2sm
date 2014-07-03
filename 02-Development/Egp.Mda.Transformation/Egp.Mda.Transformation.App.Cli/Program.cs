using System.IO;
using Egp.Mda.Transformation.Core;

namespace Egp.Mda.Transformation.App.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = args[0];
            var stream = File.OpenRead(file);
            var xmiDeserializer = new MagicDrawXmiDeserializer();
            var xmiModel = xmiDeserializer.From(stream);
            var scenarioService = new ScenarioService();
            var scenarioModel = scenarioService.From(xmiModel);

            var behaviorService = new BehaviorService();
            var behaviorModel = behaviorService.From(scenarioModel);
        }
    }
}