using MindNotes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Providers;
public class FakeProvider : IDatabaseProvider {
    private List<Note> _notes = new();

    private Dictionary<int, string> _guids = new() {
        { 1, "96641CF9-0B3E-4098-B3F3-4DF85972B04D" },
        { 2, "960213E1-F213-401E-976F-186AC1F7B3F1" },
        { 3, "30201456-48C6-435D-9C99-C9DBC3C8BB30" },
        { 4, "9490E17F-64F2-4F6D-9384-10279D861325" },
        { 5, "FAF00EDA-A8EE-4FEF-B0FB-0D15D3F85B46" },
        { 6, "0CE50FFE-EB13-4E3D-987F-6817E46106E4" },
        { 7, "661C2577-C8D3-4E4D-B008-2DC2DFE0BF57" },
        { 8, "24E065C7-37EB-4995-A54E-88629C52DFBF" },
        { 9, "4E5D96FB-D902-4C80-AD7E-0101C15F3625" },
    };

    public FakeProvider() {
        InitializeNotes();
    }

    public Task<List<Note>> GetNotes() {
        return Task.FromResult(_notes);
    }

    public Task<Note> GetNoteById(Guid id) {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        
        if (note == null) {
            throw new KeyNotFoundException($"Note with ID {id} not found.");
        }

        return Task.FromResult(note);
    }

    public Task<Note> AddNoteAsync(Note note) {
        note.Id = Guid.NewGuid();
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

    public Task DeleteNoteAsync(Guid id) {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note != null) {
            _notes.Remove(note);
        }

        return Task.CompletedTask;
    }

    private void InitializeNotes() {
        _notes = new List<Note>() {
            new Note() { Id = GetGuid(1), Content = "Note 1" },
            new Note() { Id = GetGuid(2), Content = "Note 2" },
            new Note() { Id = GetGuid(3), Content = "Note 3" },
            new Note() { Id = GetGuid(4), Content = "Note 4" },
            new Note() { Id = GetGuid(5), Content = "Note 5" },
            new Note() { Id = GetGuid(6), Content = "Note 6" },
            new Note() { Id = GetGuid(7), Content = "Note 7" },
            new Note() { Id = GetGuid(8), Content = "Note 8" },
            new Note() { Id = GetGuid(9), Content = "Note 9" },
        };
    }

    private Guid GetGuid(int id) {
        return Guid.Parse(_guids[id]);
    }
}
