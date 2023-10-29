using Avalonia.Controls;
using Stas.Monitor.Domains;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

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
        this.FindControl<ComboBox>("Thermo")!.SelectionChanged += Presenter.OnThermometerSelectionChanged;
    }
    
    public void OnThermometerSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Presenter.OnThermometerSelectionChanged(sender, e);
    }
}
