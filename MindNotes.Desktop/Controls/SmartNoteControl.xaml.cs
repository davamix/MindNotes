using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Input;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindNotes.Desktop.Controls;

public sealed partial class SmartNoteControl : UserControl {
    public static readonly DependencyProperty ShowSmartNoteProperty =
        DependencyProperty.RegisterAttached(
            "ShowSmartNote",
            typeof(bool),
            typeof(SmartNoteControl),
            new PropertyMetadata(false, OnShowSmartNoteChanged)
        );

    public static readonly DependencyProperty NoteContentProperty =
        DependencyProperty.RegisterAttached(
            "NoteContent",
            typeof(string),
            typeof(SmartNoteControl),
            new PropertyMetadata(string.Empty, OnNoteContentChanged));

    

    public static readonly DependencyProperty NotesProperty =
        DependencyProperty.RegisterAttached(
            "Notes",
            typeof(List<string>),
            typeof(SmartNoteControl),
            new PropertyMetadata(new List<string>()));

    public string NoteContent {
        get { return (string)GetValue(NoteContentProperty); }
        set { SetValue(NoteContentProperty, value); }
    }

    public List<string> Notes {
        get { return (List<string>)GetValue(NotesProperty); }
        set { SetValue(NotesProperty, value); }
    }

    public bool ShowSmartNote {
        get { return (bool)GetValue(ShowSmartNoteProperty); }
        set { SetValue(ShowSmartNoteProperty, value); }
    }

    public SmartNoteControl() {
        this.InitializeComponent();
    }

    private static void OnShowSmartNoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var control = d as SmartNoteControl;
        if (control == null) return;

        var isOpen = (bool)e.NewValue;
        if (isOpen) {
            control.Visibility = Visibility.Visible;
            control.progressRing.IsActive = true;
        } else {
            control.Visibility = Visibility.Collapsed;
            control.NoteContent = string.Empty;
        }
    }

    private static void OnNoteContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var control = d as SmartNoteControl;
        if (control == null) return;

        control.progressRing.IsActive = false;
    }

    private void frontContent_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        fadeInButtonsStoryboard.Begin();
    }

    private void frontContent_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        fadeOutButtonsStoryboard.Begin();
    }

    private void button_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    private void button_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }

    private void btnClose_Click(object sender, RoutedEventArgs e) {
        ShowSmartNote = false;
    }

    private void sliderFontSize_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) {
        mkdContent.FontSize = e.NewValue;
    }
}
