using System.Collections.ObjectModel;
using Avalonia.Threading;
using Serilog;
using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class ThermometerPresenter
{
     public ObservableCollection<Measurement> RecentMeasurements { get; }
    public ObservableCollection<Alert> RecentAlerts { get; }
    
    public ObservableCollection<DataItem> DataItems { get; } = new ObservableCollection<DataItem>();
    
    public ThermometerPresenter()
    {
        RecentMeasurements = new ObservableCollection<Measurement>();
        RecentAlerts = new ObservableCollection<Alert>();
    }
    
    public void SetRecentMeasurements(IEnumerable<Measurement> measurements)
    {
        RecentMeasurements.Clear();
        foreach (var measurement in measurements)
        {
            RecentMeasurements.Add(measurement);
        }

    }
    
    public void SetRecentAlert(IEnumerable<Alert> alerts)
    {
        RecentAlerts.Clear();
        foreach (var alert in alerts)
        {
            RecentAlerts.Add(alert);
        }

    }
    
    
    public async Task UpdateDataItems(IEnumerable<Measurement> measurements, IEnumerable<Alert> alerts)
    {
        var combinedList = measurements.Select(ConvertToDataItem)
            .Concat(alerts.Select(ConvertToDataItem))
            .OrderByDescending(item => item.Timestamp)
            .ToList();

        Action updateAction = () =>
        {
            DataItems.Clear();
            foreach (var item in combinedList) DataItems.Add(item);
            SetRecentMeasurements(measurements);
            SetRecentAlert(alerts);
        };

        if (IsInTestMode()) updateAction();
        else await Dispatcher.UIThread.InvokeAsync(updateAction);
    }


    
    public DataItem ConvertToDataItem(object obj) => obj switch
    {
        Measurement m => new DataItem
        {
            Timestamp = m.Timestamp,
            DataType = m.DataType,
            ActualTemperature = m.Value,
            MeasurementType = m.MeasurementType,
            ExpectedTemperature = m.Value
        },
        Alert a => new DataItem
        {
            Timestamp = a.Timestamp,
            DataType = a.DataType,
            ExpectedTemperature = a.ExpectedTemperature,
            ActualTemperature = a.ActualTemperature,
            MeasurementType = "Alerte",
        },
        _ => throw new InvalidOperationException("Unknown type")
    };

    private bool IsInTestMode()
    {
        return AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.ToLowerInvariant().StartsWith("nunit.framework"));
    }
}