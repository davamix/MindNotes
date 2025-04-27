using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MindNotes.Core.Models;

namespace MindNotes.Desktop.ViewModels;

public partial class NotesViewModel : ObservableObject {

    [RelayCommand]
    private async Task UpdateNote(Note note) {
        if (note == null) {
            Notify("Error on update note.", "Note is null.", NotificationSeverity.Error);
            return;
        }

        try {
            await _notesService.UpdateNoteAsync(note);

            var oldNoteIndex = Notes.IndexOf(note);
            Notes.Move(oldNoteIndex, 0);

            BigNote = new Note() { Id = note.Id, Content = note.Content };
            OnPropertyChanged(nameof(BigNote));
        } catch (Exception ex) {
            Notify("Error on update note.", ex.Message, NotificationSeverity.Error);
        }
    }

    [RelayCommand]
    private async Task AddNote(string content) {
        try {
            var newNote = await _notesService.AddNoteAsync(content);

            Notes.Insert(0, newNote);
            PromptText = string.Empty;
            OnPropertyChanged(nameof(PromptText));

        } catch (Exception ex) {
            Notify("Error on add note.", ex.Message, NotificationSeverity.Error);
        }
    }

    [RelayCommand]
    private async Task DeleteNote(Note note) {
        try {
            await _notesService.DeleteNoteAsync(note.Id);

            Notes.Remove(note);
        } catch (Exception ex) {
            Notify("Error on delete note.", ex.Message, NotificationSeverity.Error);
        }
    }

    [RelayCommand]
    private async Task SearchNotes(string query) {
        if (string.IsNullOrEmpty(query)) {
            LoadNotes();
        } else {
            try {
                var notes = await _notesService.SearchNotesAsync(query.ToString());
                Notes.Clear();
                foreach (var note in notes) {
                    Notes.Add(note);
                }
            } catch (Exception ex) {
                Notify("Error on search notes.", ex.Message, NotificationSeverity.Error);
            }
        }
    }

    [RelayCommand]
    private void ShowBigNote(Note note) {
        BigNote = note;
        IsBigNoteShown = true;
    }

    [RelayCommand]
    private void HideBigNote() {
        IsBigNoteShown = false;
    }
}
