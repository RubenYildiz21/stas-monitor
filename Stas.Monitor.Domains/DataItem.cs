namespace Stas.Monitor.Domains;

public class DataItem
{
    public DateTime Timestamp { get; set; }
    public string DataType { get; set; }
    public double? ExpectedTemperature { get; set; }
    public double? ActualTemperature { get; set; }
    public string MeasurementType { get; set; }
    
    public string BackgroundColor => DataType == "Alert" ? "#FF747B" : "White";

}