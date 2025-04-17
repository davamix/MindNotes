using MindNotes.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Services;

public interface INetworkService {
    Task<bool> TestOllamaConnection();
    Task<bool> TestQdrantConnection();
}

public class NetworkService : INetworkService {
    private readonly IDatabaseProvider _databaseProvider;
    private readonly IEmbeddingsProvider _embeddingsProvider;

    public NetworkService(IDatabaseProvider databaseProvider, IEmbeddingsProvider embeddingsProvider) {
        _databaseProvider = databaseProvider;
        _embeddingsProvider = embeddingsProvider;
    }
    public async Task<bool> TestOllamaConnection() {
        try {
            return await _embeddingsProvider.TestConnection();
        } catch {
            throw;
        }
    }

    public async Task<bool> TestQdrantConnection() {
        try {
            return await _databaseProvider.TestConnection();
        } catch {
            throw;
        }
    }
}
