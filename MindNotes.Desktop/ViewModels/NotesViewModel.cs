using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MindNotes.Core.Models;
using MindNotes.Core.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MindNotes.Desktop.ViewModels;

public partial class NotesViewModel : ObservableObject {
    private readonly INotesService _notesService;

    public ObservableCollection<Note> Notes { get; set; } = new();

    [ObservableProperty]
    private string promptText;

    //[ObservableProperty]
    //private string notificationContent;
    [ObservableProperty]
    private Notification notification;

    public NotesViewModel(INotesService notesService) {
        _notesService = notesService;

        //notificationContent = "C";
        notification = new Notification();

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
    private async Task AddNote() {
        var newNote = await _notesService.AddNoteAsync(PromptText);

        Notes.Insert(0, newNote);

        PromptText = string.Empty;
    }

    [RelayCommand]
    private async Task DeleteNote(Note note) {
        await _notesService.DeleteNoteAsync(note.Id);

        Notes.Remove(note);
    }

    [RelayCommand]
    private async Task SearchNotes(string query) {
        var notes = await _notesService.SearchNotesAsync(query.ToString());
        Notes.Clear();
        foreach (var note in notes) {
            Notes.Add(note);
        }
    }

    [RelayCommand]
    private async Task ReloadNotes(string text) {
        if (string.IsNullOrEmpty(text)) {
            LoadNotes();
        }
    }

    [RelayCommand]
    private void ShowNotification(string severity) {
        Notification.IsOpen = true;
        Notification.Content = "This is a notification";

        if (severity == "1")
            Notification.Severity = NotificationSeverity.Error;
        if (severity == "2")
            Notification.Severity = NotificationSeverity.Warning;
        if (severity == "3")
            Notification.Severity = NotificationSeverity.Success;
        if (severity == "4")
            Notification.Severity = NotificationSeverity.Info;

        OnPropertyChanged(nameof(Notification));
    }
}
