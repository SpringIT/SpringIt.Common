namespace SpringIt.Config
{
    public interface IConfigReader
    {
        bool HasValue(string key);
        T GetValue<T>(string key);
    }
}