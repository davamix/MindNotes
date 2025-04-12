using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using MindNotes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Desktop.Converters;
public class StringToInfoBarSeverityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        var severity = (NotificationSeverity)value;

        return severity switch {
            NotificationSeverity.Info => InfoBarSeverity.Informational,
            NotificationSeverity.Success => InfoBarSeverity.Success,
            NotificationSeverity.Warning => InfoBarSeverity.Warning,
            NotificationSeverity.Error => InfoBarSeverity.Error,
            _ => InfoBarSeverity.Informational
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}
