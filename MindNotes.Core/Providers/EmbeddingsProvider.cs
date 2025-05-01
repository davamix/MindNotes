using Microsoft.Extensions.Configuration;
using OllamaSharp;

namespace MindNotes.Core.Providers;

public interface IEmbeddingsProvider {
    Task<List<float[]>> GenerateEmbeddings(string text);
    Task<bool> TestConnection();
}

public class EmbeddingsProvider : OllamaProviderBase, IEmbeddingsProvider {
    private string _embeddingModel;

    public EmbeddingsProvider(IConfiguration configuration)
        : base(configuration) { }

    protected override Task Initialize() {
        _embeddingModel = Configuration["AppSettings:Ollama:EmbeddingModel"];

        return base.Initialize();
    }

    public async Task<List<float[]>> GenerateEmbeddings(string text) {
        if (string.IsNullOrWhiteSpace(text)) {
            throw new ArgumentException("Text cannot be null or empty.", nameof(text));
        }

        try {
            OllamaApiClient.SelectedModel = _embeddingModel;
            var response = await base.OllamaApiClient.EmbedAsync(text);

            return response.Embeddings;
        } catch {
            throw;
        }
    }

    public async Task<bool> TestConnection() => await base.TestConnection();
}
