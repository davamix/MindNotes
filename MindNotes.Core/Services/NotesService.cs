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
    Task<Note> GetNoteByIdAsync(string id);
    Task<Note> AddNoteAsync(string content);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(string id);
}
public class NotesService : INotesService {

    private readonly IDatabaseProvider _provider;

    public NotesService(IDatabaseProvider provider) {
        _provider = provider;
    }

    public async Task<Note> AddNoteAsync(string content) {
        return await _provider.AddNoteAsync(new() { Content = content });
    }

    public async Task UpdateNoteAsync(Note note) {
        await _provider.UpdateNoteAsync(note);
    }

    public async Task DeleteNoteAsync(string id) {
        await _provider.DeleteNoteAsync(id);
    }

    public async Task<Note> GetNoteByIdAsync(string id) {
        return await _provider.GetNoteById(id);
    }

    public async Task<IList<Note>> GetNotesAsync() {
        return await _provider.GetNotes();
    }
}
