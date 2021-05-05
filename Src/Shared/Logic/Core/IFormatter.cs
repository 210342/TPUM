namespace TPUM.Shared.Logic.Core
{
    public interface IFormatter<T>
    {
        Format Format { get; }
        string FormatObject(T obj);
        T Deformat(string str);
    }
}
