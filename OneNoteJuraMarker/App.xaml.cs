using Microsoft.Extensions.DependencyInjection;


namespace OneNoteJuraMarker;

public partial class App
{
    public App()
    {
        InitializeComponent();
    }
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _serviceProvider = ConfigureServices();
        _serviceProvider.GetRequiredService<MainWindow>().Activate();
    }
}
