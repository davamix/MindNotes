using CommunityToolkit.WinUI;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using MindNotes.Core.Models;
using System;
using Windows.ApplicationModel.DataTransfer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindNotes.Desktop.Controls;
public sealed partial class NotificationControl : UserControl {

    public static readonly DependencyProperty NotificationProperty =
        DependencyProperty.RegisterAttached(
            "Notification",
            typeof(Notification),
            typeof(NotificationControl),
            new PropertyMetadata(null, OnNotificationChanged)
    );

    public Notification Notification {
        get { return (Notification)GetValue(NotificationProperty); }
        set { SetValue(NotificationProperty, value); }
    }

    private static DispatcherTimer _dismissNotificationTimer;

    public NotificationControl() {
        this.InitializeComponent();

        _dismissNotificationTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(15) };
        _dismissNotificationTimer.Tick += _dismissNotificationTimer_Tick;
    }

    private static void SetIsOpenProperty(DependencyObject d, Notification notification) {
        var grid = d.FindDescendant("notificationPanel") as Grid;
        if (grid == null) return;

        if (notification.IsOpen) {
            grid.Visibility = Visibility.Visible;
            _dismissNotificationTimer.Start();
        } else {
            grid.Visibility = Visibility.Collapsed;
            _dismissNotificationTimer.Stop();
        }
    }

    private static void SetSeverityProperty(DependencyObject d, Notification notification) {
        var border = d.FindDescendant("innerPanel") as Border;
        if (border == null) return;

        var gradient = GetSeverityBackgroundColor(notification.Severity);

        border.Background = new LinearGradientBrush(gradient, 0);
    }

    private static void OnNotificationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        SetIsOpenProperty(d, e.NewValue as Notification);
        SetSeverityProperty(d, e.NewValue as Notification);
    }

    private static GradientStopCollection GetSeverityBackgroundColor(NotificationSeverity severity) {
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

        return gradient;
    }

    private void notificationControl_Loaded(object sender, RoutedEventArgs e) {
        notificationPanel.Visibility = Visibility.Collapsed;
    }

    private void _dismissNotificationTimer_Tick(object? sender, object e) {
        this.Notification.IsOpen = false;
        SetIsOpenProperty(this, this.Notification);
    }

    private void button_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    private void button_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }

    private void notification_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        fadeInButtonsStoryboard.Begin();
    }

    private void notification_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        fadeOutButtonsStoryboard.Begin();
    }

    private void btnCopyNotification_Click(object sender, RoutedEventArgs e) {
        var data = new DataPackage {
            RequestedOperation = DataPackageOperation.Copy
        };

        var content = $"{Notification.Message}\n{Notification.Content}";

        data.SetText(content);
        Clipboard.SetContent(data);
    }
}
