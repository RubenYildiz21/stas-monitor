namespace Stas.Monitor.Domains;

public record ThermometerConfiguration (string Name, string TimestampFormat, string TempFormat, IDictionary<string, int> Profile);
