using CommunityToolkit.Mvvm.ComponentModel;
using MindNotes.Core.Application;
using MindNotes.Core.Models;
using MindNotes.Core.Services;
using System;
using System.Collections.ObjectModel;

namespace MindNotes.Desktop.ViewModels;

public partial class NotesViewModel : ObservableObject {
    private readonly INotesService _notesService;
    private readonly INotificationHub _notificationHub;

    public ObservableCollection<Note> Notes { get; set; } = new();

    [ObservableProperty]
    private string promptText;

    [ObservableProperty]
    private bool isBigNoteShown;

    [ObservableProperty]
    private Note bigNote;

    [ObservableProperty]
    private bool isSmartNote;

    public NotesViewModel(INotesService notesService,
        INotificationHub notificationHub) {
        _notesService = notesService;
        _notificationHub = notificationHub;
        LoadNotes();
    }

    private async void LoadNotes() {
        Notes.Clear();

        try {
            var notes = await _notesService.GetNotesAsync();

            foreach (var note in notes) {
                Notes.Insert(0, note);
            }
        } catch (Exception ex) {
            Notify("Error on load notes", ex.Message, NotificationSeverity.Error);
        }
    }

    private void Notify(string message, string content, NotificationSeverity severity) {
        _notificationHub.Notify(new Notification() {
            Message = message,
            Content = content,
            Severity = severity
        });
    }
}
