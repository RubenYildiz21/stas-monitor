﻿namespace Stas.Monitor.Domains;

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

    public bool IsValidHumidity()
    {
        return HumidityValue >= 0 && HumidityValue <= 100;
    }

    public string GetHumidityAlert()
    {
        if (HumidityValue < 30)
        {
            return "Low Humidity";
        }
        else if (HumidityValue > 70)
        {
            return "High Humidity";
        }
        else
        {
            return "Normal Humidity";
        }
    }
}
