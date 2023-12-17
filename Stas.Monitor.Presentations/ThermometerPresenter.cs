using System.Collections;
using System.Collections.ObjectModel;
using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class ThermometerPresenter : IThermometerPresenter
{
     private readonly IDataItemConverter _dataItemConverter;
     private readonly IUiThreadInvoker _uiThreadInvoker;

     public ICollection<DataItem> DataItemsTemperature { get; } = new ObservableCollection<DataItem>();

     public ICollection<DataItem> DataItemsHumidity { get; } = new ObservableCollection<DataItem>();

     public ThermometerPresenter(IDataItemConverter dataItemConverter, IUiThreadInvoker uiThreadInvoker)
    {
        _dataItemConverter = dataItemConverter;
        _uiThreadInvoker = uiThreadInvoker;
    }

     public async Task UpdateDataItemsTemperature(IEnumerable<Measurement> measurements)
     {
         var combinedList = _dataItemConverter.ConvertMeasurementsToDataItems(measurements);
         await _uiThreadInvoker.InvokeOnUIThreadAsync(() => UpdateObservableCollection(DataItemsTemperature, combinedList));
    }

     public async Task UpdateDataItemsHumidity(IEnumerable<Humidity> humidities)
     {
         var combinedList = _dataItemConverter.ConvertHumiditiesToDataItems(humidities);
         await _uiThreadInvoker.InvokeOnUIThreadAsync(() => UpdateObservableCollection(DataItemsHumidity, combinedList));
     }

     public void ClearTemperatureData()
     {
         DataItemsTemperature.Clear();
     }

     public void ClearHumidityData()
     {
         DataItemsHumidity.Clear();
     }

     public void UpdateObservableCollection<T>(ICollection<T> collection, IEnumerable<T> items)
     {
         collection.Clear();
         foreach (var item in items)
         {
             collection.Add(item);
         }
     }
}
