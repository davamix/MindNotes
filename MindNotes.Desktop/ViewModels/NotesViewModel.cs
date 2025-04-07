using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MindNotes.Core.Models;
using MindNotes.Core.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MindNotes.Desktop.ViewModels;
public partial class NotesViewModel : ObservableObject{
    private readonly INotesService _notesService;

    public ObservableCollection<Note> Notes { get; set; } = new();

    [ObservableProperty]
    private string promptText;

    public NotesViewModel(INotesService notesService) {
        _notesService = notesService;

        LoadNotes();
    }

    private async void LoadNotes() {
        Notes.Clear();
        
        var notes = await _notesService.GetNotesAsync();

        foreach (var note in notes) {
            Notes.Insert(0, note);
        }
    }

    [RelayCommand]
    private async Task UpdateNote(Note note) {
        await _notesService.UpdateNoteAsync(note);

        var updatedNote = new Note() { Id = note.Id, Content = note.Content };
        var oldNoteIndex = Notes.IndexOf(note);
        Notes.Remove(note);
        Notes.Insert(oldNoteIndex, updatedNote);
    }

    [RelayCommand]
    private async Task AddNote(){
        var newNote = await _notesService.AddNoteAsync(PromptText);

        Notes.Insert(0, newNote);

        PromptText = string.Empty;
    }

    [RelayCommand]
    private async Task DeleteNote(Note note) {
        await _notesService.DeleteNoteAsync(note.Id);

        Notes.Remove(note);
    }

}
