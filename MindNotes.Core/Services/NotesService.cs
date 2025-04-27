using MindNotes.Core.Models;
using MindNotes.Core.Providers;

namespace MindNotes.Core.Services;

public interface INotesService {
    Task<IList<Note>> GetNotesAsync();
    Task<Note> GetNoteByIdAsync(Guid id);
    Task<Note> AddNoteAsync(string content);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(Guid id);
    Task<IList<Note>> SearchNotesAsync(string query, ulong limit = 10);
}

public class NotesService : INotesService {

    private readonly IDatabaseProvider _provider;
    private readonly IEmbeddingsProvider _embeddingsProvider;

    public NotesService(IDatabaseProvider provider,
        IEmbeddingsProvider embeddingsProvider) {
        _provider = provider;
        _embeddingsProvider = embeddingsProvider;
    }

    public async Task<Note> AddNoteAsync(string content) {
        try {
            var embeddings = await _embeddingsProvider.GenerateEmbeddings(content);

            return await _provider.AddNoteAsync(new() {
                Content = content,
                Embeddings = embeddings,
            });
        } catch {
            throw;
        }
    }

    public async Task UpdateNoteAsync(Note note) {
        try {
            var embeddings = await _embeddingsProvider.GenerateEmbeddings(note.Content);
            note.Embeddings = embeddings;

            await _provider.UpdateNoteAsync(note);
        } catch {
            throw;
        }
    }

    public async Task DeleteNoteAsync(Guid id) {
        try {
            await _provider.DeleteNoteAsync(id);
        } catch {
            throw;
        }
    }

    public async Task<Note> GetNoteByIdAsync(Guid id) {
        try {
            return await _provider.GetNoteById(id);
        } catch {
            throw;
        }
    }

    public async Task<IList<Note>> GetNotesAsync() {
        try {
            return await _provider.GetNotes();
        } catch {
            throw;
        }
    }

    public async Task<IList<Note>> SearchNotesAsync(string query, ulong limit = 10) {
        try {
            var embeddings = await _embeddingsProvider.GenerateEmbeddings(query);

            var qdrantQuery = new QdrantQuery(embeddings, query);

            return await _provider.SearchNotes(qdrantQuery, limit);
        } catch {
            throw;
        }
    }
}
