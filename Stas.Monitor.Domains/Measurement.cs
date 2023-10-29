namespace Stas.Monitor.Domains;

public class Measurement : DataItem
{
    public string ThermometerName { get; set; }
    public DateTime Timestamp { get; set; }
    
    public string DataType
    {
        get => "Measurement";
        set => throw new NotImplementedException("set DataType dans Measurement non implémenté");
    }
    public string MeasurementType { get; set; }
    public double Value { get; set; }
}