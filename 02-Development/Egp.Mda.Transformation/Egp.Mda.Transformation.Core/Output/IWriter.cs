namespace Egp.Mda.Transformation.Core.Output
{
    internal interface IWriter
    {
        void write(string text);
        void write(string uri, string text);
    }
}