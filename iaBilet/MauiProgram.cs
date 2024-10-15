using iaBilet.Core.Services;
using iaBilet.ViewModels;

using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microsoft.Maui.LifecycleEvents;
using iaBilet.ViewModels.Settings;
using iaBilet.Pos;

namespace iaBilet
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
#if WINDOWS
            builder.ConfigureLifecycleEvents(x =>
            {
                x.AddWindows(x =>
                {
                    x.OnActivated((w, e) =>
                    {
                    });

                    x.OnWindowCreated(w =>
                    {
                        if (w is not MauiWinUIWindow window)
                            return;

                        window.ExtendsContentIntoTitleBar = false;
                        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                        appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);

                        switch (appWindow.Presenter)
                        {
                            case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                                overlappedPresenter.SetBorderAndTitleBar(false, false);
                                overlappedPresenter.IsMaximizable = false;
                                overlappedPresenter.IsResizable = false;
                                overlappedPresenter.IsMinimizable = false;
                                overlappedPresenter.SetBorderAndTitleBar(false, false);
                                break;
                        }
                    });
                });
            });
#endif
           
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<iaBiletRestService>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<EventsViewModel>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddTransient<CartViewModel>();
            builder.Services.AddSingleton<PosViewModel>();
            builder.Services.AddSingleton<MainPage>();
            return builder.Build();
        }
    }
}
