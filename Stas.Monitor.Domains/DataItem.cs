namespace Stas.Monitor.Domains;

 public record DataItem
 {
     public DateTime Timestamp { get; init; }

     public double? Difference { get; init; }

     public double? ActualValue { get; init; }

     public string? MeasurementType { get; init; }

     public string? FormattedDifference { get; init; }

     public string GetFormattedData()
     {
         return $"Timestamp: {Timestamp}, Value: {ActualValue}, Difference: {FormattedDifference}";
     }

     public bool IsValid()
     {
         if (Difference > ActualValue * 1.1)
         {
             return true;
         }

         return false;
     }
 }
