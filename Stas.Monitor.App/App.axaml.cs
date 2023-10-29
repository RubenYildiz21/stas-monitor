using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Serilog;
using Stas.Monitor.Views;
using Stas.Monitor.Domains;
using Stas.Monitor.Infrastructures;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.App;

public partial class App : Application
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
            
            SetupApp(desktop?.Args ?? Array.Empty<string>());
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
                IConfigurationReader reader = new IniConfigurationReader(fileStream);
                MeasurementServices measurementServices = new MeasurementServices(reader.GetCsvFilePath());
                AlertServices alertServices = new AlertServices(reader.GetCsvAlertFilePath());
                MainPresenter presenter = new MainPresenter(reader.GetConfiguration(), measurementServices, alertServices);
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