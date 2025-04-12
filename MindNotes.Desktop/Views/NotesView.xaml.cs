using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.WinUI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MindNotes.Desktop.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindNotes.Desktop.Views;
public sealed partial class NotesView : UserControl {
    public NotesView() {
        this.InitializeComponent();

        this.DataContext = Ioc.Default.GetRequiredService<NotesViewModel>();
    }

    private void button_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    private void button_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }

    private void btnSettings_Click(object sender, RoutedEventArgs e) {
        var mainGrid = (sender as DependencyObject).FindAscendant("mainGrid");

        if(mainGrid != null) {
            var settings = mainGrid.FindDescendant("settingsViewControl");

            if(settings != null) {
                settings.Visibility = Visibility.Visible;
            }
                
        }
    }
}
