using MindNotes.Core.Models;
using MindNotes.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Services;

public interface INotesService {
    Task<IList<Note>> GetNotesAsync();
    Task<Note> GetNoteByIdAsync(Guid id);
    Task<Note> AddNoteAsync(string content);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(Guid id);
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
        var embeddings = await _embeddingsProvider.GenerateEmbeddings(content);

        return await _provider.AddNoteAsync(new() {
            Content = content,
            Embeddings = embeddings,
        });
    }

    public async Task UpdateNoteAsync(Note note) {
        await _provider.UpdateNoteAsync(note);
    }

    public async Task DeleteNoteAsync(Guid id) {
        await _provider.DeleteNoteAsync(id);
    }

    public async Task<Note> GetNoteByIdAsync(Guid id) {
        return await _provider.GetNoteById(id);
    }

    public async Task<IList<Note>> GetNotesAsync() {
        return await _provider.GetNotes();
    }
}
