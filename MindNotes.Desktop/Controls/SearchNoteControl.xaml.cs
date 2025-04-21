using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindNotes.Desktop.Controls;
public sealed partial class SearchNoteControl : UserControl {
    public static readonly DependencyProperty SearchCommandProperty =
        DependencyProperty.RegisterAttached(
            "SearchCommand",
            typeof(RelayCommand<string>),
            typeof(SearchNoteControl),
            new PropertyMetadata(null)
        );

    public static readonly DependencyProperty SmartSearchCommandProperty =
        DependencyProperty.RegisterAttached(
            "SmartSearchCommand",
            typeof(RelayCommand<string>),
            typeof(SearchNoteControl),
            new PropertyMetadata(null)
        );

    public RelayCommand<string> SearchCommand {
        get { return (RelayCommand<string>)GetValue(SearchCommandProperty); }
        set { SetValue(SearchCommandProperty, value); }
    }

    public RelayCommand<string> SmartSearchCommand {
        get { return (RelayCommand<string>)GetValue(SmartSearchCommandProperty); }
        set { SetValue(SmartSearchCommandProperty, value); }
    }

    public SearchNoteControl() {
        this.InitializeComponent();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
        fadeInBackgroundBorderStoryboard.Begin();
        fadeInButtonsStoryboard.Begin();

    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
        fadeOutBackgroundBorderStoryboard.Begin();
        fadeOutButtonsStoryboard.Begin();
    }

    private void button_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    private void button_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }
}
