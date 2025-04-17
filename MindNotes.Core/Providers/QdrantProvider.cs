﻿using Google.Protobuf.Collections;
using Microsoft.Extensions.Configuration;
using MindNotes.Core.Models;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace MindNotes.Core.Providers;
public class QdrantProvider : IDatabaseProvider {

    private readonly IConfiguration _configuration;
    QdrantClient _client;
    string _collection;
    ulong _embeddingsVectorSize;

    public QdrantProvider(IConfiguration configuration) {
        _configuration = configuration;

        Task.Run(Initialize).Wait();
    }

    private async Task Initialize() {
        _client = new QdrantClient(new Uri(_configuration["AppSettings:Qdrant:Server"]));
        _collection = _configuration["AppSettings:Qdrant:Collection"];
        _embeddingsVectorSize = ulong.Parse(_configuration["AppSettings:Ollama:OutputVectorSize"]);

        await CreateCollection();
    }

    private async Task CreateCollection() {
        var collections = await _client.ListCollectionsAsync();

        if (collections.Any(c => c == _collection)) return;

        await _client.CreateCollectionAsync(_collection, new VectorParams {
            Size = _embeddingsVectorSize,
            Distance = Distance.Cosine
        });
    }

    public async Task<Note> AddNoteAsync(Note note) {
        note.Id = Guid.NewGuid();
        note.CreatedAt = DateTime.UtcNow;
        note.UpdatedAt = DateTime.UtcNow;

        return await SaveNoteAsync(note);
    }

    public async Task DeleteNoteAsync(Guid id) {
        _ = await _client.DeleteAsync(_collection, id);
    }

    public Task<Note> GetNoteById(Guid id) {
        throw new NotImplementedException();
    }

    public async Task<List<Note>> GetNotes(Guid offsetId = default) {
        var notes = new List<Note>();

        var result = await _client.ScrollAsync(
            collectionName: _collection,
            limit: 50,
            offset: offsetId == Guid.Empty ? null : offsetId,
            payloadSelector: true);

        foreach (var value in result.Result) {
            var note = value.Payload.ToNote();
            note.Id = Guid.Parse(value.Id.Uuid);
            notes.Add(note);
        }

        return notes;
    }

    public async Task UpdateNoteAsync(Note note) {
        note.UpdatedAt = DateTime.UtcNow;

        _ = await SaveNoteAsync(note);
    }

    public async Task<List<Note>> SearchNotes(QdrantQuery query, ulong limit = 10) {
        var notes = new List<Note>();

        var result = await _client.QueryAsync(
            collectionName: _collection,
            query: query.VectorQuery,
            limit: limit);

        foreach (var value in result) {
            //if (value.Score < 0.5) continue;
            var note = value.Payload.ToNote();
            note.Id = Guid.Parse(value.Id.Uuid);
            notes.Add(note);
        }

        return notes;
    }

    public async Task<bool> TestConnection() {
        var health = await _client.HealthAsync();
        return health.Version != null;
    }

    private async Task<Note> SaveNoteAsync(Note note) {
        var result = await _client.UpsertAsync(
                    collectionName: _collection,
                    points: new List<PointStruct> {
                new() {
                    Id = note.Id,
                    Vectors = note.Embeddings.SelectMany(x=>x).ToArray()
                    ,
                    Payload = {
                        ["content"] = note.Content,
                        ["created_at"] = note.CreatedAt.ToString("o"),
                        ["updated_at"] = note.UpdatedAt.ToString("o"),
                    }
                }
                    });

        return note;
    }


}

public static class PayloadToNoteMapper {
    public static Note ToNote(this MapField<string, Value> value) {
        var note = new Note();
        if (value.TryGetValue("content", out var content)) {
            note.Content = content.StringValue;
        }
        if (value.TryGetValue("created_at", out var createdAt)) {
            note.CreatedAt = DateTime.Parse(createdAt.StringValue);
        }
        if (value.TryGetValue("updated_at", out var updatedAt)) {
            note.UpdatedAt = DateTime.Parse(updatedAt.StringValue);
        }

        return note;
    }
}
