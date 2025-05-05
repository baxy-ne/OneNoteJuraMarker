using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;
using OneNoteJuraMarker.OneNoteLogic;
using OneNoteJuraMarker.Utils;
using OneNoteJuraMarker.ViewModels;
using OneNoteJuraMarker.Views;

namespace OneNoteJuraMarker
{
    partial class App
    {
        private IServiceProvider _serviceProvider;
        private static IServiceProvider ConfigureServices()
        {
            var provider = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                    .Build())
                .AddSingleton<IWindowSizeUtility, WindowSizeUtility>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainPage>()
                .AddSingleton<MainViewModel>()
                .AddSingleton<OneNoteProgram>()
                .BuildServiceProvider(true);

            return provider;
        }
    }
}
