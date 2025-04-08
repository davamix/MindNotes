using Microsoft.Extensions.Configuration;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Providers;

public interface IEmbeddingsProvider {
    Task<List<float[]>> GenerateEmbeddings(string text);
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

        var isOllamaRunning = await _ollamaApiClient.IsRunningAsync();

        if (!isOllamaRunning) {
            throw new Exception("Ollama server is not running.");
        }
    }

    public async Task<List<float[]>> GenerateEmbeddings(string text) {
        if (string.IsNullOrWhiteSpace(text)) {
            throw new ArgumentException("Text cannot be null or empty.", nameof(text));
        }

        var response = await _ollamaApiClient.EmbedAsync(text);

        return response.Embeddings;
    }
}
