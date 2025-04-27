using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MindNotes.Core.Application;
using MindNotes.Core.Providers;
using MindNotes.Core.Services;
using MindNotes.Desktop.ViewModels;

namespace MindNotes.Desktop.Bootstrap;
public static class IocConfiguration {

    public static IServiceCollection RegisterProviders(this IServiceCollection services) {
        //services.AddSingleton<IProvider, FakeProvider>();
        services.AddSingleton<IDatabaseProvider, QdrantProvider>();
        services.AddSingleton<IEmbeddingsProvider, OllamaEmbeddingsProvider>();

        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services) {
        services.AddSingleton<INotesService, NotesService>();
        services.AddSingleton<INetworkService, NetworkService>();

        return services;
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services) {
        services.AddTransient<NotesViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<NotificationsViewModel>();

        return services;
    }

    public static IServiceCollection RegisterConfiguration(this IServiceCollection services) {
        services.AddSingleton(typeof(IConfiguration), sp => new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build());

        return services;
    }

    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services) {
        services.AddSingleton<INotificationHub, NotificationHub>();
        return services;
    }
}
