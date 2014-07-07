using System.IO;

namespace Egp.Mda.Transformation.Core
{
    public class FileWriter : IWriter
    {
        public void Write(string text)
        {
            var strTempFile = Path.GetTempFileName();
            File.WriteAllText(@strTempFile, text);
        }

        public void Write(string uri, string text)
        {
            File.WriteAllText(@uri, text);
        }
    }
}