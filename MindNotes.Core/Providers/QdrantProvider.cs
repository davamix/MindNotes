using MindNotes.Core.Models;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Providers;
public class QdrantProvider : IDatabaseProvider {
    private const string COLLECTION_NAME = "mind_notes_collection";

    QdrantClient _client;

    public QdrantProvider() {
        Task.Run(InitializeDatabase).Wait();
    }

    private async Task InitializeDatabase() {
        _client = new QdrantClient("localhost", 6334);

        await CreateCollection();
        var result = await _client.HealthAsync();

        Debug.WriteLine(result);
    }

    private async Task CreateCollection() {
        var collections = await _client.ListCollectionsAsync();

        if (collections.Any(c => c == COLLECTION_NAME)) return;

        await _client.CreateCollectionAsync(COLLECTION_NAME);

    }

    public Task<Note> AddNoteAsync(Note note) {
        throw new NotImplementedException();
    }

    public Task DeleteNoteAsync(string id) {
        throw new NotImplementedException();
    }

    public Task<Note> GetNoteById(string id) {
        throw new NotImplementedException();
    }

    public Task<List<Note>> GetNotes() {
        throw new NotImplementedException();
    }

    public Task UpdateNoteAsync(Note note) {
        throw new NotImplementedException();
    }
}
