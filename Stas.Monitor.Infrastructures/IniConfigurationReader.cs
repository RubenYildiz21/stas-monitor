namespace Stas.Monitor.Infrastructures;

using System;
using System.Collections.Generic;
using System.IO;
using Stas.Monitor.Domains;
public class IniConfigurationReader : IConfigurationReader
{
    
    private Dictionary<string, Dictionary<string, string>> _sections = new Dictionary<string, Dictionary<string, string>>();
    private int _thermometerCount = 0;
    public IniConfigurationReader(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }
        using var reader = new StreamReader(stream);
        ParseFile(reader);
    }
    
    private void ParseFile(StreamReader reader)
    {
        string currentSection = "";
        String line;

        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();
            if (IsCommentOrEmpty(line)) continue;

            currentSection = UpdateSectionIfNecessary(line, currentSection);
            if (IsNewSection(line)) 
                _sections[currentSection] = new Dictionary<string, string>();
            else 
                AddKeyValuePair(currentSection, line);
        }
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

    private string FormatSectionName(string sectionName)
    {
        if (sectionName == "general")
        {
            _thermometerCount++;
            return $"{_thermometerCount}_{sectionName}";
        }
        return $"{_thermometerCount}_{sectionName}";
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
    
    public string GetValue(string section, string key)
    {
        if (_sections.TryGetValue(section, out var sectionDictionary))
        {
            if (sectionDictionary.TryGetValue(key, out var value))
            {
                return value;
            }
        }
        throw new KeyNotFoundException("Invalid key");
    }

    public IEnumerable<Measurement> GetMeasurements()
    {
        throw new NotImplementedException();
    }

    public Configuration GetConfiguration()
    {
        var thermometerConfigs = Enumerable.Range(1, _thermometerCount)
            .Select(i => CreateThermometerConfig(i))
            .ToList();
        return new Configuration(GetCsvFilePath(), thermometerConfigs);
    }

    private ThermometerConfiguration CreateThermometerConfig(int i)
    {
        var sections = GetSections(i);
        var profile = GetProfile(sections.profileSection);
        return new ThermometerConfiguration
        {
            Name = GetValue(sections.generalSection, "name"),
            TempFormat = GetValue(sections.formatSection, "temperature"),
            TimestampFormat = GetValue(sections.formatSection, "DateTime"),
            Profile = profile
        };
    }

    private (string generalSection, string formatSection, string profileSection) GetSections(int i)
        => ($"{i}_general", $"{i}_format", $"{i}_profile");

    private Dictionary<string, int> GetProfile(string profileSection)
        => _sections[profileSection]
            .Where(kv => kv.Key.StartsWith("jal"))
            .ToDictionary(kv => kv.Key, kv => int.Parse(kv.Value));



    public string GetCsvFilePath()
    {
        return GetValue("0_csv", "filepath");
    }
    
    public string GetCsvAlertFilePath()
    {
        return GetValue("0_alerteCsv", "filepath");
    }
}