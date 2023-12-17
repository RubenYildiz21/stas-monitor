namespace Stas.Monitor.Domains;

public interface IConfigurationReader
{
    Configuration GetConfiguration();

    string GetValue(string section, string key);
}
