using CommunityToolkit.Mvvm.ComponentModel;
using MindNotes.Core.Application;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindNotes.Core.Models;
using CommunityToolkit.Mvvm.Input;
using Windows.ApplicationModel.DataTransfer;

namespace MindNotes.Desktop.ViewModels;

public partial class NotificationsViewModel : ObservableObject {
    private readonly INotificationHub _notificationHub;

    public ObservableCollection<Notification> Notifications { get; set; } = new();

    [ObservableProperty]
    private bool showNotificationPanel = false;

    public NotificationsViewModel(INotificationHub notificationHub) {
        _notificationHub = notificationHub;

        _notificationHub.NotificationReceived += ShowNotification;

        LoadNotifications();
    }

    private void ShowNotification(Notification notification) {
        Notifications.Add(notification);

        ShowNotificationPanel = true;
    }

    private void LoadNotifications() {
        foreach (var n in _notificationHub.ReadNotifications()) {
            Notifications.Add(n);
        }

        ShowNotificationPanel = Notifications.Any();
    }

    [RelayCommand]
    private void CopyNotification(Notification notification) {
        var data = new DataPackage {
            RequestedOperation = DataPackageOperation.Copy
        };

        var content = $"{notification.Message}\n{notification.Content}";

        data.SetText(content);
        Clipboard.SetContent(data);
    }

    [RelayCommand]
    private void DeleteNotification(Notification notification) {
        Notifications.Remove(notification);

        ShowNotificationPanel = Notifications.Any();
    }
}
