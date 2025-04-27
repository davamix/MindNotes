using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using MindNotes.Desktop.ViewModels;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Media.Animation;
using CommunityToolkit.WinUI;
using MindNotes.Core.Models;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindNotes.Desktop.Views;

public sealed partial class NotificationsView : UserControl {
    private static DispatcherTimer _dismissNotificationTimer;

    public NotificationsView() {
        this.InitializeComponent();

        this.DataContext = Ioc.Default.GetRequiredService<NotificationsViewModel>();

        _dismissNotificationTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
        _dismissNotificationTimer.Tick += _dismissNotificationTimer_Tick;

        var context = DataContext as NotificationsViewModel;
        context.Notifications.CollectionChanged += Notifications_CollectionChanged;
        
    }

    private void notificationsView_Loaded(object sender, RoutedEventArgs e) {
        var context = DataContext as NotificationsViewModel;
        if (context == null) return;

        if (!context.Notifications.Any()) return;

        if (context.Notifications.Any(x => x.Severity == NotificationSeverity.Error)) {
            _dismissNotificationTimer.Stop();
        } else {
            _dismissNotificationTimer.Start();
        }
    }

    private void Notifications_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        var collection = sender as ObservableCollection<Notification>;
        if (collection == null) return;

        if (!collection.Any()) {
            _dismissNotificationTimer.Stop();
            notificationPanel.Visibility = Visibility.Collapsed;
            return;
        }

        if(collection.Any(x=>x.Severity == NotificationSeverity.Error)) {
            _dismissNotificationTimer.Stop();
        } else {
            _dismissNotificationTimer.Start();
        }
    }

    private void _dismissNotificationTimer_Tick(object? sender, object e) {
        notificationPanel.Visibility = Visibility.Collapsed;
    }

    private void button_PointerEntered(object sender, PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    private void button_PointerExited(object sender, PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }

    private void notificationItem_PointerEntered(object sender, PointerRoutedEventArgs e) {
        var storyboard = innerPanel.Resources["fadeInButtonsStoryboard"] as Storyboard;
        if (storyboard is null) return;

        storyboard.Stop();

        var grid = sender as Border; // Buttons container
        if (grid is null) return;

        SetAnimation(storyboard, grid);
    }

    private void notificationItem_PointerExited(object sender, PointerRoutedEventArgs e) {
        var storyboard = innerPanel.Resources["fadeOutButtonsStoryboard"] as Storyboard;
        if (storyboard is null) return;

        storyboard.Stop();

        var grid = sender as Border; // Buttons container
        if (grid is null) return;

        SetAnimation(storyboard, grid);
    }

    private static void SetAnimation(Storyboard storyboard, Border grid) {
        foreach (var animation in storyboard.Children) {
            Storyboard.SetTarget(animation, grid.FindChild<StackPanel>());
        }

        storyboard.Begin();
    }

    
}
