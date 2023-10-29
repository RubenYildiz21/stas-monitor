namespace Stas.Monitor.Domains;

public class ThermometerConfiguration
{
    public string Name { get; set; }
    public string TimestampFormat { get; set; }
    public string TempFormat { get; set; }
    public Dictionary<string, int> Profile { get; set; } = new Dictionary<string, int>();
}