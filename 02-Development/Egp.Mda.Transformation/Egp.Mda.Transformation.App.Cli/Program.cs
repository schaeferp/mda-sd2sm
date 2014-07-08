using System;
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
            var xmiModel = xmiDeserializer.SequenceDiagramFrom(stream);
            var scenarioService = new ScenarioService();
            var scenarioModel = scenarioService.From(xmiModel);

            var behaviorService = new BehaviorService();
            var behaviorModel = behaviorService.From(scenarioModel);

            var ioAutomatonService = new AutomatonService();
            var ioAutomaton = ioAutomatonService.From(behaviorModel);

            var stateMachineService = new StateMachineService();
            var stateMachineModel = stateMachineService.From(ioAutomaton);

            var plantUmlOutputGenerator = new PlantUmlOutputGenerator();
            var diagramList = plantUmlOutputGenerator.GenerateTextDiagrams(stateMachineModel);
            var writer = new FileWriter();
            var i = 1;
            foreach (var diagram in diagramList)
            {
                writer.Write(args[1] + i + ".txt", diagram);
                i++;
            }
        }
    }
}