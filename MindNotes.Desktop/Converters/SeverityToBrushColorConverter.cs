using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using MindNotes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Desktop.Converters;

public class SeverityToBrushColorConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        var severity = (NotificationSeverity)value;

        var gradient = new GradientStopCollection();

        switch (severity) {
            case NotificationSeverity.Info:
                gradient.Add(new GradientStop() { Color = ColorHelper.FromArgb(200, 211, 204, 227), Offset = 0.5 });
                gradient.Add(new GradientStop() { Color = ColorHelper.FromArgb(100, 211, 204, 227), Offset = 1 });
                break;
            case NotificationSeverity.Success:
                gradient.Add(new GradientStop() { Color = ColorHelper.FromArgb(255, 40, 199, 111), Offset = 0.5 });
                gradient.Add(new GradientStop() { Color = ColorHelper.FromArgb(200, 40, 199, 111), Offset = 1 });
                break;
            case NotificationSeverity.Warning:
                gradient.Add(new GradientStop() { Color = ColorHelper.FromArgb(255, 255, 210, 111), Offset = 0.5 });
                gradient.Add(new GradientStop() { Color = ColorHelper.FromArgb(200, 255, 210, 111), Offset = 1 });
                break;
            case NotificationSeverity.Error:
                gradient.Add(new GradientStop() { Color = ColorHelper.FromArgb(255, 233, 109, 113), Offset = 0.5 });
                gradient.Add(new GradientStop() { Color = ColorHelper.FromArgb(200, 233, 109, 113), Offset = 1 });

                break;
            default:
                gradient.Add(new GradientStop() { Color = Colors.Transparent });
                break;
        }

        return new LinearGradientBrush(gradient, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}
