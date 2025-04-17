using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MindNotes.Core.Models;

namespace MindNotes.Desktop.ViewModels;

public partial class NotesViewModel : ObservableObject {

	[RelayCommand]
	private async Task UpdateNote(Note note) {
		await _notesService.UpdateNoteAsync(note);

		var updatedNote = new Note() { Id = note.Id, Content = note.Content };
		var oldNoteIndex = Notes.IndexOf(note);
		Notes.Remove(note);
		Notes.Insert(oldNoteIndex, updatedNote);
	}

	[RelayCommand]
	private async Task AddNote() {
		try {
			var newNote = await _notesService.AddNoteAsync(PromptText);

			Notes.Insert(0, newNote);

			PromptText = string.Empty;
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

	[RelayCommand]
	private async Task ReloadNotes(string text) {
		if (string.IsNullOrEmpty(text)) {
			LoadNotes();
		}
	}
}
