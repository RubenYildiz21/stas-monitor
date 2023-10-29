using System.ComponentModel;
using System.Runtime.CompilerServices;
using Stas.Monitor.Domains;
using System.Timers;
using Avalonia.Controls;

namespace Stas.Monitor.Presentations;

public class MainPresenter : INotifyPropertyChanged
{
    private DataManager _dataManager;
    private ThermometerConfiguration _currentThermometer;
    private IEnumerable<Measurement> _recentMeasurements;
    private IEnumerable<Alert> _recentAlerts;

    public MainPresenter(Configuration configuration, MeasurementServices measurementService, AlertServices alertServices)
    {
        _currentThermometer = configuration.Thermometers.FirstOrDefault();
        _dataManager = new DataManager(configuration, measurementService, alertServices, _currentThermometer);
        var timerManager = new TimerManager();
        SelectedThermometer = new ThermometerPresenter();
        timerManager.StartTimer(OnUpdateTimerElapsed);
    }
    
    public Configuration Configuration
    {
        get => _dataManager.Configuration;
    }
    
    public ThermometerPresenter SelectedThermometer { get; set; }
    public ThermometerConfiguration CurrentThermometer
    {
        get => _currentThermometer;
        set
        {
            _currentThermometer = value;
            OnPropertyChanged(nameof(CurrentThermometer));
        }
    }
    
    public void UpdateRecentMeasurements()
    {
        _recentMeasurements = _dataManager.GetRecentMeasurements();
        OnPropertyChanged(nameof(RecentMeasurements));
    }
    
    public void UpdateRecentAlerts()
    {
        _recentAlerts = _dataManager.GetRecentAlerts(_recentMeasurements);
        OnPropertyChanged(nameof(RecentAlerts));
    }
    
    private void OnUpdateTimerElapsed(object sender, ElapsedEventArgs e)
    {
        UpdateRecentMeasurements();
        UpdateRecentAlerts();
        SelectedThermometer.UpdateDataItems(_recentMeasurements, _recentAlerts);
    }
    
    public IEnumerable<Measurement> RecentMeasurements
    {
        get => _recentMeasurements;
        set
        {
            _recentMeasurements = value;
            OnPropertyChanged(nameof(RecentMeasurements));
        }
    }

    public IEnumerable<Alert> RecentAlerts
    {
        get => _recentAlerts;
        set
        {
            _recentAlerts = value;
            OnPropertyChanged(nameof(RecentAlerts));
        }
    }

    public void SetCurrentThermometer(string thermometerName)
    {
        _dataManager.SetCurrentThermometer(thermometerName);
        _currentThermometer = _dataManager.Configuration.Thermometers.FirstOrDefault();
        OnPropertyChanged(nameof(CurrentThermometer));
        UpdateRecentMeasurements();
        UpdateRecentAlerts();
    }
    
    public void OnThermometerSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems[0] is ThermometerConfiguration selectedThermometer)
        {
            SetCurrentThermometer(selectedThermometer.Name);
        }
    }
    

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


