namespace Stas.Monitor.Domains;

public record Measurement : DataItem
{
    public string? ThermometerName { get; init; }

    public new DateTime Timestamp { get; init; }

    public double Temperature { get; init; }

    public new double Difference { get; init; }

    public new string FormattedDifference
    {
        get
        {
            return Difference != 0 ? $"{Difference:F2}°C" : string.Empty;
        }
    }

    public bool IsValidMeasurement()
    {
        return Temperature >= 0 && Temperature <= 100;
    }

    public string GetMeasurementAlert()
    {
        if (Temperature < 30)
        {
            return "Low Temperature";
        }
        else if (Temperature > 70)
        {
            return "High Temperature";
        }
        else
        {
            return "Normal Temperature";
        }
    }
}
