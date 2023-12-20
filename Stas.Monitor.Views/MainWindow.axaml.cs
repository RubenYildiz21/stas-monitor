using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

/// <inheritdoc />
public partial class MainWindow : Window
{
    public new MainPresenter Presenter { get; set; }

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainPresenter presenter)
    {
        Presenter = presenter;
        this.DataContext = presenter;
        InitializeComponent();
        this.FindControl<ComboBox>("Thermo")!.SelectionChanged += Presenter.OnThermometerSelectionChanged!;
        this.FindControl<ComboBox>("DurationSelector")!.SelectionChanged += OnDurationSelectionChanged!;
    }

    public void OnThermometerSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Presenter.OnThermometerSelectionChanged(sender, e);
    }

    public void OnShowTemperatureChanged(object sender, RoutedEventArgs e)
    {
        Presenter.OnShowTemperatureChanged(sender, e);
    }

    public void OnShowHumidityChanged(object sender, RoutedEventArgs e)
    {
        Presenter.OnShowHumidityChanged(sender, e);
    }

    public void OnDurationSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is ComboBoxItem selectedItem)
        {
            var selectedDuration = selectedItem.Content!.ToString();
            Presenter.SelectedDuration = selectedDuration!;
        }
    }
}
