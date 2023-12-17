namespace Stas.Monitor.Infrastructures;

using Domains;
using Serilog;

public class IniConfigurationReader : IConfigurationReader
{
    private IFileReader _fileReader;

    public IniConfigurationReader(Stream stream, IFileReader fileReader)
    {
        _fileReader = fileReader;
        if (stream != null)
        {
            using var reader = new StreamReader(stream);
            _fileReader.ParseFile(reader);
        }
        else
        {
            Log.Error("File cannot be null");
        }
    }

    public string GetValue(string section, string key)
    {
        var sections = _fileReader.GetSections();
        if (sections.TryGetValue(section, out var sectionDictionary))
        {
            if (sectionDictionary.TryGetValue(key, out var value))
            {
                return value;
            }
        }

        Log.Error("monitor: missing required section thermometer");
        return null!;
    }

    public Configuration GetConfiguration()
    {
        int thermometerCount = _fileReader.GetThermometerCount();
        var thermometerConfigs = new List<ThermometerConfiguration>();

        for (int i = 1; i <= thermometerCount; i++)
        {
            var sections = GetSections(i);
            var profile = _fileReader.GetProfile(sections.ProfileSection);
            var thermometerConfig = new ThermometerConfiguration(
                Name: GetValue(sections.GeneralSection, "name"),
                TempFormat: GetValue(sections.FormatSection, "temperature"),
                TimestampFormat: GetValue(sections.FormatSection, "DateTime"),
                Profile: profile);

            thermometerConfigs.Add(thermometerConfig);
        }

        return new Configuration(thermometerConfigs);
    }

    private (string GeneralSection, string FormatSection, string ProfileSection) GetSections(int i)
        => ($"{i}_general", $"{i}_format", $"{i}_profile");
}
