using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Animations;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using MindNotes.Core.Models;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.Gaming.XboxLive.Storage;
using Windows.UI.WebUI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindNotes.Desktop.Controls;
public sealed partial class NotificationControl : UserControl {

    public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.RegisterAttached(
            "IsOpen",
            typeof(bool),
            typeof(NotificationControl), 
            new PropertyMetadata(false, IsOpenPropertyChanged)
    );

    public static readonly DependencyProperty NotificationContentProperty =
        DependencyProperty.RegisterAttached(
            "NotificationContent",
            typeof(string),
            typeof(NotificationControl),
            new PropertyMetadata(string.Empty, NotificationContentPropertyChanged)
    );

    public static readonly DependencyProperty SeverityProperty = DependencyProperty.RegisterAttached(
            "Severity",
            typeof(NotificationSeverity),
            typeof(NotificationControl),
            new PropertyMetadata(NotificationSeverity.Info, SeverityPropertyChanged)
    );

    public bool IsOpen {
        get { return (bool)GetValue(IsOpenProperty); }
        set { SetValue(IsOpenProperty, value); }
    }
    public string NotificationContent {
        get { return (string)GetValue(NotificationContentProperty); }
        set { SetValue(NotificationContentProperty, value); }
    }
    public NotificationSeverity Severity {
        get { return (NotificationSeverity)GetValue(SeverityProperty); }
        set { SetValue(SeverityProperty, value); }
    }

    private static DispatcherTimer _dismissNotificationTimer;

    public NotificationControl() {
        this.InitializeComponent();

        _dismissNotificationTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(15) };
        _dismissNotificationTimer.Tick += _dismissNotificationTimer_Tick;
    }

    private void _dismissNotificationTimer_Tick(object? sender, object e) {
        Debug.WriteLine($"TICK: {sender}");

        this.IsOpen = false;
    }

    private static void IsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var grid = d.FindDescendant("notificationPanel") as Grid;
        if (grid == null) return;

        var isOpen = (bool)e.NewValue;

        if (isOpen) {
            grid.Visibility = Visibility.Visible;
            _dismissNotificationTimer.Start();
        } else {
            grid.Visibility = Visibility.Collapsed;
            _dismissNotificationTimer.Stop();
        }
    }

    private static void NotificationContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var textblock = d.FindDescendant("txtNotificationContent") as TextBlock;
        if (textblock == null) return;

        var content = (string)e.NewValue;
        textblock.Text = content;
    }

    private static void SeverityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var border = d.FindDescendant("innerPanel") as Border;
        if (border == null) return;

        var severity = (NotificationSeverity)e.NewValue;
        var gradient = GetSeverityBackgroundColor(severity);

        border.Background = new LinearGradientBrush(gradient, 0);
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

        data.SetText(txtNotificationContent.Text);
        Clipboard.SetContent(data);
    }



}
