using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public interface IDataItemConverter
{
    IEnumerable<DataItem> ConvertMeasurementsToDataItems(IEnumerable<Measurement> measurements);

    IEnumerable<DataItem> ConvertHumiditiesToDataItems(IEnumerable<Humidity> humidities);
}
