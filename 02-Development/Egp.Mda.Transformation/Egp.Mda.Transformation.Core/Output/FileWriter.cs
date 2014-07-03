using System.IO;

namespace Egp.Mda.Transformation.Core.Output
{
    internal class FileWriter : IWriter
    {
        public void write(string text)
        {
            var strTempFile = Path.GetTempFileName();
            File.WriteAllText(@strTempFile, text);
        }

        public void write(string uri, string text)
        {
            File.WriteAllText(@uri, text);
        }
    }
}