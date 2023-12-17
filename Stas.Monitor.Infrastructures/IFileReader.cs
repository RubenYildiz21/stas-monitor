namespace Stas.Monitor.Infrastructures;

public interface IFileReader
{
    void ParseFile(TextReader reader);

    IDictionary<string, Dictionary<string, string>> GetSections();

    int GetThermometerCount();

    IDictionary<string, int> GetProfile(string profileSection);
}
