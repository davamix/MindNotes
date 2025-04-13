using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MindNotes.Core.Models;
using MindNotes.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MindNotes.Desktop.ViewModels;

public partial class NotesViewModel : ObservableObject {
    private readonly INotesService _notesService;

    public ObservableCollection<Note> Notes { get; set; } = new();

    [ObservableProperty]
    private string promptText;

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
        try {
            await _notesService.DeleteNoteAsync(note.Id);

            Notes.Remove(note);
        } catch (Exception ex) {
            Notify("Error on delete note.", ex.Message, NotificationSeverity.Error);
        }
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
        var message = "This is a notification";
        var longContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus non dapibus purus, vel elementum purus. Vestibulum in placerat turpis, hendrerit pellentesque turpis. Pellentesque varius elit orci, vel scelerisque erat consectetur in. Sed eget felis libero. Praesent ligula sapien, imperdiet in odio non, aliquet placerat ligula. Vestibulum pulvinar elit sed lobortis vehicula. Quisque lobortis odio id dui interdum, in bibendum dui fermentum. Vestibulum non efficitur arcu. Ut lacinia, augue ut placerat tempor, mi arcu lobortis sem, sit amet malesuada mauris tellus ac mi. Morbi vitae viverra lorem, ut venenatis ex. Ut faucibus quam vitae ex aliquam, ac lacinia dolor finibus. Phasellus finibus malesuada ante in bibendum. Duis dapibus lacinia auctor. Mauris aliquet lectus eu eleifend laoreet. In semper vulputate vulputate.";

        if (severity == "1")
            Notify(message, longContent, NotificationSeverity.Error);
        if (severity == "2")
            Notify(message, "Severity: " + severity, NotificationSeverity.Warning);
        if (severity == "3")
            Notify(message, "Severity: " + severity, NotificationSeverity.Success);
        if (severity == "4")
            Notify(message, "Severity: " + severity, NotificationSeverity.Info);

        OnPropertyChanged(nameof(Notification));
    }

    private void Notify(string message, string content, NotificationSeverity severity) {
        Notification = new Notification() {
            Message = message,
            Content = content,
            Severity = severity,
            IsOpen = true
        };

        OnPropertyChanged(nameof(Notification));
    }
}
