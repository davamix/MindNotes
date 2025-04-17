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
}

public class NotificationHub : INotificationHub {
    public event Action<Notification> NotificationReceived;

    //private readonly List<Notification> _notifications = new();

    public void Notify(Notification notification) {
        //_notifications.Add(notification);

        NotificationReceived?.Invoke(notification);
    }
}
