﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Models;

public enum NotificationSeverity {
    Info,
    Success,
    Warning,
    Error
}
public class Notification {
    public string Content { get; set; } = string.Empty;
    public bool IsOpen { get; set; } = false;
    public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;
}
