using MindNotes.Core.Models;

namespace MindNotes.Core.Providers;

public interface IDatabaseProvider {
    Task<List<Note>> GetNotes(Guid offsetId = default);
    Task<Note> AddNoteAsync(Note note);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(Guid id);
    Task<Note> GetNoteById(Guid id);
    Task<List<Note>> SearchNotes(QdrantQuery query, ulong limit = 10);
    Task<bool> TestConnection();
}
