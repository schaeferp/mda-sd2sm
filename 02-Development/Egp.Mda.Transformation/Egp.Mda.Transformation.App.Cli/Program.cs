using System.IO;
using Egp.Mda.Transformation.Core;

namespace Egp.Mda.Transformation.App.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = args[0];
            var stream = File.OpenRead(file);
            //var scnSvc = new MagicDrawXmiScenarioService();
            //var x = scnSvc.From(stream);
            var bla = new MagicDrawXmiDeserializer();
            var blub = bla.From(stream);
        }
    }
}
