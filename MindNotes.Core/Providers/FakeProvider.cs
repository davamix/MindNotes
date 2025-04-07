using MindNotes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Providers;
public class FakeProvider : IDatabaseProvider {
    private List<Note> _notes = new();

    public FakeProvider() {
        InitializeNotes();
    }

    public Task<List<Note>> GetNotes() {
        return Task.FromResult(_notes);
    }

    public Task<Note> GetNoteById(string id) {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        
        if (note == null) {
            throw new KeyNotFoundException($"Note with ID {id} not found.");
        }

        return Task.FromResult(note);
    }

    public Task<Note> AddNoteAsync(Note note) {
        note.Id = Guid.NewGuid().ToString();
        note.CreatedAt = DateTime.UtcNow;

        _notes.Add(note);

        return Task.FromResult(note);
    }

    public Task UpdateNoteAsync(Note note) {
        var existingNote = _notes.FirstOrDefault(n => n.Id == note.Id);
        
        if (existingNote != null) {
            existingNote.Content = note.Content;
            existingNote.UpdatedAt = DateTime.UtcNow;

            return Task.CompletedTask;
        }

        throw new KeyNotFoundException($"Note with ID {note.Id} not found.");
    }

    public Task DeleteNoteAsync(string id) {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note != null) {
            _notes.Remove(note);
        }

        return Task.CompletedTask;
    }

    private void InitializeNotes() {
        _notes = new List<Note>() {
            new Note() { Id = "1", Content = "Note 1" },
            new Note() { Id = "2", Content = "Note 2" },
            new Note() { Id = "3", Content = "Note 3" },
            new Note() { Id = "4", Content = "Note 4" },
            new Note() { Id = "5", Content = "Note 5" },
            new Note() { Id = "6", Content = "Note 6" },
            new Note() { Id = "7", Content = "Note 7" },
            new Note() { Id = "8", Content = "Note 8" },
            new Note() { Id = "9", Content = "Note 9" },
        };
    }
}
