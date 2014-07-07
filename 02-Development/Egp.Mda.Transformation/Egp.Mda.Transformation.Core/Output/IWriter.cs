namespace Egp.Mda.Transformation.Core.Output
{
    internal interface IWriter
    {
        void Write(string text);
        void Write(string uri, string text);
    }
}