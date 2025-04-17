using Microsoft.Extensions.Configuration;
using OllamaSharp;

namespace MindNotes.Core.Providers;

public interface IEmbeddingsProvider {
    Task<List<float[]>> GenerateEmbeddings(string text);
    Task<bool> TestConnection();
}

public class OllamaEmbeddingsProvider : IEmbeddingsProvider {
    private readonly IConfiguration _configuration;
    private IOllamaApiClient _ollamaApiClient;

    public OllamaEmbeddingsProvider(IConfiguration configuration) {
        _configuration = configuration;

        Task.Run(Initialize).Wait();
    }

    public async Task Initialize() {
        _ollamaApiClient = new OllamaApiClient(new Uri(_configuration["AppSettings:Ollama:Server"]));
        _ollamaApiClient.SelectedModel = _configuration["AppSettings:Ollama:EmbeddingModel"];

        var isOllamaRunning = await TestConnection();

        if (!isOllamaRunning) {
            throw new Exception("Ollama server is not running.");
        }
    }

    public async Task<List<float[]>> GenerateEmbeddings(string text) {
        if (string.IsNullOrWhiteSpace(text)) {
            throw new ArgumentException("Text cannot be null or empty.", nameof(text));
        }

        try {
            var response = await _ollamaApiClient.EmbedAsync(text);

            return response.Embeddings;
        } catch {
            throw;
        }

    }

    public async Task<bool> TestConnection() {
        try {
            return await _ollamaApiClient.IsRunningAsync();
        } catch {
            throw;
        }
    }
}
