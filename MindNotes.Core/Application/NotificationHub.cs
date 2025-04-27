using MindNotes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Application;

public interface INotificationHub {
    event Action<Notification> NotificationReceived;
    void Notify(Notification notification);
    void Register(Notification notification);
    IEnumerable<Notification> ReadNotifications();
}

public class NotificationHub : INotificationHub {
    public event Action<Notification> NotificationReceived;

    private Queue<Notification> _notifications = new();

    public void Notify(Notification notification) {
        //_notifications.Add(notification);

        NotificationReceived?.Invoke(notification);
    }

    public void Register(Notification notification) {
        _notifications.Enqueue(notification);
    }

    public IEnumerable<Notification> ReadNotifications() {
        while (_notifications.Any()) {
            yield return _notifications.Dequeue();
        }
    }
}
