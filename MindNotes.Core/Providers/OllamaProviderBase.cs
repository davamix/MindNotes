using Microsoft.Extensions.Configuration;
using OllamaSharp;

namespace MindNotes.Core.Providers;
public abstract class OllamaProviderBase {
    protected readonly IConfiguration Configuration;
    protected IOllamaApiClient OllamaApiClient { get; set; }

    protected OllamaProviderBase(IConfiguration configuration) {
        Configuration = configuration;

        Task.Run(Initialize).Wait();
    }

    protected virtual async Task Initialize() {
        OllamaApiClient = new OllamaApiClient(new Uri(Configuration["AppSettings:Ollama:Server"]));

        var isOllamaRunning = await TestConnection();

        if (!isOllamaRunning) {
            throw new Exception("Ollama server is not running.");
        }
    }

    public virtual async Task<bool> TestConnection() {
        try {
            return await OllamaApiClient.IsRunningAsync();
        } catch {
            throw;
        }
    }
}
