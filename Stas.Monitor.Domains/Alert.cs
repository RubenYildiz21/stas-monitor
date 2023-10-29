namespace Stas.Monitor.Domains;

public class Alert : DataItem
{
    public string ThermometerName { get; set; }
    public DateTime Timestamp { get; set; }
    public string DataType
    {
        get => "Alert";
        set => throw new NotImplementedException("set DataType dans Alert non implémenté");
    }
    public double ExpectedTemperature { get; set; }
    public double ActualTemperature { get; set; }
}