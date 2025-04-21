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
    private Notification notification;

    [ObservableProperty]
    private bool isBigNoteShown;

    [ObservableProperty]
    private Note bigNote;

    public NotesViewModel(INotesService notesService,
        INotificationHub notificationHub) {
        _notesService = notesService;
        _notificationHub = notificationHub;

        _notificationHub.NotificationReceived += ShowNotification;
        notification = new Notification();

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
        ShowNotification(new Notification() {
            Message = message,
            Content = content,
            Severity = severity
        });
    }

    private void ShowNotification(Notification notification) {
        notification.IsOpen = true;

        Notification = notification;

        OnPropertyChanged(nameof(Notification));
    }


}
