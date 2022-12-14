namespace Toolkit.Framework.Foundation;

public interface IWritableConfigurationProvider
{
    void Write<TValue>(string section, TValue value) where TValue : class, new();
}