using Avalonia.Interactivity;
using Serilog;

namespace Stas.Monitor.Presentations;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using Avalonia.Controls;
using Domains;

public class MainPresenter : INotifyPropertyChanged
{
    private readonly IDisplayManager _displayManager;
    private IDataManager _dataManager;
    private ThermometerConfiguration _currentThermometer;
    private IEnumerable<Measurement> _recentMeasurements;
    private IEnumerable<Humidity> _recentHumidities;
    private string _selectedDuration = "1 minute";
    private bool _showTemperature = true;
    private bool _showHumidity = true;

    public MainPresenter(Configuration configuration, IMeasurementServices measurementService)
    {
        _currentThermometer = configuration.Thermometers.FirstOrDefault()!;
        _dataManager = new DataManager(configuration, measurementService, _currentThermometer ?? throw new InvalidOperationException() );
        var timerManager = new TimerManager();
        IDataItemConverter dataItemConverter = new DataItemConverter();
        IUiThreadInvoker uiThreadInvoker = new UiThreadInvoker();
        SelectedThermometer = new ThermometerPresenter(dataItemConverter, uiThreadInvoker);
        timerManager.StartTimer(OnUpdateTimerElapsed!);
        _displayManager = new DisplayManager(SelectedThermometer);
    }

    public Configuration Configuration
    {
        get => _dataManager.Configuration;
    }

    public string SelectedDuration
    {
        get => _selectedDuration;
        set
        {
            if (_selectedDuration != value)
            {
                _selectedDuration = value;
                OnPropertyChanged();
                UpdateDisplayBasedOnDuration();
            }
        }
    }

    public IThermometerPresenter SelectedThermometer { get; set; }

    public ThermometerConfiguration CurrentThermometer
    {
        get => _currentThermometer;
        set
        {
            _currentThermometer = value;
            OnPropertyChanged();
        }
    }

    public void UpdateDisplayBasedOnDuration()
    {
        DateTime fromTime = CalculateFromTimeBasedOnDuration();
        _recentMeasurements = _dataManager.GetRecentMeasurements(_currentThermometer.Name, fromTime);
        _recentHumidities = _dataManager.GetRecentHumidities(_currentThermometer.Name, fromTime);
        if (_showTemperature)
        {
            SelectedThermometer.UpdateDataItemsTemperature(_recentMeasurements);
        }

        if (_showHumidity)
        {
            SelectedThermometer.UpdateDataItemsHumidity(_recentHumidities);
        }
    }

    private DateTime CalculateFromTimeBasedOnDuration()
    {
        var lastTimestamp = _dataManager.GetLastMeasurementTimestamp(_currentThermometer.Name);

        // Si le timestamp n'est pas disponible, retournez une valeur par défaut.
        if (lastTimestamp == DateTime.MinValue)
        {
            Log.Error("Thermometer not found or no data available.");
            return DateTime.Now;
        }

        switch (_selectedDuration)
        {
            case "30 seconds":
                return lastTimestamp.AddSeconds(-30);
            case "1 minute":
                return lastTimestamp.AddMinutes(-1);
            case "5 minutes":
                return lastTimestamp.AddMinutes(-5);
            default:
                return lastTimestamp.AddMinutes(-1);
        }
    }

    public void UpdateRecentMeasurements()
    {
        var fromTime = CalculateFromTimeBasedOnDuration();
        _recentMeasurements = _dataManager.GetRecentMeasurements(_currentThermometer.Name, fromTime);
        OnPropertyChanged(nameof(RecentMeasurements));
    }

    public void UpdateRecentHumidities()
    {
        var fromTime = CalculateFromTimeBasedOnDuration();
        _recentHumidities = _dataManager.GetRecentHumidities(_currentThermometer.Name, fromTime);
        OnPropertyChanged(nameof(RecentHumidities));
    }

    public void OnUpdateTimerElapsed(object sender, ElapsedEventArgs e)
    {
        UpdateRecentMeasurements();
        UpdateRecentHumidities();
        if (_showTemperature)
        {
            SelectedThermometer.UpdateDataItemsTemperature(_recentMeasurements);
        }

        if (_showHumidity)
        {
            SelectedThermometer.UpdateDataItemsHumidity(_recentHumidities);
        }
    }

    public IEnumerable<Measurement> RecentMeasurements
    {
        get => _recentMeasurements;
        set
        {
            _recentMeasurements = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<Humidity> RecentHumidities
    {
        get => _recentHumidities;
        set
        {
            _recentHumidities = value;
            OnPropertyChanged();
        }
    }

    public void SetCurrentThermometer(string thermometerName)
    {
        _dataManager.SetCurrentThermometer(thermometerName);
        _currentThermometer = _dataManager.Configuration.Thermometers.FirstOrDefault(t => t.Name == thermometerName)!;
        if (_currentThermometer == null)
        {
            Log.Error($"No thermometer found with name: {thermometerName}");
            throw new InvalidOperationException($"No thermometer found with name: {thermometerName}");
        }

        OnPropertyChanged(nameof(CurrentThermometer));
        UpdateRecentMeasurements();
        UpdateRecentHumidities();
    }

    public void OnThermometerSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems != null && e.AddedItems.Count > 0 && e.AddedItems[0] is ThermometerConfiguration selectedThermometer)
        {
            SetCurrentThermometer(selectedThermometer.Name);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void OnShowTemperatureChanged(object sender, RoutedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        _showTemperature = checkBox?.IsChecked ?? false;
        _displayManager.UpdateTemperatureDisplay(_recentMeasurements, _showTemperature);
    }

    public void OnShowHumidityChanged(object sender, RoutedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        _showHumidity = checkBox?.IsChecked ?? false;
        _displayManager.UpdateHumidityDisplay(_recentHumidities, _showHumidity);
    }
}
