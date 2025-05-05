using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MindNotes.Core.Models;
using Microsoft.UI.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls.Primitives;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindNotes.Desktop.Controls;

public sealed partial class BigNoteControl : UserControl {

    public static readonly DependencyProperty ShowNoteProperty =
        DependencyProperty.RegisterAttached(
            "ShowNote",
            typeof(bool),
            typeof(BigNoteControl),
            new PropertyMetadata(false, OnShowNoteChanged)
        );

    public static readonly DependencyProperty NoteProperty =
        DependencyProperty.RegisterAttached(
            "Note",
            typeof(Note),
            typeof(BigNoteControl),
            new PropertyMetadata(null)
        );


    public static readonly DependencyProperty SaveCommandProperty =
        DependencyProperty.RegisterAttached(
            "SaveCommand",
            typeof(RelayCommand<Note>),
            typeof(BigNoteControl),
            new PropertyMetadata(null)
        );

    public static readonly DependencyProperty DeleteCommandProperty =
        DependencyProperty.RegisterAttached(
            "DeleteCommand",
            typeof(RelayCommand<Note>),
            typeof(BigNoteControl),
            new PropertyMetadata(null)
        );

    public bool ShowNote {
        get { return (bool)GetValue(ShowNoteProperty); }
        set { SetValue(ShowNoteProperty, value); }
    }

    public Note Note {
        get { return (Note)GetValue(NoteProperty); }
        set { SetValue(NoteProperty, value); }
    }

    public RelayCommand<Note> SaveCommand {
        get { return (RelayCommand<Note>)GetValue(SaveCommandProperty); }
        set { SetValue(SaveCommandProperty, value); }
    }

    public RelayCommand<Note> DeleteCommand {
        get { return (RelayCommand<Note>)GetValue(DeleteCommandProperty); }
        set { SetValue(DeleteCommandProperty, value); }
    }


    public BigNoteControl() {
        this.InitializeComponent();

        txtContentFront.Config = new CommunityToolkit.Labs.WinUI.MarkdownTextBlock.MarkdownConfig();

        this.FontSize = 20;
    }

    private void Edit_Click(object sender, RoutedEventArgs e) {
        ShowBackView();
    }

    private void Save_Click(object sender, RoutedEventArgs e) {
        ShowFrontView();
    }
    private void Delete_Click(object sender, RoutedEventArgs e) {
        ShowFrontView();
        ShowNote = false;
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e) {
        ShowNote = false;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e) {
        ShowNote = false;
    }

    private void button_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    private void button_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }

    private void frontContent_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        fadeInButtonsStoryboard.Begin();
    }

    private void frontContent_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        fadeOutButtonsStoryboard.Begin();
    }

    private void txtContentBack_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e) {
        SetTextCounter();
    }

    private void sliderFontSize_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) {
        this.FontSize = e.NewValue;
    }

    private static void OnShowNoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var control = d as BigNoteControl;
        if (control == null) return;

        var isOpen = (bool)e.NewValue;
        if (isOpen) {
            control.Visibility = Visibility.Visible;
        } else {
            control.Visibility = Visibility.Collapsed;
        }
    }

    private void ShowBackView() {
        frontContent.Visibility = Visibility.Collapsed;
        backContent.Visibility = Visibility.Visible;

        backContent.Focus(FocusState.Programmatic);
        SetTextCounter();
    }

    private void ShowFrontView() {
        backContent.Visibility = Visibility.Collapsed;
        frontContent.Visibility = Visibility.Visible;
    }
    private void SetTextCounter() {
        //txtCounter.Text = $"({txtContentBack.Text.Length} / {txtContentBack.MaxLength})";
    }
}
