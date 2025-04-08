using MindNotes.Core.Models;

namespace MindNotes.Core.Providers;

public interface IDatabaseProvider {
    Task<List<Note>> GetNotes();
    Task<Note> AddNoteAsync(Note note);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(Guid id);
    Task<Note> GetNoteById(Guid id);
}
