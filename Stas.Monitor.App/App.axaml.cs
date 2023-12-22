namespace Stas.Monitor.App;

using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Domains;
using Infrastructures;
using Presentations;
using Serilog;
using Views;

public class App : Application
{
    private MainWindow? _mainWindow;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var log = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = log; // Définit le log dans un singleton (Beurk)
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            SetupApp(desktop.Args ?? Array.Empty<string>());
            desktop.MainWindow = _mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void SetupApp(string[] args)
    {
        if (args.Length < 2)
        {
            Log.Logger.Error("monitor: missing configuration file argument");
            return;
        }

        try
        {
            using (FileStream fileStream = new FileStream(args[1], FileMode.Open))
            {
                IFileReader fileReader = new FileReader();
                IConfigurationReader reader = new IniConfigurationReader(fileStream, fileReader);
                string dbUrl = reader.GetValue("0_db", "url");
                IDbConnectionFactory dbConnectionFactory = new DbConnectionFactory(dbUrl);
                ITemperatureRepository temperatureRepository = new TemperatureRepository(dbConnectionFactory);
                IHumidityRepository humidityRepository = new HumidityRepository(dbConnectionFactory);
                DatabaseService db = new DatabaseService(temperatureRepository, humidityRepository);
                MeasurementServices measurementServices = new MeasurementServices(db);
                MainPresenter presenter = new MainPresenter(reader.GetConfiguration(), measurementServices);
                _mainWindow = _mainWindow ?? new MainWindow(presenter);
                _mainWindow.Presenter = presenter;
            }
        }
        catch (FileNotFoundException e)
        {
            Log.Error($"File not found: {e.FileName}", e);
        }
    }
}
