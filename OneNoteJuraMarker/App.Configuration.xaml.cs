using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;
using OneNoteJuraMarker.Utils;
using OneNoteJuraMarker.ViewModels;

namespace OneNoteJuraMarker
{
    partial class App
    {
        private IServiceProvider _serviceProvider;

        public static IServiceProvider Services
        {
            get
            {
                IServiceProvider serviceProvider = ((App)Current)._serviceProvider 
                                                   ?? throw new InvalidOperationException("The service provider is not initialized");
                return serviceProvider;
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var provider = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                    .Build())
                .AddSingleton<IWindowSizeUtility, WindowSizeUtility>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainViewModel>()
                .BuildServiceProvider(true);

            return provider;
        }
    }
}