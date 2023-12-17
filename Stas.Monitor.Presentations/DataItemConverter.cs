using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class DataItemConverter : IDataItemConverter
{
    public IEnumerable<DataItem> ConvertMeasurementsToDataItems(IEnumerable<Measurement> measurements)
    {
        return measurements.Select(m => new DataItem
        {
            Timestamp = m.Timestamp,
            ActualValue = m.Temperature,
            MeasurementType = m.MeasurementType,
            Difference = m.Difference,
            FormattedDifference = m.FormattedDifference,
        });
    }

    public IEnumerable<DataItem> ConvertHumiditiesToDataItems(IEnumerable<Humidity> humidities)
    {
        return humidities.Select(h => new DataItem
        {
            Timestamp = h.Timestamp,
            ActualValue = h.HumidityValue,
            MeasurementType = h.MeasurementType,
            Difference = h.Difference,
            FormattedDifference = h.FormattedDifference,
        });
    }
}
