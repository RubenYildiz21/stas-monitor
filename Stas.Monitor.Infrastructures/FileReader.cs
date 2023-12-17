namespace Stas.Monitor.Infrastructures;

public class FileReader : IFileReader
{
    private readonly IDictionary<string, Dictionary<string, string>> _sections = new Dictionary<string, Dictionary<string, string>>();
    private int _thermometerCount;

    public void ParseFile(TextReader reader)
    {
        string currentSection = string.Empty;
        string line;

        while ((line = reader.ReadLine()!) != null)
        {
            line = line.Trim();
            if (IsCommentOrEmpty(line))
            {
                continue;
            }

            currentSection = UpdateSectionIfNecessary(line, currentSection);
            if (IsNewSection(line))
            {
                _sections[currentSection] = new Dictionary<string, string>();
            }
            else
            {
                AddKeyValuePair(currentSection, line);
            }
        }
    }

    public IDictionary<string, Dictionary<string, string>> GetSections()
    {
        return _sections;
    }

    public int GetThermometerCount()
    {
        return _thermometerCount;
    }

    private string UpdateSectionIfNecessary(string line, string currentSection)
    {
        if (IsNewSection(line))
        {
            var sectionName = GetSectionName(line);
            return FormatSectionName(sectionName);
        }

        return currentSection;
    }

    private bool IsCommentOrEmpty(string line)
    {
        return string.IsNullOrEmpty(line) || line.StartsWith(";");
    }

    private bool IsNewSection(string line)
    {
        return line.StartsWith("[") && line.EndsWith("]");
    }

    private string GetSectionName(string line)
    {
        return line.Substring(1, line.Length - 2);
    }

    private string FormatSectionName(string sectionName)
    {
        if (sectionName == "general")
        {
            _thermometerCount++;
            return $"{_thermometerCount}_{sectionName}";
        }

        return $"{_thermometerCount}_{sectionName}";
    }

    private void AddKeyValuePair(string currentSection, string line)
    {
        var keyValue = line.Split(new[] { '=' }, 2);
        if (keyValue.Length == 2)
        {
            var key = keyValue[0].Trim();
            var value = keyValue[1].Trim();
            if (_sections.TryGetValue(currentSection, out var section))
            {
                section[key] = value;
            }
        }
    }

    public IDictionary<string, int> GetProfile(string profileSection)
        => _sections[profileSection]
            .Where(kv => kv.Key.StartsWith("jal"))
            .ToDictionary(kv => kv.Key, kv => int.Parse(kv.Value));
}
