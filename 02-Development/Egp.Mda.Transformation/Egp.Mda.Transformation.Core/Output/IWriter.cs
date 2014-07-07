namespace Egp.Mda.Transformation.Core.Output
{
    public interface IWriter
    {
        void Write(string text);
        void Write(string uri, string text);
    }
}