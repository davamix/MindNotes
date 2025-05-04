using MindNotes.Core.Models;
using MindNotes.Core.Providers;
using System.Diagnostics;

namespace MindNotes.Core.Services;

public interface INotesService {
    Task<IList<Note>> GetNotesAsync();
    Task<Note> GetNoteByIdAsync(Guid id);
    Task<Note> AddNoteAsync(string content);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(Guid id);
    Task<IList<Note>> SearchNotesAsync(string query, ulong limit = 10);
    IAsyncEnumerable<string> SmartSearchNoteAsync(string query);
}

public class NotesService : INotesService {

    private readonly IDatabaseProvider _provider;
    private readonly IEmbeddingsProvider _embeddingsProvider;
    private readonly ILlmProvider _llmProvider;

    public NotesService(IDatabaseProvider provider,
        IEmbeddingsProvider embeddingsProvider,
        ILlmProvider llmProvider) {
        _provider = provider;
        _embeddingsProvider = embeddingsProvider;
        _llmProvider = llmProvider;
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

    public async IAsyncEnumerable<string> SmartSearchNoteAsync(string query) {
        // Get notes from the database based on the query
        var notes = await SearchNotesAsync(query);

        // Generate prompt
        var prompt = LlModel.Gemma3PromptTemplate
            .Replace("{user_prompt}", query);

        // Attach query and notes to the prompt
        // <query>query</query>
        // <note>content, date...</note>
        // <note>content, date...</note>
        // ...
        prompt = prompt.Replace("{notes}",
            string.Join("\n", notes.Select(x => LlModel.NotePromptTemplate
                .Replace("{content}", x.Content)
                .Replace("{note_id}", x.Id.ToString())
                .Replace("{date_updated}", x.UpdatedAt.ToString())
                .Replace("{date_created}", x.CreatedAt.ToString()))));

        // Call the LLM with the prompt
        var noteResponse = string.Empty;
        await foreach (var response in _llmProvider.Query(prompt)) {
            yield return response;
        }
    }
}
