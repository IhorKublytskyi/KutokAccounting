using Microsoft.Extensions.Logging;

namespace KutokAccounting;

public partial class App : Application
{
    public App(ILogger<App> logger)
    {
        InitializeComponent();

        logger.LogInformation("Application starting");
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage())
        {
            Title = "Kutok"
        };
    }
}