namespace Stas.Monitor.Domains;

public record Humidity : DataItem
{
    public string? ThermometerName { get; init; }

    public new DateTime Timestamp { get; init; }

    public double HumidityValue { get; init; }

    public new double Difference { get; init; }

    public new string FormattedDifference
    {
        get
        {
            return Difference != 0 ? $"{Difference:F2}%" : string.Empty;
        }
    }
}
