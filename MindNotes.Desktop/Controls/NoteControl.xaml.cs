using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MindNotes.Core.Models;
using System;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindNotes.Desktop.Controls {
    public sealed partial class NoteControl : UserControl {
        public static readonly DependencyProperty NoteProperty =
        DependencyProperty.RegisterAttached(
            "Note",
            typeof(Note),
            typeof(NoteControl),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.RegisterAttached(
                "SaveCommand",
                typeof(RelayCommand<Note>),
                typeof(NoteControl),
                new PropertyMetadata(null)
            );

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.RegisterAttached(
                "DeleteCommand",
                typeof(RelayCommand<Note>),
                typeof(NoteControl),
                new PropertyMetadata(null)
            );

        public static readonly DependencyProperty ShowNoteCommandProperty =
            DependencyProperty.RegisterAttached(
                "ShowNoteCommand",
                typeof(RelayCommand<Note>),
                typeof(NoteControl),
                new PropertyMetadata(null)
            );

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

        public RelayCommand<Note> ShowNoteCommand {
            get { return (RelayCommand<Note>)GetValue(ShowNoteCommandProperty); }
            set { SetValue(ShowNoteCommandProperty, value); }
        }

        public NoteControl() {
            this.InitializeComponent();
        }

        private void Edit_Click(object sender, RoutedEventArgs e) {
            ShowBackView();
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            ShowFrontView();
        }
        private void Delete_Click(object sender, RoutedEventArgs e) {
            ShowFrontView();
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
            txtCounter.Text = $"({txtContentBack.Text.Length} / {txtContentBack.MaxLength})";
        }

        private async void mkdContent_LinkClicked(object sender, CommunityToolkit.WinUI.UI.Controls.LinkClickedEventArgs e) {
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
